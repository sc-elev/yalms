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
        List<Assignment> GetAllAssignmentsByCourseID(int courseID);
        Assignment GetAssignmentByAssignmentID(int? assignmentID); 

        Assignment GetNewestAssignment(); 
        void InsertAssignment(Assignment company); 
        void DeleteAssignment(int assignmentID); 
        void UpdateAssignment(Assignment company); 
        void Save(); 


     } 
} 

