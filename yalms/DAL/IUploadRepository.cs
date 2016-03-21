using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

using yalms.Models; 

namespace yalms.DAL 
{ 

    public interface IUploadRepository : IDisposable 
    { 
        IEnumerable<Upload> GetAllUploads();

        IEnumerable<Upload> GetAllUploadsByTeacherUserID(int teacherUserID);
        IEnumerable<Upload> GetAllUploadsBySchoolClassID(int schoolClassID);
        IEnumerable<Upload> GetAllUploadsByStudentUserID(int studentUserID);

        Upload GetUpload_SimpleByID(int? uploadID); 
        Upload GetUploadByID(int? uploadID); 

        Upload GetNewestUpload(); 
        void InsertUpload(Upload company); 
        void DeleteUpload(int uploadID); 
        void UpdateUpload(Upload company); 
        void Save(); 


     } 
} 

