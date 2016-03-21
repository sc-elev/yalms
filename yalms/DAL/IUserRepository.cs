using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic; 
using yalms.Models; 

namespace yalms.DAL 
{ 

    public interface IUserRepository : IDisposable 
    {
        IEnumerable<ApplicationUser> GetAllUsers();
        ApplicationUser GetUser_SimpleByID(int userID);
        ApplicationUser GetUserByID(int id);

        ApplicationUser GetNewestUser();
        void InsertUser(ApplicationUser company, string id); 
        void DeleteUser(int idD);
        void UpdateUser(ApplicationUser company, string id); 
        void Save(); 


     } 
} 

