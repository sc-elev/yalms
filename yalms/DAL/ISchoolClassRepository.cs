using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic; 
using yalms.Models; 

namespace yalms.DAL 
{ 

    public interface ISchoolClassRepository : IDisposable 
    { 
        IEnumerable<SchoolClass> GetAllSchoolClasses(); 
        SchoolClass GetSchoolClass_SimpleByID(int? schoolClassID); 
        SchoolClass GetSchoolClassByID(int? schoolClassID); 

        SchoolClass GetNewestSchoolClass(); 
        void InsertSchoolClass(SchoolClass company, int schoolClassID); 
        void DeleteSchoolClass(int schoolClassID); 
        void UpdateSchoolClass(SchoolClass company, int schoolClassID); 
        void Save(); 


     } 
} 

