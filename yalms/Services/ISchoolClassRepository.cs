using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic; 
using yalms.Models; 

namespace yalms.Services 
{ 

    public interface ISchoolClassRepository : IDisposable 
    { 
        IEnumerable<SchoolClass> GetAllSchoolClasses();
        SchoolClass GetSchoolClassBySchoolClassID(int? schoolClassID);
        SchoolClass GetSchoolClassBySchoolClassID_Full(int? schoolClassID);

        SchoolClass GetNewestSchoolClass(); 
        void InsertSchoolClass(SchoolClass company); 
        void DeleteSchoolClass(int schoolClassID); 
        void UpdateSchoolClass(SchoolClass company); 
        void Save(); 


     } 
} 

