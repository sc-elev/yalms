using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using yalms.Models;
using yalms.DAL;

namespace yalms.Services
{
    public class SubmissionRepository :BaseRepository, ISubmissionRepository
    {
               
        public SubmissionRepository() : base() {}

        public SubmissionRepository(EFContext ctx) : base(ctx) { }
 
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
    }
}