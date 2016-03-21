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
    public class StudentControllerTests
    {

        [Test]
        public void StudentCtrlReturnsValidDate()
        {
            IDateProvider today = new DummyDateProvider("2016-03-26");
            IUserProvider who = 
                new DummyUserProvider("J Edgar Hoover", "student");
            var _today = DateTime.Now.Date;
            var slots = new List<Slot> {
                new Slot {CourseID = 1, RoomID = 1, SlotID = 1, When = _today.AddHours(8) },
            };
            var context = new Mock<YalmContext>();
            context.Setup(x => x.GetSlots()).Returns(slots);
            var controller = new StudentController(who, today, context.Object);

            var action = (ViewResult)controller.MainView();

            Assert.AreEqual("2016-03-26", 
                            ((StudentMainViewModel)action.Model).Date);
                             
        }

        [Test]
        public void StudentCtrlReturnsValidSlots()
        {
            var userManager = new UserManager<MemoryUser>(new MemoryUserStore());
            var users = new List<ApplicationUser> { 
                new ApplicationUser("student1"),
                new ApplicationUser("student2"),
                new ApplicationUser("student3"),
                new ApplicationUser("teacher1"),
                new ApplicationUser("teacher2"),
                new ApplicationUser("user1"),
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
            var _today = DateTime.Now.Date;
            var slots = new List<Slot> {
                new Slot {CourseID = 1, RoomID = 1, SlotID = 1, When = _today.AddHours(8) },
                new Slot {CourseID = 2, RoomID = 1, SlotID = 1, When = _today.AddHours(9) },
                new Slot {CourseID = 3, RoomID = 1, SlotID = 1, When = _today.AddHours(10) },
                new Slot {CourseID = 1, RoomID = 1, SlotID = 1, When = _today.AddHours(12) },
                new Slot {CourseID = 1, RoomID = 1, SlotID = 1, When = _today.AddHours(13) },
                new Slot {CourseID = 2, RoomID = 1, SlotID = 1, When = _today.AddHours(14) },
                new Slot {CourseID = 2, RoomID = 1, SlotID = 1, When = _today.AddHours(32) },
                new Slot {CourseID = 3, RoomID = 1, SlotID = 1, When = _today.AddHours(33) },
                new Slot {CourseID = 3, RoomID = 1, SlotID = 1, When = _today.AddHours(34) },
            };
            var context = new Mock<YalmContext>();
            context.Setup(x => x.GetCourses()).Returns(courses);
            context.Setup(x => x.GetSlots()).Returns(slots);
            context.Setup(x => x.GetUsers()).Returns(users);
            context.Setup(x => x.GetSchoolClassStudents()).Returns(classMembers);
            context.Setup(x => x.GetSchoolClasses()).Returns(classes);

            IDateProvider today = new DummyDateProvider(DateTime.Now);
            IUserProvider who =
                new DummyUserProvider("J Edgar Hoover", "student");
            var mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(am => am.SignOut());
            mockAuthenticationManager.Setup(am => am.SignIn());
            var controller = new StudentController(who, today, context.Object);

            var action = (ViewResult)controller.MainView();
            StudentMainViewModel model = (StudentMainViewModel)action.Model;

            Assert.AreEqual(6, model.slots.Count());             
        }
    }
}
