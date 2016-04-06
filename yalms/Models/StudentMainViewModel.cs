using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yalms.Services;
using yalms.DAL;

namespace yalms.Models
{
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

        // Declare repositorys here
        private ISubmissionRepository submissionRepository;
        private IAssignmentRepository assignmentRepository;
        private ISchoolClassStudentRepository schoolClassStudentRepository;
        private ISlotRepository slotRepository;
        private ICourseRepository courseRepository;

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

        public StudentMainViewModel(EFContext context,
                                    IUserProvider user,
                                    IDateProvider dateProvider)
        {
            // make sure that all repositories gets the same and correct context
            this.submissionRepository = new SubmissionRepository(context);
            this.assignmentRepository = new AssignmentRepository(context);
            this.schoolClassStudentRepository = new SchoolClassStudentRepository(context);
            this.slotRepository = new SlotRepository(context);
            this.courseRepository = new CourseRepository(context);

            var result = slotRepository.GetStudentsDailySheduleByStudentUserID(
                                        user.UserID(), dateProvider.Today());

            slots = new List<Slot>(result)
                            .OrderBy(w => w.SlotNR)
                            .ToList();

            var scs = schoolClassStudentRepository.GetSchoolClassStudentByID(user.UserID());
            var courses = courseRepository.GetAllCoursesBySchoolClassID(scs.SchoolClassID);

            assignmentSelections = assignmentRepository.BuildAssignmentSelections(courses).ToList();

            Assignments = new List<AssignmentNode>();
            foreach (var course in courses)
            {
                var category = new AssignmentNode();
                category.Title = course.Name;
                category.Assignments = assignmentRepository.GetAllAssignmentsByCourseID(course.CourseID);

                if (category.Assignments.Count > 0 )
                    Assignments.Add(category);
            }
            var approvedNode = new SubmissionsNode("Godkända");
            var rejectedNode = new SubmissionsNode("Ej godkända");
            foreach (var course in courses)
            {
                var assignmentIDs = assignmentRepository.GetAllAssignmentsIDsByCourseID(course.CourseID);
                var courseNode = new SubmissionsNode(course.Name);
                var query = submissionRepository.GetAllSubmissionsByStateAndUser(
                    context, assignmentIDs, Submission.States.Accepted, user);
                courseNode.Submissions = query.ToList();
                if (courseNode.Submissions.Count > 0)
                    approvedNode.Children.Add(courseNode);

                courseNode = new SubmissionsNode(course.Name);
                query = submissionRepository.GetAllSubmissionsByStateAndUser(
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
    }
}
