using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using yalms.Models;

namespace yalms.Services
{
    public interface ISubmissionRepository : IDisposable 
    {
        IEnumerable<Submission> GetAllSubmissionsByStateAndUser(
               EFContext context,
               IList<int> assignments,
               Submission.States state,
               IUserProvider user);
        void Save();

    }
}