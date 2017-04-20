﻿using cwssWpf.Data;
using cwssWpf.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser_Dialog : Window
    {
        // TODO:
        // Rename all objects in the window editor and set appropriate label text

        public User NewUser;
        public bool Success = false;

        public NewUser_Dialog(Window mainWindow)
        {
            InitializeComponent();
            this.Left = mainWindow.Left + 50;
            this.Top = mainWindow.Top + 50;
            cbGender.ItemsSource = (Enum.GetValues(typeof(GenderType)).Cast<GenderType>().ToList());
            FocusManager.SetFocusedElement(this, tbFirstName);
            MouseLeftButtonDown += Helpers.Window_MouseDown;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //reset field backgrounds
            tbFirstName.Background = Brushes.LightGoldenrodYellow;
            tbLastName.Background = Brushes.LightGoldenrodYellow;
            tbIdNumber.Background = Brushes.LightGoldenrodYellow;
            tbPassword.Background = Brushes.LightGoldenrodYellow;
            tbPassword2.Background = Brushes.LightGoldenrodYellow;
            tbEmail.Background = Brushes.LightGoldenrodYellow;
            tbAddress.Background = Brushes.LightGoldenrodYellow;
            tbCity.Background = Brushes.LightGoldenrodYellow;
            tbState.Background = Brushes.LightGoldenrodYellow;
            tbZip.Background = Brushes.LightGoldenrodYellow;
            tbPhone.Background = Brushes.LightGoldenrodYellow;
            cbGender.Background = Brushes.LightGoldenrodYellow;

            if (validateFields())
            {
                var success = Db.dataBase.AddUser(
                    tbFirstName, tbLastName,
                    tbIdNumber, tbPassword, tbPassword2,
                    tbEmail, tbAddress, tbCity, tbState, tbZip,
                    tbPhone, cbGender
                    );

                if (success)
                {
                    if (MainWindow.CurrentUser == null)
                        MainWindow.CurrentUser = new Data.User();

                    Logger.Log(MainWindow.CurrentUser.UserId, LogType.AddUser,
                        MainWindow.CurrentUser.GetName() + " Added User: " +
                        tbFirstName.Text + " " + tbLastName.Text);

                    Success = true;
                    NewUser = Db.dataBase.GetUser(int.Parse(tbIdNumber.Text));

                    bool signed = Helpers.ShowWaiver(NewUser);
                }
                else
                {
                    if (MainWindow.CurrentUser == null)
                        MainWindow.CurrentUser = new Data.User();

                    Logger.Log(MainWindow.CurrentUser.UserId, LogType.Error,
                        MainWindow.CurrentUser.GetName() + " Failed Add User: " +
                        tbFirstName.Text + " " + tbLastName.Text);

                    var alert = new Alert_Dialog("Add User Failed", "User add failed.");
                    MainWindow.WindowsOpen.Add(alert, new TimerVal(6));
                    alert.ShowDialog();
                }

                this.Close();
            }
            else
            {
                var alert = new Alert_Dialog("Form Error/s", "Please fix highlighted fields.");
                MainWindow.WindowsOpen.Add(alert, new TimerVal(6));
                alert.Show();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnGenerateID_Click(object sender, RoutedEventArgs e)
        {
            if(tbIdNumber.IsEnabled)
            {
                var id = Helpers.GenerateNewID();
                if(id == -1)
                {
                    var alert = new Alert_Dialog("ID Error", "ID number not available.  Please alert system administrator.");
                    alert.ShowDialog();
                }
                else
                {
                    tbIdNumber.IsEnabled = false;
                    tbIdNumber.Text = id.ToString();
                    btnGenerateID.Content = "Student";
                }
            }
            else
            {
                tbIdNumber.IsEnabled = true;
                tbIdNumber.Text = "";
                btnGenerateID.Content = "Non Student";
            }
        }

        private bool validateFields()
        {
            bool valid = true;

            if(string.IsNullOrEmpty(tbFirstName.Text))
            {
                valid = false;
                tbFirstName.Background = Brushes.LightPink;
            }

            if(string.IsNullOrEmpty(tbLastName.Text))
            {
                valid = false;
                tbLastName.Background = Brushes.LightPink;
            }

            if(tbPassword.Password != tbPassword2.Password || (tbPassword.Password.Length < StaticValues.MinPasswordLength || tbPassword2.Password.Length < StaticValues.MinPasswordLength))
            {
                valid = false;
                tbPassword2.Background = Brushes.LightPink;
                tbPassword.Background = Brushes.LightPink;
            }

            if(!Helpers.ValidateIdInput(tbIdNumber.Text.ToString()))
            {
                valid = false;
                tbIdNumber.Background = Brushes.LightPink;
            }

            int zip = 0;
            if(!int.TryParse(tbZip.Text, out zip) || tbZip.Text.Length != 5)
            {
                valid = false;
                tbZip.Background = Brushes.LightPink;
            }

            var phoneReg = new Regex(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$");
            if(!phoneReg.IsMatch(tbPhone.Text))
            {
                valid = false;
                tbPhone.Background = Brushes.LightPink;
            }

            if(cbGender.SelectedIndex == -1)
            {
                valid = false;
                cbGender.Background = Brushes.LightPink;
            }

            if(string.IsNullOrEmpty(tbEmail.Text))
            {
                valid = false;
                tbEmail.Background = Brushes.LightPink;
            }

            if (string.IsNullOrEmpty(tbAddress.Text))
            {
                valid = false;
                tbAddress.Background = Brushes.LightPink;
            }

            if (string.IsNullOrEmpty(tbState.Text))
            {
                valid = false;
                tbState.Background = Brushes.LightPink;
            }

            if (string.IsNullOrEmpty(tbCity.Text))
            {
                valid = false;
                tbCity.Background = Brushes.LightPink;
            }

            return valid;
        }
    }
}
