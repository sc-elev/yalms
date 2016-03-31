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
        public void seedUploads()
        {
            string[] paths = new string[] {
                "~/Upload", "~/Upload/Assignments", "~/Upload/Shared"};
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
        static public string GetAssignmentPath(
            int userID, int assignmentID, string filename)
        {
            string [] parts = { 
                 "~", "Upload", "Assignments", userID.ToString()};
            var path = Path.Combine(parts);
            path = System.Web.HttpContext.Current.Server.MapPath(path); //FIXME - testability.
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return Path.Combine(
                path, assignmentID.ToString() + "-" + filename);
        }

    }
}