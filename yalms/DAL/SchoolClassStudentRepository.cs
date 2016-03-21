 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models; 

namespace yalms.DAL
{

    public class SchoolClassStudentRepository: ISchoolClassStudentRepository
    {
        // Get context for specific connectionstring.
        private EFContext context = new EFContext();


        #region Get all SchoolClassStudents.
        public IEnumerable<SchoolClassStudent> GetAllSchoolClassStudents()
        {
            return context.SchoolClassStudents;
        }
        #endregion

        #region Get SchoolClassStudent by its SchoolClassStudent ID without populating foregin key data
        public IEnumerable<SchoolClassStudent> GetAllSchoolClassStudentsBySchoolClassID(int? schoolClassID)
        {
            return (from scs in context.SchoolClassStudents
                    where scs.SchoolClassID == schoolClassID
                    select scs
                );     
        }
        #endregion

        #region Get SchoolClassStudent by its SchoolClassStudent ID
        public SchoolClassStudent GetSchoolClassStudentByID(int? schoolClassStudentID)
        {
            // Get single SchoolClassStudent by its unique ID
            var schoolClassStudent = context.SchoolClassStudents.SingleOrDefault(o => o.SchoolClassStudentID == schoolClassStudentID);

            return schoolClassStudent;
        }
        #endregion

        #region Get newest SchoolClassStudent.
        public SchoolClassStudent GetNewestSchoolClassStudent()
        {
           return context.SchoolClassStudents.OrderByDescending(u => u.SchoolClassStudentID).FirstOrDefault();
        }
        #endregion

        #region Insert new SchoolClassStudent object and register what user created it and when.
        public void InsertSchoolClassStudent(SchoolClassStudent schoolClassStudent)
        {


            // Add SchoolClassStudent to context
            context.SchoolClassStudents.Add(schoolClassStudent);

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Delete SchoolClassStudent  from database by SchoolClassStudent ID - Do not use unless sure it will not create data inconsistency and only if user is super Admin.
        public void DeleteSchoolClassStudent (int schoolClassStudentID)
        {
            // Get SchoolClassStudent by ID.
            SchoolClassStudent schoolClassStudent = context.SchoolClassStudents.SingleOrDefault(o => o.SchoolClassStudentID == schoolClassStudentID);
            context.SchoolClassStudents.Remove(schoolClassStudent);
            Save();
        }
        #endregion



        #region Update existing SchoolClassStudent object and register what user modified it and when.
        public void UpdateSchoolClassStudent (SchoolClassStudent newSchoolClassStudent)
        {
            // Get existing SchoolClassStudent object by ID for update.
            var oldSchoolClassStudent = context.SchoolClassStudents.SingleOrDefault(o => o.SchoolClassStudentID == newSchoolClassStudent.SchoolClassStudentID);

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

