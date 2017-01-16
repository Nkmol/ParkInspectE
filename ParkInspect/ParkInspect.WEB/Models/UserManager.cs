using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
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

        public List<ParkInspect.Client> GetClients()
        {
            using (var db = new ParkInspectEntities())
            {
                return db.Clients.ToList();
            }
        }

        public int ChangePassword(Client client)
        {
            using (var db = new ParkInspectEntities())
            {                
                db.Entry(client.ToPoco()).State = EntityState.Modified;
                return db.SaveChanges();
            }
        }
    }

    public class Client
    {
        public Client(ParkInspect.Client client)
        {
            Id = client.id;
            Name = client.name;
            Phonenumber = client.phonenumber;
            Email = client.email;
            Password = client.password;
        }
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string Phonenumber { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string Email { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string Password { get; set; }

        internal ParkInspect.Client ToPoco()
        {
            return new ParkInspect.Client()
            {
                id = Id,
                name = Name,
                email = Email,
                password = Password,
                phonenumber = Phonenumber,
            };
        }
    }
}