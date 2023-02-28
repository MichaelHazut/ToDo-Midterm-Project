using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp_ProjectV2.Models;

namespace ToDoApp_ProjectV2.DataAccessLayer
{
    internal class ToDoAppContext : DbContext
    {
        public ToDoAppContext() : base("Data Source=MICHAELZENBOOK;Initial Catalog=ToDoAppDBTest1;Integrated Security=True")
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ToDoTask> ToDoTasks { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
        }
    }
}
