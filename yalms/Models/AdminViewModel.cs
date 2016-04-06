using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yalms.DAL;

namespace yalms.Models
{
    public class AdminViewModel
    {
        private IList<int> studentIDs;
        private IList<int> teacherIDs;
        protected EFContext context; 
        protected IList<SelectListItem> _UnregisteredUsers;
        protected IList<SelectListItem> _Classes;
        protected IList<SelectListItem> _RegisteredUsers;
        protected IList<SelectListItem> _Courses;
        protected IList<SelectListItem> _Teachers;
        protected IList<ApplicationUser> _ClassList;
        protected Dictionary<char, IList<SelectListItem>> _StudentsByIndex;
        

        public IList<SelectListItem> UnregisteredUsers {
            set { _UnregisteredUsers = value; }
            get {
                if(_UnregisteredUsers != null) return _UnregisteredUsers;
                _UnregisteredUsers = context.GetUsers()
                    .Where(s => !studentIDs.Contains(s.Id))
                    .Where(s => !teacherIDs.Contains(s.Id))
                    .Select( s => new SelectListItem {
                        Text = s.UserName != null ? s.UserName : s.Email,
                        Value = s.Id.ToString()
                    })
                    .ToList();
                return _UnregisteredUsers;
            }
        }

        public IList<SelectListItem> Classes {
            set { _Classes = value; }
            get { 
                if (_Classes != null) return _Classes;
                _Classes = context.GetSchoolClasses()
                    .Select( s => new SelectListItem {
                        Text = s.Name,
                        Value = s.SchoolClassID.ToString()
                    } )
                    .ToList();
                return _Classes;
            }
        }


        public IList<SelectListItem> RegisteredUsers { 
            set { _RegisteredUsers =  value; }
            get {
                if (_RegisteredUsers != null) return _RegisteredUsers;
                _RegisteredUsers = context.GetUsers()
                    .Where(s => studentIDs.Contains(s.Id) || 
                        teacherIDs.Contains(s.Id))
                    .Select( s => new SelectListItem {
                        Text = s.UserName != null ? s.UserName : s.Email,
                        Value = s.Id.ToString()
                    })
                    .ToList();
                return _RegisteredUsers;
            }
        }

        public IList<SelectListItem> Courses  {
            set { _Courses = value; }
            get { 
                if (_Courses != null) return _Courses;
                _Courses = context.GetCourses()
                    .Join(context.GetSchoolClasses(),
                          course => course.SchoolClassID,
                          class_ => class_.SchoolClassID,
                          (course, class_) => new SelectListItem {
                              Value = course.CourseID.ToString(),
                              Text = course.Name + " - " + class_.Name
                          }
                    )
                    .ToList();
                return _Courses;
            }
        }


        public IList<SelectListItem> Teachers { 
            set { _Teachers =  value; }
            get {
                if (_Teachers != null) return _Teachers;
                _Teachers = context.GetUsers()
                    .Where(s =>  teacherIDs.Contains(s.Id))
                    .Select( s => new SelectListItem {
                        Text = s.UserName != null ? s.UserName : s.Email,
                        Value = s.Id.ToString()
                    })
                    .ToList();
                return _Teachers;
            }
        }


        public IList<ApplicationUser> ClassList
        {
            set { _ClassList =  value; }
            get {
                if (_ClassList != null) return _ClassList;
                if (SelectedClass == null || SelectedClass == 0)
                    return new List<ApplicationUser>();
                var userIDs = context.GetSchoolClassStudents()
                    .Where(s => s.SchoolClassID == SelectedClass)
                    .Select(s => s.Student_UserID)
                    .ToList();
                _ClassList = context.GetUsers()
                    .Where(u => userIDs.Contains(u.Id))
                    .Select(u => new ApplicationUser {
                        UserName = 
                            u.UserName != null && u.Email != u.UserName ? 
                                u.UserName: "-",
                        PhoneNumber = 
                            u.PhoneNumber != null ? u.PhoneNumber : "-",
                        Email = u.Email
                    })
                    .ToList();
                return _ClassList;
            }
        }


        public Dictionary<char, IList<SelectListItem>> StudentsByIndex { 
            set { _StudentsByIndex =  value; }
            get {
                if (_StudentsByIndex != null) return _StudentsByIndex;
                var students =  context.GetUsers()
                    .Where(s => studentIDs.Contains(s.Id)) 
                    .ToList();
                _StudentsByIndex = new Dictionary<char, IList<SelectListItem>>();
                foreach (var s in students)
                {
                    var name = s.UserName;
                    var key = name[0];
                    if (!_StudentsByIndex.Keys.Contains(key))
                        _StudentsByIndex[key] = new List<SelectListItem>();
                    _StudentsByIndex[key].Add(new SelectListItem {
                        Text = s.UserName != null ? s.UserName : s.Email,
                        Value = s.Id.ToString()
                    });   
                }
                return _StudentsByIndex;
            }
        }

        public int SelectedUser { set; get; }
        public string SelectedUsers { set; get; }
        public int SelectedClass { set; get; }
        public string SelectedClassname { set; get; }
        public string SelectedRole { set; get; }
        public string Role { set; get; }
        public int SelectedVictim { set; get; }
        public int SelectedTeacher { set; get; }
        public int SelectedCourse { set; get; }
        public string NewCourse { set; get; }


        public AdminViewModel()
        {
            context = new EFContext(); //DFIXME Needless use when fed from outside.
           _UnregisteredUsers = null;
           _Classes = null;
           _RegisteredUsers = null; 
           _Courses = null;
           _Teachers = null;
           _StudentsByIndex =  null;
        }

        public AdminViewModel(EFContext context): this()
        {
            this.context = context;

            var options = new IdentityFactoryOptions<ApplicationRoleManager>();
            options.DataProtectionProvider = null;
            options.Provider = null;
            var roleManager = ApplicationRoleManager.CreateFromDb(options, context);
            var studentRole = 
                roleManager.Roles.Where(r => r.Name == "student").Single();
            var teacherRole =
                roleManager.Roles.Where(r => r.Name == "teacher").Single();

            studentIDs = studentRole.Users.Select(u => u.UserId).ToList();
            teacherIDs = teacherRole.Users.Select(u => u.UserId).ToList();
        }
    }  
}