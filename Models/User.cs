using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Mapping;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp_ProjectV2.DataAccessLayer;
using ToDoApp_ProjectV2.Models;
//using Xamarin.Google.Crypto.Tink.Subtle;

namespace ToDoApp_ProjectV2.Models
{
    public class User : ISqlable
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<ToDoTask> ToDoTasks { get; set; }
        [Column]
        public string FirstName { get; set; }
        [Column]
        public string LastName { get; set; }
        public bool IsStayLoggedIn { get; set; }

        public User() { }
        public User(string email, string password, string fName, string lName)
        {
            this.Email = email;
            this.Password = password;
            this.FirstName = char.ToUpper(fName[0]) + fName.Substring(1);
            this.LastName = char.ToUpper(lName[0]) + lName.Substring(1);
        }
        public static bool UserCheckIfExist(string email)
        {
            try
            {
                using (ToDoAppContext context = new ToDoAppContext())
                {
                    return context.Users.Any(x => x.Email == email);
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public void SqlCreate()
        {
            try
            {
                using (ToDoAppContext context = new ToDoAppContext())
                {
                    context.Users.Add(this);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        public void SqlDelete()
        {
            using (ToDoAppContext context = new ToDoAppContext())
            {
                context.Users.Remove(context.Users.First(x => x.Id == this.Id));
                context.SaveChanges();
            }
        }
        public void SqlUpdate()
        {
            this.SqlDelete();
            this.SqlCreate();
        }
        public static User SqlRead(string email)
        {
            if (!UserCheckIfExist(email))
            {
                return new User();
            }
            try
            {
                using (ToDoAppContext context = new ToDoAppContext())
                {
                    return context.Users.First(x => x.Email == email);
                }
            }
            catch (Exception)
            {
                return new User();
            }
        }
        public static string FormatEmail(string email)
        {
            return email.Replace(" ", String.Empty);
        }
        public bool CheckPassword(string password)
        {
            if(this.Password == password)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Id:{Id}, Name: {FirstName} {LastName}, Email: {Email}";
        }
    }
}
