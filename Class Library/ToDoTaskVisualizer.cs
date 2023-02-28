using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp_ProjectV2.Models;

namespace ToDoApp_ProjectV2.Class_Library
{
    internal static class ToDoTaskVisualizer
    {
        internal static VerticalStackLayout CreateVisualizedTask(ToDoTask toDoTask, MainPage obj)
        {
            
            Grid grid = new Grid()
            {
                ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }
            },
                RowDefinitions = { new RowDefinition { Height = new GridLength(150) }
            },
            };

            
            HorizontalStackLayout titleStack = new HorizontalStackLayout();
            
            Label labelTitle = new Label()
            {
                Text = toDoTask.Title,
                Margin = 15,
                FontSize = 34
            };
            Entry editTitleEntry = new Entry()
            {
                IsVisible = false,
                Placeholder = labelTitle.Text,
                FontSize = 34,
                Margin = new Thickness(10, 10, 5, 10)
            };

            ImageButton editTitleButton = new ImageButton()
            {
                IsVisible = false,
                Source = "editicon.png",
                WidthRequest = 40
            };
            editTitleButton.Clicked += obj.EditElementButtonClick;

            titleStack.Children.Add(labelTitle);
            titleStack.Children.Add(editTitleEntry);
            titleStack.Children.Add(editTitleButton);


            HorizontalStackLayout descriptionStack = new HorizontalStackLayout();
            Label labelDescription = new Label()
            {
                Text = toDoTask.Description,
                Margin = new Thickness(15, 15, 5, 15),
            };
            Entry editDescriptionEntry = new Entry()
            {
                IsVisible = false,
                Placeholder = labelDescription.Text,
                Margin = new Thickness(15, 15, 5, 15)
            };
            ImageButton eDescButton = new ImageButton()
            {
                IsVisible = false,
                Source = "editicon.png",
                WidthRequest = 40
            };
            eDescButton.Clicked += obj.EditElementButtonClick;

            descriptionStack.Children.Add(labelDescription);
            descriptionStack.Children.Add(editDescriptionEntry);
            descriptionStack.Children.Add(eDescButton);

            VerticalStackLayout detailsStack = new VerticalStackLayout();
            detailsStack.Children.Add(titleStack);
            detailsStack.Children.Add(descriptionStack);

            ImageButton deleteTaskButton = new ImageButton()
            {
                IsVisible = false,
                Source = "trash.png",
                WidthRequest = 60,
                Padding = 10
            };
            deleteTaskButton.Clicked += obj.DeleteTaskButtonClick;
            HorizontalStackLayout leftStack = new HorizontalStackLayout();
            leftStack.Children.Add(deleteTaskButton);
            leftStack.Children.Add(detailsStack);


            VerticalStackLayout dateTimeStack = new VerticalStackLayout()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            
            string startTime = DateTime.Parse(toDoTask.StartTime.ToString()).ToString("HH:mm");
            string endTime = DateTime.Parse(toDoTask.EndTime.ToString()).ToString("HH:mm");
            Label timeLabel = new Label()
            {
                Text = $"{startTime} - {endTime}",
                HorizontalOptions = LayoutOptions.Center,
            };
            
            Label dateLabel = new Label()
            {
                Text = toDoTask.Date.ToString("dd/MM/yyyy"), 
                HorizontalOptions = LayoutOptions.Center,
            };

            Entry editStartTimeEntry = new Entry()
            {
                IsVisible = false,
                Placeholder = startTime
            };
            editStartTimeEntry.TextChanged += obj.EditTime_TextChanged;

            Entry editEndTimeEntry = new Entry()
            {
                IsVisible = false,
                Placeholder = endTime
            };
            editEndTimeEntry.TextChanged += obj.EditTime_TextChanged;

            DatePicker editDatePicker = new DatePicker()
            {
                IsVisible = false,
                MinimumDate = new DateTime(2022, 1, 1),
                Date = toDoTask.Date,
                Format = "dd/MM/yyyy"
            };
            
            dateTimeStack.Children.Add(timeLabel);
            dateTimeStack.Children.Add(dateLabel);
            dateTimeStack.Children.Add(editStartTimeEntry);
            dateTimeStack.Children.Add(editEndTimeEntry);
            dateTimeStack.Children.Add(editDatePicker);

            
            HorizontalStackLayout rightStack = new HorizontalStackLayout()
            {
                VerticalOptions = LayoutOptions.Center,
                Spacing = 20,
                HeightRequest = 100
            };
            
            ImageButton editTimeButton = new ImageButton()
            {
                IsVisible = false,
                Source = "editicon.png",
                WidthRequest = 40
            };
            editTimeButton.Clicked += obj.EditElementButtonClick;
            
            ImageButton mainEditButton = new ImageButton()
            {
                Source = "editicon.png",
                WidthRequest = 60,
                Padding = 5
            };
            mainEditButton.Clicked += obj.EditTaskButtonClick;


            ImageButton saveEditButton = new ImageButton()
            {
                IsVisible = false,
                Source = "save.png",
                WidthRequest = 60,
                Padding = 5,
            };
            saveEditButton.Clicked += obj.SaveTaskChanges;

            rightStack.Children.Add(editTimeButton);
            rightStack.Children.Add(dateTimeStack);
            rightStack.Children.Add(mainEditButton);
            rightStack.Children.Add(saveEditButton);

            grid.Add(leftStack, 0, 0);
            grid.Add(rightStack, 1, 0);


            VerticalStackLayout fullStack = new VerticalStackLayout();
            fullStack.Children.Add(grid);

            toDoTask.Elements = new ToDoTaskElements(labelTitle, editTitleEntry, editTitleButton,labelDescription,
                editDescriptionEntry, eDescButton,deleteTaskButton, timeLabel,dateLabel,editStartTimeEntry,
                editEndTimeEntry,editDatePicker, editTimeButton,mainEditButton, saveEditButton, fullStack
            );
            return fullStack;
        }
    }
}
