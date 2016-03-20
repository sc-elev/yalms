using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic; 
using yalms.Models; 

namespace yalms.DAL 
{ 

    public interface IUserRepository : IDisposable 
    {
        IEnumerable<DomainUser> GetAllUsers();
        DomainUser GetUser_SimpleByID(string userID);
        DomainUser GetUserByID(string id);

        DomainUser GetNewestUser();
        void InsertUser(DomainUser company, string id); 
        void DeleteUser(string idD);
        void UpdateUser(DomainUser company, string id); 
        void Save(); 


     } 
} 

