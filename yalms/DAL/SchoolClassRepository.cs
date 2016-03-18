 
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


        #region Get all SchoolClasses even those tagged as removed and not yet created.
        public IEnumerable<SchoolClass> GetAllSchoolClasses()
        {
            return context.SchoolClasses;
        }

        #endregion

        #region Get SchoolClass by its SchoolClass ID without populating foregin key data
        public SchoolClass GetSchoolClass_SimpleByID(int? schoolClassID)
        {
            // Get single SchoolClass by its unique ID
            return context.SchoolClasses.SingleOrDefault(o => o.SchoolClassID == schoolClassID);

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
        public void InsertSchoolClass(SchoolClass schoolClass, int userID)
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
        }
        #endregion

        #region Tag SchoolClass as removed, and register what user removed it and when.
        public void RemoveSchoolClass(SchoolClass newSchoolClass, int userID)
        {
            // Get SchoolClass for update
            var oldSchoolClass = context.SchoolClasses.Single(o => o.SchoolClassID == newSchoolClass.SchoolClassID);


            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Update existing SchoolClass object and register what user modified it and when.
        public void UpdateSchoolClass (SchoolClass newSchoolClass,int userID)
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

        #region Update SchoolClass with foreignkey names for presentation.
        private SchoolClass PopulateSchoolClassWithForeignKeyDataObjects(SchoolClass schoolClass)
        {
            // Get objects for Sub keys
            return schoolClass;
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

