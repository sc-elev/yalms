 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using yalms.Models;
using System.Web.Mvc;
using yalms.DAL;
using System;

namespace yalms.Services
{

    public class BaseRepository: IDisposable
    {
        protected EFContext context;
        protected bool disposed = false;


        public BaseRepository()
        {
            context = new EFContext();
        }


        public BaseRepository(EFContext context)
        {
            this.context = context;
        }


        public void Save()
        {
            context.SaveChanges();
        }

       
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);
        }
    }
}