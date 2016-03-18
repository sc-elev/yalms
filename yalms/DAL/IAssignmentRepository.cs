using System; 
using System.Data; 
using System.Data.Entity; 
using System.Collections.Generic;
using yalms.Models; 

namespace yalms.DAL 
{ 

    public interface IAssignmentRepository : IDisposable 
    { 

        IEnumerable<Assignment> GetAllAssignments(); 
        Assignment GetAssignment_SimpleByID(int? assignmentID); 
        Assignment GetAssignmentByID(int? assignmentID); 

        Assignment GetNewestAssignment(); 
        void InsertAssignment(Assignment company, int assignmentID); 
        void DeleteAssignment(int assignmentID); 
        void UpdateAssignment(Assignment company, int assignmentID); 
        void Save(); 


     } 
} 

