using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using yalms.Models;

namespace yalms.Content.Controllers
{
    public class AdminController : Controller
    {
        protected IDateProvider dateProvider;

        protected IUserProvider userProvider;

        protected EFContext context;

        protected ApplicationUserManager UserManager;
        // GET: Admin
        public ActionResult Index()
        {
            AdminViewModel model = new AdminViewModel(context);
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> RegisterUser(AdminViewModel modelArg)
        {
            var model = new AdminViewModel(context);
            int userId = modelArg.SelectedUser;
            var user = context.GetUsers()
                            .Where(u => u.Id == userId)
                            .SingleOrDefault();
            if (user == null)
            {
                ViewBag.Message = "Felaktig användare (!)";
                model = new AdminViewModel(context);
                return View("Index", model);
            }
            if (modelArg.SelectedRole == "student")
            {
                await UserManager.AddToRoleAsync(userId, "student");
            }
            if (modelArg.SelectedRole == "teacher")
            {
                await UserManager.AddToRoleAsync(userId, "teacher");
            }
            int classId = modelArg.SelectedClass;
            var scs = context.SchoolClassStudents
                           .Where(s => s.Student_UserID == userId)
                           .SingleOrDefault();
            if (scs != null) context.SchoolClassStudents.Remove(scs);
            scs = new SchoolClassStudent
            {
                SchoolClassID = modelArg.SelectedClass,
                Student_UserID = userId
            };
            context.SchoolClassStudents.Add(scs);
            context.SaveChanges();
            model = new AdminViewModel(context);
            return View("Index", model);
        }


        [HttpPost]
        public async Task<ActionResult> RemoveUser(AdminViewModel modelArg)
        {
            var model = new AdminViewModel(context);
            int userId = modelArg.SelectedUser;
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.Message = "Felaktig användare (!)";
                return View("Index", model);
            }
            foreach (var login in user.Logins.ToList())
            {
                await UserManager.RemoveLoginAsync(
                    login.UserId,
                    new UserLoginInfo(login.LoginProvider, login.ProviderKey));
            }
            var rolesForUser = await UserManager.GetRolesAsync(userId);
            if (rolesForUser.Count() > 0)
            {
                foreach (var item in rolesForUser.ToList())
                {
                    // item should be the name of the role
                    var result =
                        await UserManager.RemoveFromRoleAsync(user.Id, item);
                }
            }
            ViewBag.Message = "Användare bortttagen";
            model = new AdminViewModel(context);
            return View("Index", model);
        }


        [HttpPost]
        public ActionResult AddCourse(AdminViewModel modelArg)
        {
            AdminViewModel model = new AdminViewModel(context);
            string name = modelArg.NewCourse;
            if (name == null || name.Length == 0) {
                ViewBag.Message = "Obligatoriskt namn saknas";
                return View("Index", model);
            }
            int teacherId = modelArg.SelectedTeacher;
            var teacher = context.GetUsers()
                            .Where(u => u.Id == teacherId)
                            .SingleOrDefault();
            if (teacher == null) {
                ViewBag.Message = "Felaktig lärare (!)";
                return View("Index", model);
            }
            var classID = modelArg.SelectedClass;
            var class_ = context.GetSchoolClasses()
                             .Where(s => s.SchoolClassID == classID)
                             .SingleOrDefault();
            if (class_ == null) {
                ViewBag.Message = "Felaktig klass (!)";
                return View("Index", model);
            }
            var sc = new Course {
                Name = name,
                SchoolClassID = classID,
                Teacher_UserID = teacherId
            };
            context.Courses.Add(sc);
            context.SaveChanges();
            model = new AdminViewModel(context);
            return View("Index", model);
        }


        [HttpPost]
        public ActionResult RemoveCourse(AdminViewModel modelArg)
        {
            AdminViewModel model = new AdminViewModel(context);
            int courseID = modelArg.SelectedCourse;
            var course = context.GetCourses()
                             .Where(c => c.CourseID == courseID)
                             .SingleOrDefault();
            if (course == null) {
                ViewBag.Message = "Felaktig kurs (!)";
                return View("Index", model);
            }
            context.Courses.Remove(course);
            context.SaveChanges();
            model = new AdminViewModel(context);
            return View("Index", model);
        }


