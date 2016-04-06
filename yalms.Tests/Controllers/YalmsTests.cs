using System;
using NUnit.Framework;
using yalms.Models;
using yalms.Controllers;
using System.Web.Mvc;
using Moq;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using yalms;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using yalms.DAL;

namespace yalms.Tests.Controllers
{
    public class MemoryUser : IUser{
        private readonly IList<UserLoginInfo> _logins;
        private readonly IList<Claim> _claims;
        private readonly IList<string> _roles;

        public MemoryUser(string name) {
            Id = Guid.NewGuid().ToString();
            _logins = new List<UserLoginInfo>();
            _claims = new List<Claim>();
            _roles = new List<string>();
            if (name.StartsWith("student")) _roles.Add("student");
            if (name.StartsWith("teacher")) _roles.Add("teacher");
            UserName = name;
        }

        public virtual string Id { get; set; }
        public virtual string UserName { get; set; }

        /// <summary>
        /// The salted/hashed form of the user password
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        public IList<UserLoginInfo> Logins { get { return _logins; } }

        public IList<Claim> Claims { get { return _claims; } }

        public IList<string> Roles { get { return _roles; } }
    }


    public class LoginComparer : IEqualityComparer<UserLoginInfo>
    {
        public bool Equals(UserLoginInfo x, UserLoginInfo y)
        {
            return x.LoginProvider == y.LoginProvider && x.ProviderKey == y.ProviderKey;
        }


        public int GetHashCode(UserLoginInfo obj)
        {
            return (obj.ProviderKey + "--" + obj.LoginProvider).GetHashCode();
        }
    }


    public class MemoryUserStore : IUserStore<MemoryUser>, IUserLoginStore<MemoryUser>, IUserRoleStore<MemoryUser>, IUserClaimStore<MemoryUser>, IUserPasswordStore<MemoryUser>, IUserSecurityStampStore<MemoryUser> {
        private Dictionary<string, MemoryUser> _users = new Dictionary<string, MemoryUser>();
        private Dictionary<UserLoginInfo, MemoryUser> _logins = new Dictionary<UserLoginInfo, MemoryUser>(new LoginComparer());

        public Task CreateAsync(MemoryUser user) {
            _users[user.Id] = user;
            return Task.FromResult(0);
        }

        public Task UpdateAsync(MemoryUser user) {
            _users[user.Id] = user;
            return Task.FromResult(0);
        }

        public Task<MemoryUser> FindByIdAsync(string userId) {
            if (_users.ContainsKey(userId)) {
                return Task.FromResult(_users[userId]);
            }
            return Task.FromResult<MemoryUser>(null);
        }

        public void Dispose() {
        }

        public IQueryable<MemoryUser> Users {
            get {
                return _users.Values.AsQueryable();
            }
        }

        public Task<MemoryUser> FindByNameAsync(string userName) {
            return Task.FromResult(Users.Where(u => u.UserName.ToUpper() == userName.ToUpper()).FirstOrDefault());
        }

