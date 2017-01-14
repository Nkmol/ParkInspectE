using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkInspect.WEB.Models
{
    public class UserManager
    {
        public bool IsValid(string username, string password)
        {
            using (var db = new ParkInspectEntities()) // use your DbConext
            {
                // if your users set name is Users
                return db.Clients.ToList().Find(u => u.email == username
                    && u.password == password) != null;
            }
        }
    }
}