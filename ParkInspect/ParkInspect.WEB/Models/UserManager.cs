using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;

namespace ParkInspect.WEB.Models
{
    public class UserManager
    {
        public bool IsValid(string username, string password)
        {
            using (var db = new ParkInspectEntities()) // use your DbConext
            {
                var sha = SHA256.Create();

                var bytes = new byte[password.Length * sizeof(char)];
                System.Buffer.BlockCopy(password.ToCharArray(), 0, bytes, 0, bytes.Length);

                sha.ComputeHash(bytes);

                var chars = new char[sha.Hash.Length / sizeof(char)];
                System.Buffer.BlockCopy(sha.Hash, 0, chars, 0, sha.Hash.Length);

                var result = new string(chars);

                var client = db.Clients.ToList().Find(u => u.email == username && u.password == password);

                return client != null ||
                       db.Clients.ToList().Find(u => u.email == username && u.password == result) != null;
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