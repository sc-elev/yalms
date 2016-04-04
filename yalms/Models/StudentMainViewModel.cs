using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yalms.DAL;

namespace yalms.Models
{

    public class StudentMainViewModelFactory
    {
        private Controller controller;
        private EFContext context;
        private IUserProvider userProvider;

        public StudentMainViewModelFactory(Controller controller,
                                           EFContext context,
                                           IUserProvider user)
        {
            this.controller = controller;
            this.context = context;
            this.userProvider = user;
        }


        public StudentMainViewModel Create( IDateProvider date, int DaysToAdd = 0)
        {
            StudentMainViewModel model =
                 controller.TempData["studentViewModel"] as StudentMainViewModel;

            var nextDate = date != null ? date.Today() : DateTime.Now.Date;
            if (model != null && date == null)
            {
                nextDate = model.Today;
            }
            nextDate = nextDate.AddDays(DaysToAdd);
            var nextDay = new DummyDateProvider(nextDate);
            model =
                new StudentMainViewModel(context, userProvider, nextDay);
            controller.TempData["studentViewModel"] = model;
            return model;
        }
    }


    public class AssignmentNode
    {
        public string Title { set; get; }
        public IList<Assignment> Assignments { set; get; }
        public IList<AssignmentNode> Children { set; get; }

        public AssignmentNode()
        {
            Assignments = new List<Assignment>();
            Children = new List<AssignmentNode>();
        }

        public AssignmentNode(string title): this()
        {
            Title = title;
        }
    }


    public class SubmissionsNode
    {
        public string Title { set; get; }
        public IList<Submission> Submissions { set; get; }
        public IList<SubmissionsNode> Children { set; get; }

        public SubmissionsNode()
        {
            Submissions = new List<Submission>();
            Children = new List<SubmissionsNode>();
        }

        public SubmissionsNode(string title)
            : this()
        {
            Title = title;
        }
    }


    public class StudentMainViewModel
    {
        public string WeekDay { set; get; }
        public int WeekNr  { set; get; }
        public string Date { set; get; }
        public DateTime Today { set; get; }

        public IList<Slot> slots { get; set; }
        public IList<TimingInfo> SlotTimings { get; set; }
        public string SchoolClass { get; set; }

        public IList<AssignmentNode> Assignments { set; get; }
        public SubmissionsNode SubmissionStates { set; get; }

        public int selectedAssignment { set; get; }
        public IList<SelectListItem> assignmentSelections { set; get; }

        public static StudentMainViewModel Create(
            Controller controller,
            EFContext context,
            UserProvider user,
            DateProvider date,
            int DaysToAdd = 0)
        {
            StudentMainViewModel model =
                 controller.TempData["studentViewModel"] as StudentMainViewModel;
            var nextDay =
                new DummyDateProvider(model.Today.AddDays(DaysToAdd));
            var dateProvider = new DummyDateProvider(nextDay.Today());
            model =
                new StudentMainViewModel(context, user,  dateProvider);
            controller.TempData["StudentViewModel"] = model;
            return model;
        }

        private IEnumerable<Submission> SubmissionsByState(
                       EFContext context,
                       IList<int> assignments, 
                       Submission.States state,
                       IUserProvider user)
        {
              return
                    from submission in context.GetSubmissions()
                    where assignments.Contains(submission.AssignmentID) &&
                        submission.State == state
                    join assignment in context.GetAssignments()
                        on submission.AssignmentID equals assignment.AssignmentID
                    select new Submission {
                        AssignmentID = assignment.AssignmentID,
                        State = Submission.States.New,
                        SubmissionTime = DateTime.Now,
                        UserID = user.UserID(),
                        assignment = assignment
                    };
        }


        private IEnumerable<SelectListItem> 
            BuildAssignmentSelections(EFContext context, IList<Course> courses)
        {
            List<int> courseIDs = courses.Select(c => c.CourseID).ToList();
            return
                from assignment in context.GetAssignments()
                where courseIDs.Contains(assignment.CourseID)
                join course in context.GetCourses() 
                    on assignment.CourseID equals course.CourseID
                select new SelectListItem {
                    Value = assignment.AssignmentID.ToString(),
                    Text = course.Name + "-" +
                        assignment.Name
                };
        }


        public StudentMainViewModel(EFContext context,
                                    IUserProvider user,
                                    IDateProvider dateProvider)
        {

            var repo = new SlotRepository(context);
            var result = repo.GetStudentsDailySheduleByStudentUserID(
                                        user.UserID(), dateProvider.Today());
            slots = new List<Slot>(result)
                            .OrderBy(w => w.SlotNR)
                            .ToList();
            var scs = context.GetSchoolClassStudents()
                .Where( s=> s.Student_UserID == user.UserID())
                .SingleOrDefault();
            var courses = context.GetCourses()
                .Where(c => c.SchoolClassID == scs.SchoolClassID)
                .ToList();
            assignmentSelections = 
                BuildAssignmentSelections(context, courses).ToList();
            Assignments = new List<AssignmentNode>();
            foreach (var course in courses)
            {
                var category = new AssignmentNode();
                category.Title = course.Name;
                category.Assignments = context.GetAssignments()
                    .Where(a => a.CourseID == course.CourseID)
                    .ToList();
                if (category.Assignments.Count > 0 )
                    Assignments.Add(category);
            }
            var approvedNode = new SubmissionsNode("Godkända");
            var rejectedNode = new SubmissionsNode("Ej godkända");
            foreach (var course in courses)
            {    
                var assignmentIDs = context.GetAssignments() 
                    .Where(a => a.CourseID == course.CourseID)
                    .Select(a => a.AssignmentID)
                    .ToList();
                var courseNode = new SubmissionsNode(course.Name);
                var query = SubmissionsByState(
                    context, assignmentIDs, Submission.States.Accepted, user);
                courseNode.Submissions = query.ToList();
                if (courseNode.Submissions.Count > 0)
                    approvedNode.Children.Add(courseNode);

                courseNode = new SubmissionsNode(course.Name);
                query = SubmissionsByState(
                     context, assignmentIDs, Submission.States.Rejected, user);
                courseNode.Submissions = query.ToList();
                if (courseNode.Submissions.Count > 0)
                    rejectedNode.Children.Add(courseNode);
            }
            SubmissionStates = new SubmissionsNode("");
            if (rejectedNode.Children.Count > 0)
                SubmissionStates.Children.Add(rejectedNode);
            if (approvedNode.Children.Count > 0)
                SubmissionStates.Children.Add(approvedNode);
  
            Today = dateProvider.Today();
            var cultureInfo = new System.Globalization.CultureInfo("sv-SE");
            var month = CultureInfo
                            .CurrentCulture
                            .DateTimeFormat
                            .GetAbbreviatedMonthName(Today.Month);
            Date = Today.Day + " " + month;
            WeekNr = cultureInfo.Calendar.GetWeekOfYear(
                Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            WeekDay = cultureInfo.DateTimeFormat.GetDayName(Today.DayOfWeek);
            SlotTimings = new List<TimingInfo>(SlotTimingInfo.Timings);
        }


        public StudentMainViewModel() { }
    }
}