        public Task AddLoginAsync(MemoryUser user, UserLoginInfo login) {
            user.Logins.Add(login);
            _logins[login] = user;
            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(MemoryUser user, UserLoginInfo login) {
            var logs = user.Logins.Where(l => l.ProviderKey == login.ProviderKey && l.LoginProvider == login.LoginProvider).ToList();
            foreach (var l in logs) {
                user.Logins.Remove(l);
                _logins[l] = null;
            }
            return Task.FromResult(0);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(MemoryUser user) {
            return Task.FromResult(user.Logins);
        }

        public Task<MemoryUser> FindAsync(UserLoginInfo login) {
            if (_logins.ContainsKey(login)) {
                return Task.FromResult(_logins[login]);
            }
            return Task.FromResult<MemoryUser>(null);
        }

        public Task AddToRoleAsync(MemoryUser user, string role) {
            user.Roles.Add(role);
            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(MemoryUser user, string role) {
            user.Roles.Remove(role);
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(MemoryUser user) {
            return Task.FromResult(user.Roles);
        }

        public Task<bool> IsInRoleAsync(MemoryUser user, string role) {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsAsync(MemoryUser user) {
            return Task.FromResult(user.Claims);
        }

        public Task AddClaimAsync(MemoryUser user, Claim claim) {
            user.Claims.Add(claim);
            return Task.FromResult(0);
        }

        public Task RemoveClaimAsync(MemoryUser user, Claim claim) {
            user.Claims.Remove(claim);
            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(MemoryUser user, string passwordHash) {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(MemoryUser user) {
            return Task.FromResult(user.PasswordHash);
        }

        public Task SetSecurityStampAsync(MemoryUser user, string stamp) {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(MemoryUser user) {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task DeleteAsync(MemoryUser user) {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(MemoryUser user) {
            return Task.FromResult(user.PasswordHash != null);
        }
    }


    [TestFixture]
    public class YalmsTests
    {

        protected Mock<EFContext> GetStandardContext(DateTime? when = null)
        {
            var users = new List<ApplicationUser> { 
                new ApplicationUser("student1", 3),
                new ApplicationUser("J Edgar Hoover", 4),
                new ApplicationUser("student3", 5),
                new ApplicationUser("teacher1", 1),
                new ApplicationUser("teacher2", 2),
                new ApplicationUser("user1", 6),
            };
            var courses = new List<Course> {
                new Course { Name = "kurs1", SchoolClassID = 1, 
                             Teacher_UserID = 1, CourseID = 1},
                new Course { Name = "kurs2", SchoolClassID = 1, 
                             Teacher_UserID = 2, CourseID = 2},
                new Course { Name = "kurs3", SchoolClassID = 2, 
                             Teacher_UserID = 2, CourseID = 3}
                // FIXME: Student_UserID => string
            };
            var classes = new List<SchoolClass> {
                new SchoolClass {Name = "7b", SchoolClassID = 1},
                new SchoolClass {Name = "7c", SchoolClassID = 2},
             };
            var classMembers = new List<SchoolClassStudent> {
                new SchoolClassStudent { SchoolClassID = 1, Student_UserID = 3},
                new SchoolClassStudent { SchoolClassID = 1, Student_UserID = 4},
                new SchoolClassStudent { SchoolClassID = 2, Student_UserID = 5},
            };
            
            var _today = (DateTime)(when != null ? when : DateTime.Now.Date);

            var slots = new List<Slot> {
                new Slot {CourseID = 1, RoomID = 1, SlotNR = 0, When = _today },
                new Slot {CourseID = 2, RoomID = 1, SlotNR = 1, When = _today },
                new Slot {CourseID = 3, RoomID = 1, SlotNR = 2, When = _today },
                new Slot {CourseID = 1, RoomID = 1, SlotNR = 3, When = _today },
                new Slot {CourseID = 1, RoomID = 1, SlotNR = 5, When = _today },
                new Slot {CourseID = 2, RoomID = 1, SlotNR = 6, When = _today },
                new Slot {CourseID = 2, RoomID = 1, SlotNR = 0, When = _today.AddDays(1) },
                new Slot {CourseID = 3, RoomID = 1, SlotNR = 1, When = _today.AddDays(1) },
                new Slot {CourseID = 3, RoomID = 1, SlotNR = 2, When = _today.AddDays(1) },
            };
            var rooms = new List<Room> {
                new Room { RoomID = 1, Description = "E265" }
            };

            var assignments = new List<Assignment> {
                new Assignment {AssignmentID = 1, CourseID = 1, 
                                StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(14),
                               Name = "Uppgift 1"},
                new Assignment {AssignmentID = 2, CourseID = 2, 
                                StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(3),
                               Name = "Uppgift 2"},                               
                new Assignment {AssignmentID = 3, CourseID = 2, 
                                StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(3),
                               Name = "Uppgift 3"},
                new Assignment {AssignmentID = 4, CourseID = 2, 
                                StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(3),
                                Name = "Uppgift 4"},
                new Assignment {AssignmentID = 5, CourseID = 2, 
                                StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(5),
                                Name = "Uppgift 5"}
            };

            var submissions = new List<Submission> {
                new Submission { AssignmentID = 1, State = Submission.States.New, UserID = 4},
                new Submission { AssignmentID = 2, State = Submission.States.Accepted, UserID = 4},
                new Submission { AssignmentID = 3, State = Submission.States.Rejected, UserID = 4},
                new Submission { AssignmentID = 4, State = Submission.States.Accepted, UserID = 4},
                new Submission { AssignmentID = 5, State = Submission.States.Accepted, UserID = 4},
            };

            var uploads = new List<Upload> {
                new Upload {AssignmentID  = 1, Description = "assignment 1",
                    Grade = 3, GradeDescription = "what grade?", 
                    SchoolClassID = 1, Uploaded = DateTime.Now.AddDays(-2),
                    UploadedBy = 3 },
                new Upload {AssignmentID  = 2, Description = "assignment 2",
                    Grade = 2, GradeDescription = "what grade?", 
                    SchoolClassID = 1, Uploaded = DateTime.Now.AddDays(-1),
                    UploadedBy = 3 },
                new Upload {AssignmentID  = 3, Description = "assignment 2A",
                    Grade = 3, GradeDescription = "what grade?", 
                    SchoolClassID = 2, Uploaded = DateTime.Now.AddDays(-1),
                    UploadedBy = 4 }
            };

            var context = new Mock<EFContext>();
            context.Setup(x => x.GetCourses()).Returns(courses);
            context.Setup(x => x.GetSlots()).Returns(slots);
            context.Setup(x => x.GetUsers()).Returns(users);
            context.Setup(x => x.GetSchoolClassStudents()).Returns(classMembers);
            context.Setup(x => x.GetSchoolClasses()).Returns(classes);
            context.Setup(x => x.GetRooms()).Returns(rooms);
            context.Setup(x => x.GetAssignments()).Returns(assignments);
            context.Setup(x => x.GetUploads()).Returns(uploads);
            context.Setup(x => x.GetSubmissions()).Returns(submissions);
            return context;
        }
    }
}
