using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Globalization;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;
using ToDoApp_ProjectV2.Class_Library;
using ToDoApp_ProjectV2.DataAccessLayer;
using ToDoApp_ProjectV2.Models;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;

namespace ToDoApp_ProjectV2;
public partial class MainPage : ContentPage
{
    internal string PrevText = "";
    internal List<ToDoTask> ToDoTaskList = new List<ToDoTask>();
    internal List<VerticalStackLayout> taskLayoutList = new List<VerticalStackLayout>();
    public User CurrentUser;
    
    public MainPage()
    {
        InitializeComponent();
        SetTodaySchedule();
        SetTodayDate();
    }
    public MainPage(User user)
    {
        CurrentUser = user;
        InitializeComponent();

        SetTodayDate();
        SetTodaySchedule();
        InitializeUserOptions();
        InitializeToDoTasks(CurrentUser);
    }
    void InitializeToDoTasks(User user)
    {
        ToDoTaskList = ToDoTask.SqlReadManyTasks(user);
        ToDoTaskList.ForEach(x =>
        {
            VerticalStackLayout fullStack = ToDoTaskVisualizer.CreateVisualizedTask(x, this);
            TasksStackLayout.Children.Add(fullStack);
            taskLayoutList.Add(fullStack);
        });
    }
    void InitializeUserOptions()
    {
        if (string.IsNullOrEmpty(CurrentUser.FirstName))
        {
            return;
        }

        UserName_Label.Text = $"{CurrentUser.FirstName} {CurrentUser.LastName}";
        UserEmail_Label.Text = CurrentUser.Email;
    }
    async void AutoScroll()
    {
        await UserScrollView.ScrollToAsync(0, 150, true);
    }
    private void ChangeFlyoutPage(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LoginPage());
    }
    void SetTime(object sender, EventArgs e)
    {
        var hey = new Label();
        hey.Text = "7 AM";
        var panel = todaySchedule;
        panel.Children.Add(hey);

    }
    void SetTodaySchedule()
    {
        int hour = 0;
        VerticalStackLayout schedule = todaySchedule;

        for (var i = 0; i < 24; i++)
        {

            HorizontalStackLayout aTime = new HorizontalStackLayout();
            aTime.Children.Add(new Label
            {
                Text = hour++ + " AM",
                Margin = new Thickness(5, 15),
                VerticalOptions = LayoutOptions.Center
            });
            aTime.Children.Add(new Line()
            {
                Stroke = new Color(0, 0, 0),
                StrokeThickness = 2,
                X2 = 900,
                VerticalOptions = LayoutOptions.Center,
                Opacity = 0.3
            });
            schedule.Children.Add(aTime);
        }
    }
    void SetTodayDate()
    {
        var DateNow = DateTime.UtcNow.Date;
        todayDate.Text = DateNow.ToString("dd/MM/yyyy");

    }
    void OpenOrCloseNewTaskWindow(object sender, EventArgs e)//need to add reset to the timers
    {
        ClearTaskForm(sender, e);
        string[] iconArr = new string[] { "plusicon.png", "closeicon.png" };
        newToDoForm.IsVisible = !newToDoForm.IsVisible;
        addButton.Source = iconArr[Convert.ToInt32(newToDoForm.IsVisible)];
        TaskTitle.Text = "";
        TaskDescription.Text = "";
    }
    void ClearTaskForm(object sender, EventArgs e)
    {
        TaskDate.Date = DateTime.UtcNow.Date;
        TaskStartTime.Time = new TimeSpan();
        TaskEndTime.Time = new TimeSpan();
    }
    void CreateNewTaskClick(object sender, EventArgs e)
    {
        if (TaskTitle.Text.ToString() == "")
        {
            TaskTitle.Placeholder = "   Title Is Empty";
            TaskTitle.PlaceholderColor = new Color(255, 0, 0);
            return;
        }

        ToDoTask toDoTask = new ToDoTask(
            TaskTitle.Text,
            TaskDescription.Text,
            TaskStartTime.Time,
            TaskEndTime.Time,
            TaskDate.Date
            ) { UserId = CurrentUser.Id };

        VerticalStackLayout fullStack = ToDoTaskVisualizer.CreateVisualizedTask(toDoTask, this);
        TasksStackLayout.Children.Add(fullStack);
        ToDoTaskList.Add(toDoTask);
        taskLayoutList.Add(fullStack);

        toDoTask.SqlCreate();
        OpenOrCloseNewTaskWindow(sender, e);

        ClearTaskForm(sender, e);
    }
    void OpenUserOptions(object sender, EventArgs e)
    {
        userOptions.IsVisible = !userOptions.IsVisible;
    }
    void LogOut(object sender, EventArgs e)
    {
        Navigation.PopAsync() ;
    }
    internal void EditTaskButtonClick(object sender, EventArgs e)
    {
        ImageButton button = sender as ImageButton;
        ToDoTask task = ToDoTaskList.First(x => x.Elements.EditButtons.Contains(button));
        task.Elements.EditTask();
    }
    internal void EditElementButtonClick(object sender, EventArgs e)
    {
        ImageButton button = sender as ImageButton;
        ToDoTask task = ToDoTaskList.First(x => x.Elements.EditImageButtons.Contains(button));

        task.Elements.EditElement(button);
    }
    internal void SaveTaskChanges(object sender, EventArgs e)
    {
        ImageButton button = sender as ImageButton;
        ToDoTask task = ToDoTaskList.First(x => x.Elements.EditButtons.Contains(button));
        task.Elements.SaveChanges(task);
    }
    internal void EditTime_TextChanged(object sender, EventArgs e)
    {
        Entry time = sender as Entry;
        if (time.Text.Length < PrevText.Length)
        {
            PrevText = time.Text;
            return;
        }
        if (time.Text.Length == 2)
        {
            time.Text += ":";
            PrevText = time.Text;
            return;
        }
        if (time.Text.Length >= 3 && time.Text[2] != ':')
        {
            string newTime = time.Text.Insert(2, ":");
            time.Text = newTime;
        }
        if (time.Text.Length == 6)
        {
            string newTime = time.Text.Remove(4, 1);
            time.Text = newTime;
        }
        PrevText = time.Text;
    }
    internal async void DeleteTaskButtonClick(object sender, EventArgs e)
    {
        if (await DisplayAlert("Delete", "You sure you'd like to delete task?", "Yes", "No"))
        {
            ImageButton button = sender as ImageButton;
            ToDoTask task = ToDoTaskList.First(x => x.Elements.EditButtons.Contains(button));
            TasksStackLayout.Remove(task.Elements.TaskLayout);
            task.SqlDelete();
        }
    }


    //for Sql Testing
    void SqlTester(object sender, EventArgs e)
    {
        ToDoAppContext context = new ToDoAppContext();
        //FillSqlUsers();
        //FllSqlToDoTasks();
        //context.SaveChanges();
    }
    //void FillSqlUsers()
    //{

    //    Context.Users.AddRange(new List<User>()
    //    {
    //        new User(){FirstName = "Michael", LastName = "Hazut", Id = 1},
    //        new User(){FirstName = "Andry", LastName = "Vash", Id = 2},
    //        new User(){FirstName = "Dan", LastName = "Belanogov", Id = 3},
    //        new User(){FirstName = "Sharon", LastName = "Nazarenko", Id = 19},
    //        new User(){FirstName = "Yaniv", LastName = "Cohen", Id = 18},
    //        new User(){FirstName = "Maor", LastName = "Mizmori", Id = 17},
    //        new User(){FirstName = "Guy", LastName = "Drimel", Id = 16},
    //        new User(){FirstName = "Alex", LastName = "Dvir", Id = 15},
    //        new User(){FirstName = "Dima", LastName = "Israeli", Id = 14},
    //        new User(){FirstName = "Dillen", LastName = "Mizrahi", Id = 13},
    //        new User(){FirstName = "Daniel", LastName = "Levi", Id = 11},
    //        new User(){FirstName = "David", LastName = "Teaski", Id = 12}
    //    });
    //    Context.SaveChanges();
    //}
    //void FllSqlToDoTasks()
    //{
    //    ToDoAppContext context = new ToDoAppContext();
    //    List<User> users = context.Users.Select(x => x).ToList();

    //    List<ToDoTask> toDoTasks = new List<ToDoTask>()
    //    {
    //        new ToDoTask("Test Task 1","Descripton test 1",new TimeSpan(12,30,00),new TimeSpan(13,30,00), new DateTime(2022,09,07)){UserId = 11},
    //        new ToDoTask("Test Task 2","Descripton test 2",new TimeSpan(14,60,00),new TimeSpan(15,45,00), new DateTime(2022,03,14)){UserId = 12},
    //        new ToDoTask("Test Task 3","Descripton test 3",new TimeSpan(09,15,00),new TimeSpan(05,36,00), new DateTime(2022,09,15)){UserId = 13},
    //        new ToDoTask("Test Task 4","Descripton test 4",new TimeSpan(08,26,00),new TimeSpan(11,18,00), new DateTime(2022,08,31)){UserId = 14},
    //        new ToDoTask("Test Task 5","Descripton test 5",new TimeSpan(10,30,00),new TimeSpan(04,25,00), new DateTime(2022,02,14)){UserId = 15},
    //        new ToDoTask("Test Task 6","Descripton test 6",new TimeSpan(11,36,00),new TimeSpan(13,39,00), new DateTime(2022,06,27)){UserId = 16},
    //        new ToDoTask("Test Task 7","Descripton test 7",new TimeSpan(12,42,00),new TimeSpan(22,17,00), new DateTime(2022,04,20)){UserId = 17},
    //        new ToDoTask("Test Task 8","Descripton test 8",new TimeSpan(07,25,00),new TimeSpan(21,85,00), new DateTime(2022,01,17)){UserId = 18},
    //    };
    //    Context.ToDoTasks.AddRange(toDoTasks);
    //    Context.SaveChanges();
    //}

}


