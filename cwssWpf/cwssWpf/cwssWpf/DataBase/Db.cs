﻿using cwssWpf.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cwssWpf.DataBase
{
    public static class Db
    {
        //public static Context dataBase;
        public static _BaseDataObject dataBase = _DataBase.Data;

        public static User GetUser(int loginId)
        {
            try
            {
                return dataBase.Users.Where(user => user.LoginId == loginId).First();
            }
            catch
            {
                return null;
            }
        }

        public static bool AddUser(int loginId, UserType userType, string password, bool canClimb = true, string userName = "", string email = "", string phone = "")
        {
            try
            {
                var findUser = Db.GetUser(loginId);
                if (findUser == null)
                {
                    var user = new User();
                    user.CanClimb = canClimb;
                    user.Email = email;
                    user.Password = password;
                    user.PhoneNumber = phone;
                    user.UserName = userName;
                    user.LoginId = loginId;
                    user.UserType = userType;

                    dataBase.Users.Add(user);

                    dataBase.SaveChanges();
                }
                else
                {
                    MessageBox.Show("User Already Exists.");
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool AddUser(string loginId, UserType userType, string password, bool canClimb = true, string userName = "", string email = "", string phone = "")
        {
            try
            {
                var Id = int.Parse(loginId);
                AddUser(Id, userType, password, canClimb, userName, email, phone);
            }
            catch
            {
                return false;
            }
            return true;
        }



        public static void Initialize()
        {
            var dbPath = System.IO.Path.Combine(Environment.CurrentDirectory, "AppData", @"CwssDataBase.cwdb");
            _DataBase.Load(dbPath);
            dataBase = _DataBase.Data;
        }


        //----------------------------------------------------------------------------
        // 
        public static void ResetUsers()
        {
            var list = new List<User>();
            foreach (var user in dataBase.Users)
            {
                list.Add(user);
            }
            foreach (var user in list)
            {
                dataBase.Users.Remove(user);
            }
        }
    }
}
