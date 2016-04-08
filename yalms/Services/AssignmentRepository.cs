 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models;
using System.Web.Mvc;
using yalms.DAL;

namespace yalms.Services
{

    public class AssignmentRepository: BaseRepository,  IAssignmentRepository
    {
        public AssignmentRepository() : base() {}

        public AssignmentRepository(EFContext ctx) : base(ctx) {}

        public List<int> GetAllAssignmentsIDsByCourseID(int courseID)
        {
            return context.GetAssignments()
                .Where(a => a.CourseID == courseID)
                .Select(a => a.AssignmentID)
                .ToList();
        }

        public IEnumerable<SelectListItem> BuildAssignmentSelections(IList<Course> courses)//EFContext context,
        {
            List<int> courseIDs = courses.Select(c => c.CourseID).ToList();
            return
                from assignment in context.GetAssignments()
                where courseIDs.Contains(assignment.CourseID)
                join course in context.GetCourses()
                    on assignment.CourseID equals course.CourseID
                select new SelectListItem
                {
                    Value = assignment.AssignmentID.ToString(),
                    Text = course.Name + "-" +
                        assignment.Name
                };
        }




        public IEnumerable<Assignment> GetAllAssignments()
        {
            return context.GetAssignments();
        }


        public List<Assignment> GetAllAssignmentsByCourseID(int courseID)
        {
            return (from assi in context.GetAssignments()
                        where assi.CourseID == courseID
                        select assi
                        ).ToList();
        }



        public Assignment GetAssignmentByAssignmentID(int? assignmentID)
        {
            // Get single Assignment by its unique ID
            var assignment = context.GetAssignments().SingleOrDefault(o => o.AssignmentID == assignmentID);


            return assignment;
        }

        #region Get newest Assignment.
        public Assignment GetNewestAssignment()
        {
           return context.Assignments.OrderByDescending(u => u.AssignmentID).FirstOrDefault();
        }
        #endregion

        #region Insert new Assignment object and register what user created it and when.
        public void InsertAssignment(Assignment assignment)
        {
            // Add Assignment to context
            context.Assignments.Add(assignment);

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Delete Assignment  from database by Assignment ID - Do not use unless sure it will not create data inconsistency and only if user is super Admin.
        public void DeleteAssignment (int assignmentID)
        {
            // Get Assignment by ID.
            Assignment assignment = context.Assignments.SingleOrDefault(o => o.AssignmentID == assignmentID);
            context.Assignments.Remove(assignment);
        }
        #endregion

        #region Update existing Assignment object.
        public void UpdateAssignment (Assignment newAssignment)
        {
            // Get existing Assignment object by ID for update.
            var oldAssignment = context.Assignments.SingleOrDefault(o => o.AssignmentID == newAssignment.AssignmentID);
            oldAssignment.EndTime = newAssignment.EndTime;
            oldAssignment.PathUrl = newAssignment.PathUrl;
            oldAssignment.StartTime = newAssignment.StartTime;

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

    }
}

