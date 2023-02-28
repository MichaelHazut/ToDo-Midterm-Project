using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ToDoApp_ProjectV2.Class_Library;
using ToDoApp_ProjectV2.DataAccessLayer;


namespace ToDoApp_ProjectV2.Models
{
    public class ToDoTask : ISqlable
    {
        [Key]
        public int TaskId { get; set; }
        public int UserId { get; set; }
        //[ForeignKey("UserId")]
        //public virtual User User { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Date { get; set; }
        public DateOnly DateOnly { get; set; }

        internal ToDoTaskElements Elements;



        public ToDoTask() { }
        internal ToDoTask(string title, string desciption, TimeSpan sTime, TimeSpan eTime, DateTime date)
        {
            this.Title = title;
            this.Description = desciption;
            this.StartTime = Convert.ToDateTime(sTime.ToString());
            this.EndTime = Convert.ToDateTime(eTime.ToString());
            this.Date = date;
        }
        internal static List<ToDoTask> SqlReadManyTasks(User user)
        {
            using (ToDoAppContext context = new ToDoAppContext())
            {
                return context.ToDoTasks.Where(x => x.UserId == user.Id).ToList();
            }
        }
        public void SqlCreate()
        {
            using (ToDoAppContext context = new ToDoAppContext())
            {
                context.ToDoTasks.Add(this);
                context.SaveChanges();
            }
        }
        public void SqlDelete()
        {
            using (ToDoAppContext context = new ToDoAppContext())
            {
                context.ToDoTasks.Remove(context.ToDoTasks.First(x => x.TaskId == this.TaskId));
                context.SaveChanges();
            }
        }
        public void SqlUpdate()
        {
            this.SqlDelete();
            this.SqlCreate();
        }
        public override string ToString()
        {
            string startTime = DateTime.Parse(this.StartTime.ToString()).ToString("HH:mm");
            string endTime = DateTime.Parse(this.EndTime.ToString()).ToString("HH:mm");
            string date = this.Date.ToString("dd/MM/yyyy");

            return $"Date: {date}| Time: {startTime} - {endTime}| UserId: {this.UserId} | Title: {this.Title} | Desc: {this.Description} | ";
        }
    }
}
