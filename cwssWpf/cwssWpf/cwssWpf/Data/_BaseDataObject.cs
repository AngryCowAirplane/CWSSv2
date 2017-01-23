﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwssWpf.Data
{
    // Essentially the main tables in the database
    // the individual classes(user, item, message, etc..) will be the lines (data members, columns..)

    public class _BaseDataObject
    {
        public List<User> Users = new List<User>();
        public List<Item> Items = new List<Item>();
        public List<Message> Messages = new List<Message>();
        public List<DailyLogTag> DailyLogs = new List<DailyLogTag>();

        public _BaseDataObject()
        {

        }

        public void SaveChanges()
        {

        }

        public bool AddUser(User user)
        {
            // TODO:
            // If no user.LoginId, assign one not in Users
            // Else Check if user.LoginId already exists in Users


            Users.Add(user);
            return true;
        }

        public bool AddItem()
        {
            return true;
        }

        public bool AddMessage()
        {
            return true;
        }

        public bool AddLog()
        {
            return true;
        }
    }
}
