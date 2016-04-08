
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models;
using yalms.DAL;

namespace yalms.Services
{

    public class SchoolClassRepository : BaseRepository, ISchoolClassRepository
    {
        public SchoolClassRepository() : base() { }

        public SchoolClassRepository(EFContext ctx) : base(ctx) { }

        public IEnumerable<SchoolClass> GetAllSchoolClasses()
        {
            return context.GetSchoolClasses();
        }

        #region Get SchoolClass by its SchoolClassID
        public SchoolClass GetSchoolClassBySchoolClassID(int? schoolClassID)
        {
            // Get single SchoolClass by its unique ID
            return context.GetSchoolClasses().SingleOrDefault(o => o.SchoolClassID == schoolClassID);

        }
        #endregion

        #region Get SchoolClass by its SchoolClassID with list of all students
        public SchoolClass GetSchoolClassBySchoolClassID_Full(int? schoolClassID)
        {
            var schoolClass = context.GetSchoolClasses().SingleOrDefault(o => o.SchoolClassID == schoolClassID);

            schoolClass.Students = new UserRepository(context).GetAllSchoolClassStudentsBySchoolClassID(schoolClass.SchoolClassID);

            return schoolClass;
        }
        #endregion

        #region Get SchoolClass by its SchoolClass ID
        public SchoolClass GetSchoolClassByID(int? schoolClassID)
        {
            // Get single SchoolClass by its unique ID
            var schoolClass = context.GetSchoolClasses().SingleOrDefault(o => o.SchoolClassID == schoolClassID);


            return schoolClass;
        }
        #endregion

        #region Get newest SchoolClass.
        public SchoolClass GetNewestSchoolClass()
        {
            return context.GetSchoolClasses().OrderByDescending(u => u.SchoolClassID).FirstOrDefault();
        }
        #endregion

        #region Insert new SchoolClass object and register what user created it and when.
        public void InsertSchoolClass(SchoolClass schoolClass)
        {
            // Add SchoolClass to context
            context.SchoolClasses.Add(schoolClass);

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Delete SchoolClass  from database by SchoolClass ID
        public void DeleteSchoolClass(int schoolClassID)
        {
            // Get SchoolClass by ID.
            SchoolClass schoolClass = context.GetSchoolClasses().SingleOrDefault(o => o.SchoolClassID == schoolClassID);
            context.SchoolClasses.Remove(schoolClass);
            Save();
        }
        #endregion

        #region Update existing SchoolClass object and register what user modified it and when.
        public void UpdateSchoolClass(SchoolClass newSchoolClass)
        {
            // Get existing SchoolClass object by ID for update.
            var oldSchoolClass = context.GetSchoolClasses().SingleOrDefault(o => o.SchoolClassID == newSchoolClass.SchoolClassID);
            oldSchoolClass.Name = newSchoolClass.Name;
            oldSchoolClass.SharedClassFolderUrl = newSchoolClass.SharedClassFolderUrl;
            oldSchoolClass.Year = newSchoolClass.Year;



            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

    }
}

