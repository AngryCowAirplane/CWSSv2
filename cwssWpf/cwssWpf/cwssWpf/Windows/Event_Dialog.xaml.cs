﻿using cwssWpf.Data;
using cwssWpf.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace cwssWpf.Windows
{
    /// <summary>
    /// Interaction logic for Event_Dialog.xaml
    /// </summary>
    public partial class Event_Dialog : Window
    {
        public Event_Dialog(DateTime date)
        {
            InitializeComponent();
            cbUsers.ItemsSource = Db.dataBase.Users;
            MouseLeftButtonDown += Helpers.Window_MouseDown;
            PreviewKeyDown += Helpers.HandleEsc;
            if (date != null)
            {
                StartDate.SelectedDate = date;
                EndDate.SelectedDate = date;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            lbUsers.Items.Add(cbUsers.SelectedItem);
        }

        private void RemoveUser_Click(object sender, RoutedEventArgs e)
        {
            lbUsers.Items.Remove(lbUsers.SelectedItem);
        }

        private void addEvent_Click(object sender, RoutedEventArgs e)
        {
            var newEvent = new Event();
            newEvent.EventName = Title.Text;
            newEvent.EventStart = StartDate.SelectedDate.Value;
            newEvent.EventEnd = EndDate.SelectedDate.Value;

            foreach (var item in lbUsers.Items)
            {
                newEvent.EventMembers.Add((User)item);
            }

            newEvent.EventCreator = MainWindow.CurrentUser;
            newEvent.EventComment = Description.Text;

            if (!Db.dataBase.Events.Contains(newEvent))
                Db.dataBase.Events.Add(newEvent);

            if((bool)cbNotify.IsChecked)
            {
                var message = new Message();
                message.SetSender(MainWindow.CurrentUser);
                message.SetRecipients(newEvent.EventMembers);
                message.Subject = "You have been added to an event: " + newEvent.EventName + " on " + newEvent.EventStart.ToShortDateString();
                message.Contents = newEvent.EventStart.ToShortTimeString() + " : " + newEvent.EventComment;
                message.ExpireDate = DateTime.Now + TimeSpan.FromDays(45);
                message.TimeStamp = DateTime.Now;
                Db.dataBase.AddMessage(message);
                this.Close();
            }

            this.Close();
            var alert = new Alert_Dialog("Event Created", "Your event has been created.", AlertType.Success);
            Logger.Log(MainWindow.CurrentUser.LoginId, LogType.Calendar, "Added event to the calendar: " + newEvent.EventStart.ToShortDateString() + newEvent.EventName);
            MainWindow.WindowsOpen.Add(alert, new cwssWpf.TimerVal(2));
            alert.Show();
        }
    }
}
