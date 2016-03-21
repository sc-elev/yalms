 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models; 

namespace yalms.DAL
{

    public class UploadRepository: IUploadRepository
    {
        // Get context for specific connectionstring.
        private EFContext context = new EFContext();



        #region Get all Uploads even those tagged as removed and not yet created.
        public IEnumerable<Upload> GetAllUploads()
        {
            return context.Uploads;
        }

        #endregion

        #region Get all Uploads one specific teacher
        public IEnumerable<Upload> GetAllUploadsByTeacherUserID(int teacherUserID)
        {

            return (from uplo in context.Uploads
                        join assi in context.Assignments on uplo.AssignmentID equals assi.AssignmentID
                        join cour in context.Courses on assi.AssignmentID equals cour.CourseID
                            where cour.Teacher_UserID == teacherUserID
                    select uplo);

        }
        #endregion

        #region Get all Uploads one specific school class
        public IEnumerable<Upload> GetAllUploadsBySchoolClassID(int schoolClassID)
        {

            return (from uplo in context.Uploads
                    join assi in context.Assignments on uplo.AssignmentID equals assi.AssignmentID
                    join cour in context.Courses on assi.AssignmentID equals cour.CourseID
                    where cour.SchoolClassID == schoolClassID
                    select uplo);

        }
        #endregion

        #region Get all Uploads one specific student
        public IEnumerable<Upload> GetAllUploadsByStudentUserID(int studentUserID)
        {

            return (from uplo in context.Uploads
                    join assi in context.Assignments on uplo.AssignmentID equals assi.AssignmentID
                    join cour in context.Courses on assi.AssignmentID equals cour.CourseID
                    join cost in context.Course_Students on cour.CourseID equals cost.CourseID
                    where cost.Course_StudentID == studentUserID
                    select uplo);
        }
        #endregion

        #region Get Upload by its Upload ID without populating foregin key data
        public Upload GetUpload_SimpleByID(int? uploadID)
        {
            // Get single Upload by its unique ID
            return context.Uploads.SingleOrDefault(o => o.UploadID == uploadID);

        }
        #endregion

        #region Get Upload by its Upload ID
        public Upload GetUploadByID(int? uploadID)
        {
            // Get single Upload by its unique ID
            var upload = context.Uploads.SingleOrDefault(o => o.UploadID == uploadID);


            return upload;
        }
        #endregion

        #region Get newest Upload.
        public Upload GetNewestUpload()
        {
           return context.Uploads.OrderByDescending(u => u.UploadID).FirstOrDefault();
        }
        #endregion

        #region Insert new Upload object.
        public void InsertUpload(Upload upload)
        {
            // Add Upload to context
            context.Uploads.Add(upload);

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Delete Upload  from database by Upload ID - Do not use unless sure it will not create data inconsistency and only if user is super Admin.
        public void DeleteUpload (int uploadID)
        {
            // Get Upload by ID.
            Upload upload = context.Uploads.SingleOrDefault(o => o.UploadID == uploadID);
            context.Uploads.Remove(upload);
        }
        #endregion

      
        #region Update existing Upload object and register what user modified it and when.
        public void UpdateUpload (Upload newUpload)
        {
            // Get existing Upload object by ID for update.
            var oldUpload = context.Uploads.SingleOrDefault(o => o.UploadID == newUpload.UploadID);
            oldUpload.Description = newUpload.Description;
            oldUpload.Grade = newUpload.Grade;
            oldUpload.GradeDescription = newUpload.GradeDescription;
            oldUpload.Uploaded = newUpload.Uploaded;
            oldUpload.UploadedBy = newUpload.UploadedBy;

            // Save context changes.
            Save();
            Dispose();
        }
        #endregion

        #region Update Upload with foreignkey names for presentation.
        private Upload PopulateUploadWithForeignKeyDataObjects(Upload upload)
        {
            // Get objects for Sub keys
            upload.Assignment = new AssignmentRepository().GetAssignmentByAssignmentID(upload.AssignmentID);
            return upload;
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

