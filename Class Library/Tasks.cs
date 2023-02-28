using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ToDoApp_ProjectV2;


namespace ToDoApp_ProjectV2.Class_Library
{
    public class Tasks
    {
        internal ImageButton IdButton;
        internal ImageButton SaveButton;
        internal DatePicker UpdateDate;
        internal VerticalStackLayout TaskLayout;

        internal List<ImageButton> EditButtons;
        internal List<ImageButton> EditImageButtons;
        internal List<Entry> UpdateEntrys;
        internal List<Label> UpdateLabels;
        internal Dictionary<ImageButton, VisualElement[]> VisualElementDict;


        public Tasks() { }
        internal Tasks(Label labelTitle, Entry editTitle, ImageButton editTitleButton, Label labelDescription, Entry editDesc,
            ImageButton editDescButton, ImageButton deleteTaskButton, Label labelTime, Label labelDate,
            Entry eStartTime, Entry eEndTime, DatePicker eDate, ImageButton eTimeButton,
            ImageButton mainEditButton, ImageButton saveEditButton, VerticalStackLayout taskLayout)
        {
            IdButton = mainEditButton;
            SaveButton = saveEditButton;
            UpdateDate = eDate;
            TaskLayout = taskLayout;

            EditButtons = new List<ImageButton>() { editTitleButton, editDescButton, eTimeButton, deleteTaskButton, saveEditButton, mainEditButton };
            EditImageButtons = new List<ImageButton>() { editTitleButton, editDescButton, eTimeButton, deleteTaskButton };
            UpdateEntrys = new List<Entry>() { editTitle, editDesc, eStartTime, eEndTime };
            UpdateLabels = new List<Label>() { labelTitle, labelDescription, labelTime, labelDate };



            VisualElementDict = new Dictionary<ImageButton, VisualElement[]>()
            {
                [editTitleButton] = new VisualElement[] { labelTitle, editTitle },
                [editDescButton] = new VisualElement[] { labelDescription, editDesc },
                [eTimeButton] = new VisualElement[] { labelTime, labelDate, eStartTime, eEndTime, eDate }
            };

        }

        internal void EditTask()
        {
            EditButtons.ForEach(e => e.IsVisible = !e.IsVisible);
        }
        internal void EditElement(ImageButton sender)
        {
            VisualElementDict[sender].ToList().ForEach(e => e.IsVisible = !e.IsVisible);
            string[] iconArr = new string[] { "editicon.png", "closeicon.png" };
            string hey = sender.Source.ToString().Remove(0, 6);
            if (sender.Source.ToString().Remove(0, 6) == iconArr[0])
            {
                sender.Source = iconArr[1];
                return;
            }
            sender.Source = iconArr[0];
        }
        internal void ResetTaskView()
        {
            EditButtons.ForEach(e => e.IsVisible = !e.IsVisible);
            EditImageButtons.ForEach(x => x.Source = "editicon.png");
            VisualElementDict.ToList().ForEach(x =>
            {
                x.Value.ToList().ForEach(y =>
                {
                    if (y is Label)
                    {
                        y.IsVisible = true;
                        return;
                    }
                    y.IsVisible = false;
                });
            });
        }
        internal void SaveChanges()
        {
            List<string> list = new List<string>();
            UpdateEntrys.ForEach(x => list.Add(x.Text));
            list.Add(UpdateDate.Date.ToString("dd/MM/yyyy"));

            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrEmpty(list[i])) { continue; }
                switch (i)
                {
                    case 0: case 1:
                        UpdateLabels[i].Text = list[i];
                        UpdateEntrys[i].Placeholder = list[i];
                        UpdateEntrys[i].Text = "";
                        break;
                    case 2:
                        try
                        {
                            TimeOnly testTime = new TimeOnly(int.Parse(list[i].Substring(0,2)),int.Parse(list[i].Substring(3, 2)));
                            UpdateLabels[i].Text = $"{list[i]} - {UpdateLabels[i].Text.Substring(7)}";
                            UpdateEntrys[i].Placeholder = list[i];
                            UpdateEntrys[i].Text = "";
                            UpdateEntrys[i].BackgroundColor = new Color(0,0,0,0);
                        }
                        catch(Exception e)
                        {
                            UpdateEntrys[i].BackgroundColor = new Color(255, 0, 0);
                        }
                        break;
                    case 3:
                        UpdateLabels[2].Text = $"{UpdateLabels[2].Text.Substring(0,5)} - {list[i]}";
                        UpdateEntrys[i].Placeholder = list[i];
                        UpdateEntrys[i].Text = "";
                        break;
                    case 4:
                        UpdateLabels[3].Text = list[i];
                        break;

                }
            }




            ResetTaskView();
        }


    }
}
