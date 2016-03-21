 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models; 

namespace yalms.DAL
{

    public class SchoolClassRepository: ISchoolClassRepository
    {
        // Get context for specific connectionstring.
        private EFContext context = new EFContext();


        #region Get all SchoolClasses.
        public IEnumerable<SchoolClass> GetAllSchoolClasses()
        {
            return context.SchoolClasses;
        }
        #endregion

        
        #region Get SchoolClass by its SchoolClassID
        public SchoolClass GetSchoolClassBySchoolClassID(int? schoolClassID)
        {
            // Get single SchoolClass by its unique ID
            return context.SchoolClasses.SingleOrDefault(o => o.SchoolClassID == schoolClassID);

        }
        #endregion

        #region Get SchoolClass by its SchoolClassID with list of all students
        public SchoolClass GetSchoolClassBySchoolClassID_Full(int? schoolClassID)
        {
            var schoolClass = context.SchoolClasses.SingleOrDefault(o => o.SchoolClassID == schoolClassID);

            schoolClass.Students = new UserRepository().GetAllSchoolClassStudentsBySchoolClassID(schoolClass.SchoolClassID);

            return schoolClass;
        }
        #endregion

        #region Get SchoolClass by its SchoolClass ID
        public SchoolClass GetSchoolClassByID(int? schoolClassID)
        {
            // Get single SchoolClass by its unique ID
            var schoolClass = context.SchoolClasses.SingleOrDefault(o => o.SchoolClassID == schoolClassID);


            return schoolClass;
        }
        #endregion

        #region Get newest SchoolClass.
        public SchoolClass GetNewestSchoolClass()
        {
           return context.SchoolClasses.OrderByDescending(u => u.SchoolClassID).FirstOrDefault();
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

        #region Delete SchoolClass  from database by SchoolClass ID - Do not use unless sure it will not create data inconsistency and only if user is super Admin.
        public void DeleteSchoolClass (int schoolClassID)
        {
            // Get SchoolClass by ID.
            SchoolClass schoolClass = context.SchoolClasses.SingleOrDefault(o => o.SchoolClassID == schoolClassID);
            context.SchoolClasses.Remove(schoolClass);
            Save();
        }
        #endregion



        #region Update existing SchoolClass object and register what user modified it and when.
        public void UpdateSchoolClass (SchoolClass newSchoolClass)
        {
            // Get existing SchoolClass object by ID for update.
            var oldSchoolClass = context.SchoolClasses.SingleOrDefault(o => o.SchoolClassID == newSchoolClass.SchoolClassID);
            oldSchoolClass.Name = newSchoolClass.Name;
            oldSchoolClass.SharedClassFolderUrl = newSchoolClass.SharedClassFolderUrl;
            oldSchoolClass.Year = newSchoolClass.Year;



            // Save context changes.
            Save();
            Dispose();
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


    }
}