        [HttpPost]
        public ActionResult AddClass(AdminViewModel modelArg)
        {
            AdminViewModel model = new AdminViewModel(context);
            if (modelArg.SelectedClassname == null)
            {
                ViewBag.Message = "Nothing selected?!";
                return View("Index", model);
            }
            var classnames =
                context.GetSchoolClasses().Select(c => c.Name).ToList();
            if (classnames.Contains(modelArg.SelectedClassname))
            {
                ViewBag.Message = "Klassen finns redan";
                return View("Index", model);
            }
            var sc = new SchoolClass { Name = modelArg.SelectedClassname };
            context.SchoolClasses.Add(sc);
            context.SaveChanges();
            model = new AdminViewModel(context);
            ViewBag.Message =
                " Ny klass " + modelArg.SelectedClassname + " skapad.";
            return View("Index", model);
        }


        [HttpPost]
        public ActionResult RemoveClass(AdminViewModel modelArg)
        {
            AdminViewModel model;
            int classId = modelArg.SelectedClass;
            var victim = context.GetSchoolClasses()
                .Where(s => s.SchoolClassID == classId)
                .SingleOrDefault();
            if (victim == null)
            {
                model = new AdminViewModel(context);
                ViewBag.Message = "Klassen fimnns inte (!)";
                return View("Index", model);
            }
            context.SchoolClasses.Remove(victim);
            context.SaveChanges();
            ViewBag.Message = "Klass " + victim.Name + " borttagen.";
            model = new AdminViewModel(context);
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult ShowClassList(AdminViewModel modelArg)
        {
            AdminViewModel model = new AdminViewModel(context);
            if (modelArg.SelectedClassname == null)
            {
                ViewBag.Message = "Nothing selected?!";
                return View("Index", model);
            }
            return View("Index", model);
        }


        [HttpGet]
        public ActionResult GetClassList(int SelectedClass)
        {
            var model = new AdminViewModel(context);
            model.SelectedClass = SelectedClass;
            return PartialView("ClassList", model);
        }


        public ActionResult  AddClassStudents(AdminViewModel modelArg)
        {
            
            AdminViewModel model = new AdminViewModel(context);
            if (modelArg.SelectedUsers == null)
            {
                ViewBag.Message = "No user selected?!";
                return View("Index", model);
            }
            var class_ = context.GetSchoolClasses()
                           .Where(s => s.SchoolClassID == modelArg.SelectedClass)
                           .SingleOrDefault();
            if (class_ == null)
            {
                ViewBag.Message = "No valid class selected?!";
                return View("Index", model);
            }
            var stringIDs = modelArg.SelectedUsers.Split(',');
            foreach (var strID in stringIDs)
            {
                int id;
                if (!int.TryParse(strID, out id))
                    continue;
                var scs = context.GetSchoolClassStudents()
                    .Where(s => s.Student_UserID == id)
                    .SingleOrDefault();
                if (scs != null) context.SchoolClassStudents.Remove(scs);
                scs = new SchoolClassStudent {
                    Student_UserID = id,
                    SchoolClassID = modelArg.SelectedClass
                };
                context.SchoolClassStudents.Add(scs);
                context.SaveChanges();
            }
            model = new AdminViewModel(context);
            return View("Index", model);
        }


        public AdminController(
            IUserProvider user, IDateProvider date, EFContext ctx)
        {
            dateProvider = date;
            userProvider = user != null ? user : new UserProvider(this);
            userProvider = user;
            context = ctx;
            var userOptions = 
                new IdentityFactoryOptions<ApplicationUserManager>();
            userOptions.DataProtectionProvider = null;
            userOptions.Provider = null;
            UserManager = 
                ApplicationUserManager.CreateFromDb(userOptions, context);
        }


        public AdminController()
            : this(null, new DateProvider(),  new EFContext()) {}
       
    }
}