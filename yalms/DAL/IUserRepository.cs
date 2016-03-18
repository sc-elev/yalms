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
        DomainUser GetUser_SimpleByID(int? userID);
        DomainUser GetUserByID(int? userID);

        DomainUser GetNewestUser();
        void InsertUser(DomainUser company, int userID); 
        void DeleteUser(int userID);
        void UpdateUser(DomainUser company, int userID); 
        void Save(); 


     } 
} 

