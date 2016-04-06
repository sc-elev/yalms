using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using yalms.Models;
using yalms.DAL;

namespace yalms.Services
{
    public class SubmissionRepository : ISubmissionRepository, IDisposable
    {
        private EFContext context;

        public SubmissionRepository()
        {
            context = new EFContext();
        }

        public SubmissionRepository(EFContext context)
        {
            this.context = context;
        }

        public IEnumerable<Submission> GetAllSubmissionsByStateAndUser(
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
                  select new Submission
                  {
                      AssignmentID = assignment.AssignmentID,
                      State = Submission.States.New,
                      SubmissionTime = DateTime.Now,
                      UserID = user.UserID(),
                      assignment = assignment
                  };
        }

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
    }
}