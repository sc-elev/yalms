using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic; 
using yalms.Models; 

namespace yalms.Services 
{ 

    public interface IUserRepository : IDisposable 
    {
        List<ApplicationUser> GetAllSchoolClassStudentsBySchoolClassID(int? schoolClassID);

        IEnumerable<ApplicationUser> GetAllUsers();
        ApplicationUser GetUser_SimpleByID(int userID);
        ApplicationUser GetUserByID(int id);

        ApplicationUser GetNewestUser();
        void InsertUser(ApplicationUser company); 
        void DeleteUser(int idD);
        void UpdateUser(ApplicationUser company); 
        void Save(); 


     } 
} 

