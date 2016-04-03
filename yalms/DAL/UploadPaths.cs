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
            string[] existing = FindSubmissionPaths(assignmentID, userID);
            string version = "0";
            if (existing.Length > 0)
            {
                var parent = Directory.GetParent(existing[0]).Name;
                int ix = int.Parse(parent); //FIXME - handle parse error
                version = (ix + 1).ToString();
            }
            var path = Path.Combine(
               "~", "Upload", "Submissions", userID.ToString(), 
               assignmentID.ToString(), version
            );
            path = System.Web.HttpContext.Current.Server.MapPath(path); //FIXME - testability.
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return Path.Combine(path, filename);
        }


        // Return all files found for given submission, sorted with newest path
        // first.
        static public string[] 
            FindSubmissionPaths(int assignmentID, int userID)
        {
            var dirpath = Path.Combine(
               "~", "Upload", "Submissions", userID.ToString(), 
               assignmentID.ToString()
               );
            dirpath = System.Web.HttpContext.Current.Server.MapPath(dirpath); //FIXME - testability
            if (!Directory.Exists(dirpath)) Directory.CreateDirectory(dirpath);
            string[] subdirs = Directory.GetDirectories(dirpath, "*");
            List<string> paths = new List<string>();
            foreach (var subdir in subdirs)
            {
                var tryDir = Path.Combine(dirpath, subdir);
                string[] leafs = Directory.GetFiles(tryDir, "*");
                foreach (var leaf in leafs) paths.Add(leaf);
            }
            paths.Sort();
            paths.Reverse();
            return paths.ToArray();
        }

        static public string[] 
            FindSubmissionURIs(int assignmentID, int userID)
        {
            var paths = FindSubmissionPaths(assignmentID, userID);
            var prefix = System.Web.HttpContext.Current.Server.MapPath("~"); //FIXME - testability
            for (int i = 0; i < paths.Length; i += 1)
            {
                paths[i] = paths[i].Replace(prefix, "/");
            }
            return paths;
        }


        // Return path for storing an assignment, presumably not used.
        static public string 
            GetAssignmentPath(int assignmentID, string filename)
        {
            return Path.Combine(
                "Upload", "Assignments", assignmentID.ToString(), filename);
        }

        
        // Return dir for storing an assignment, presumably not used.
        static public string GetAssignmentDir(int assignmentID)
        {
            return Path.Combine(
                "Upload", "Assignments", assignmentID.ToString());
        }



        // Return list of all paths with documents for this assignment,
        // sorted with newest first.
        static public string[] FindAssignments(int assignmentID)
        {
            string path = GetAssignmentPath(assignmentID, "foo");
            var dirpath = Path.GetDirectoryName(path);
            string[] found = Directory.GetFiles(dirpath, "*");
            Array.Sort(found);
            Array.Reverse(found);
            return found;
        }
    }
}