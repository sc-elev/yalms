using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using yalms.Models;

namespace yalms.DAL
{
    public class UploadPaths
    {
        // Danger zone: Re-initialize the Upload directory structure
        public static void seedUploads()
        {
            string[] paths = new string[] {
                "~/Upload", "~/Upload/Assignments", "~/Upload/Submissions",
                "~/Upload/Shared"};
            foreach (var path in paths)
            {
                var dirpath =
                    System.Web.HttpContext.Current.Server.MapPath(path);
                if (Directory.Exists(dirpath)) Directory.Delete(dirpath, true);
                Directory.CreateDirectory(dirpath);
            }
        }


        // Get path for given user + assignment, possibly creating missing 
        // directories in path. 
        static public string GetSubmissionPath(
            int assignmentID, int userID, string filename)
        {
            string [] parts = { 
                "~", "Upload", "Submissions", userID.ToString()};
            var path = Path.Combine(parts);
            path = System.Web.HttpContext.Current.Server.MapPath(path); //FIXME - testability.
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return Path.Combine(
                path, assignmentID.ToString() + "-" + filename);
        }


        // Return all files found for given submission, sorted with newest path
        // first.
        static public string[] 
            FindSubmissionPaths(int assignmentID, int userID, int version=0)
        {
            string path = GetSubmissionPath(assignmentID,  userID,  "");
            var dirpath = Path.GetDirectoryName(path);
            var filename = Path.GetFileName(path);
            string[] found = Directory.GetFiles(dirpath, filename + "*");
            Array.Sort(found);
            Array.Reverse(found);
            return found;
        }

        // Return path for storing an assignment, presumable not used.
        static public string GetAssignmentPath(int assignmentID)
        {
            string[] parts = new string[] { 
                "Upload", "Assignments", assignmentID.ToString()};
            return Path.Combine(parts);
        }

        // Return list of all paths with documents for this assignment,
        // sorted with newest first.
        static public string[] FindAssignments(int assignmentID)
        {
            string path = GetAssignmentPath(assignmentID);
            var dirpath = Path.GetDirectoryName(path);
            var filename = Path.GetFileName(path);
            string[] found = Directory.GetFiles(dirpath, filename + "*");
            Array.Sort(found);
            Array.Reverse(found);
            return found;
        }
    }
}