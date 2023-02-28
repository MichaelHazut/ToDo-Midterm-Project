using ToDoApp_ProjectV2.Models;
//using CommunityToolkit.Maui.Behaviors;
using Microsoft.Win32;
using Microsoft.Maui.Graphics;

namespace ToDoApp_ProjectV2;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }
    public LoginPage(string email)
    {
        InitializeComponent();
        EmailEntry.Text = email;
    }
    
    void NewUser_Click(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RegisterPage());
    }
    void OnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            User user = User.SqlRead(User.FormatEmail(EmailEntry.Text));

            if (user.CheckPassword(PasswordEntry.Text))
            {
                Navigation.PushAsync(new MainPage(user));
                InvalidInput.IsVisible = false;
                return;
            }

        }
        catch (Exception ) { }
        InvalidInput.IsVisible = true;
    }
}