﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using cwssWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cwssWpf.Data;
using System.IO;

namespace cwssWpf.DataBaseTests
{
    [TestClass()]
    public class BasicCreateSaveLoadDb
    {
        [TestMethod]
        public void _BasicCreateSaveLoadDb()
        {
            _DataBase.Data = new _BaseDataObject();

            var user = new User();
            user.UserId = 1023542;
            user.UserName = "Derek";
            user.UserType = UserType.Admin;
            _DataBase.Data.AddUser(user);
            Assert.IsTrue(_DataBase.Data.Users.Count > 0);

            _DataBase.Save();
            Assert.IsTrue(File.Exists(_DataBase.SavePath + _DataBase.DbFileName));

            _DataBase.Data = new Data._BaseDataObject();
            Assert.IsTrue(_DataBase.Data.Users.Count == 0);

            _DataBase.Load();
            Assert.IsTrue(_DataBase.Data.Users.Count > 0);
        }
    }
}