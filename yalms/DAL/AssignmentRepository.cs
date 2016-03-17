 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models;

namespace yalms.DAL
{

   /// <summary>
   /// ---------------------------------------------------------------------------------------------------
   /// File Autogenerated at 2016-03-16 23:23:37
   /// Object derived from the  Assignment table in the LMSProject database.
   /// Solution version: 1.0.0.1
   /// [ ] Check box with 'x' to prevent entire file from being overwritten. 
   /// Do not move, remove or change the DMZ region markers. Code inside the DMZ will not be overwritten by auto generation.
   /// ---------------------------------------------------------------------------------------------------
   /// </summary>
    public class AssignmentRepository: IAssignmentRepository
    {
        // Get context for specific connectionstring.
        private EFContext context = new EFContext();



        #region Get all Assignments
        public IEnumerable<Assignment> GetAllAssignments()
        {
            return context.Assignments;
        }

        #endregion

        #region Get Assignment by its Assignment ID without populating foregin key data
        public Assignment GetAssignment_SimpleByID(int? assignmentID)
        {
            // Get single Assignment by its unique ID
            return context.Assignments.SingleOrDefault(o => o.AssignmentID == assignmentID);

        }
        #endregion

        #region Get Assignment by its Assignment ID
        public Assignment GetAssignmentByID(int? assignmentID)
        {
            // Get single Assignment by its unique ID
            var assignment = context.Assignments.SingleOrDefault(o => o.AssignmentID == assignmentID);


            return assignment;
        }
        #endregion

        #region Get newest Assignment.
        public Assignment GetNewestAssignment()
        {
           return context.Assignments.OrderByDescending(u => u.AssignmentID).FirstOrDefault();
        }
        #endregion

        #region Insert new Assignment object and register what user created it and when.
        public void InsertAssignment(Assignment assignment, int userID)
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



        #region Update existing Assignment object and register what user modified it and when.
        public void UpdateAssignment (Assignment newAssignment,int userID)
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

        #region Update Assignment with foreignkey names for presentation.
        private Assignment PopulateAssignmentWithForeignKeyDataObjects(Assignment assignment)
        {
            // Get objects for Sub keys
            assignment.CourseID_Course = new CourseRepository().GetCourse_SimpleByID(assignment.CourseID);
            return assignment;
        }
        #endregion



        #region System functions.
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region DMZ - Custome Code placed in the DMZ will be protected from system overwrites.



        #endregion DMZ End

    }
}

