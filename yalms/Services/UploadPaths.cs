using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using yalms.Models;

namespace yalms.Services
{
    public class UploadPaths
    {
        // Map ~-prefixed path to server path if available, else to
        // curerent dir (unit testing).
        private static string MapPath(string path)
        {
            return path.Replace("~", AppDomain.CurrentDomain.BaseDirectory);
        }


        // Danger zone: Re-initialize the Upload directory structure
        public static void seedUploads()
        {
            string[] paths = new string[] {
                "~/Upload", "~/Upload/Assignments", "~/Upload/Submissions",
                "~/Upload/Shared"};
            foreach (var path in paths)
            {
                var dirpath = MapPath(path);
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
                "~", "Upload", "Submissions",  assignmentID.ToString(),
                userID.ToString(), version
            );
            path = MapPath(path);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return Path.Combine(path, filename);
        }


        // Return all files found for given submission, sorted with newest path
        // first.
        static public string[] 
            FindSubmissionPaths(int assignmentID, int userID)
        {
            var dirpath = Path.Combine(
               "~", "Upload", "Submissions",assignmentID.ToString(),
               userID.ToString()
            );
            dirpath = MapPath(dirpath); 
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
            var prefix = MapPath("~"); 
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
            var path =  Path.Combine(
                "~", "Upload", "Assignments", assignmentID.ToString(), 
                Path.GetFileName(filename)
            );
            return MapPath(path);
        }


        // Return list of all paths with documents for this assignment and
        // given user, sorted with newest first. User == -1 lists all user's docs.
        static public string[] FindAssignmentDocs(int assignmentID, int userId = -1)
        {
            string[] found;
            if (userId != -1)
            {
                string path = GetAssignmentPath(assignmentID,  "foo");
                var dirpath = Path.GetDirectoryName(path);
                found = Directory.GetFiles(dirpath, "*");
            }
            else
            {
                var users = UsersByAssignment(assignmentID);
                IList<string> dirpaths = new List<string>();
                foreach(var user in users)
                {
                    var userparts = FindAssignmentDocs(assignmentID, user);
                    foreach (var part in userparts) dirpaths .Add(part);
                }
                found = dirpaths.ToArray();
            }
            Array.Sort(found);
            Array.Reverse(found);
            return found;
        }

        // All users which have submitted input for given assignment
        static public IList<int> UsersByAssignment(int assignmentID)
        {
            string path = 
                Path.Combine("~", "Upload", "Submissions", assignmentID.ToString());
            path = MapPath(path);
            string[] dirs = Directory.GetDirectories(path);
            var found = new List<int>();
            foreach (var dir in dirs)
            {
                int id;
                if (int.TryParse(dir, out id))
                    found.Add(id);
            }
            return found.ToArray();
        }

        static public string FindAssignmentPath(int assignmentID)
        {
            var dirpath =  Path.Combine(
              "~",  "Upload", "Assignments", assignmentID.ToString());
            dirpath = MapPath(dirpath);
            var files = Directory.GetFiles(dirpath, "*");
            var prefix = MapPath("~");
            return files.Length > 0 ? files[0].Replace(prefix, "") : null;
        }
    }
}