
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new TryEventBindingPageViewModel();

        }



    }

    [Table("UserInfo")]
    public class UserInfo
    {
        [PrimaryKey, AutoIncrement, Collation("Id")]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
    }

    public class TryEventBindingPageViewModel
    {
        public TryEventBindingPageViewModel()
        {
            _buttonClickCommand = new Command(ButtonClickCommandExecute, CanButtonClickCommandExecute);
            register_ = new Command(RegisterExecute, CanRegisterExecute);
        }

        //SQLite
        static string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userinfo.db3");
        private const string TableName = "UserInfo";

        //username pwd
        private string userNameText;
        public string UserNameText { get {return userNameText; } set {userNameText=value; } }
        private string pwdText;
        public string PwdText { get { return pwdText; } set { pwdText = value; } }

        //LoginButton
        public ICommand _buttonClickCommand;
        //login property
        public ICommand buttonClickCommand
        {
            get { return _buttonClickCommand; }
            set { _buttonClickCommand = value; }
        }
        public void ButtonClickCommandExecute(object args)//click
        {
            var UserName = userNameText;
            var Pwd = pwdText;
            Login(UserName,Pwd);
        }
        private bool CanButtonClickCommandExecute(object args)
        {
            return true;
        }
        private void Login(string userName,string pwd)
        {
            var sqliteConn=new Sqlite1();
            var userInfoTable = sqliteConn.GetTableInfo(TableName);
            if (userInfoTable.Count == 0)
            {
                sqliteConn.CreateTable<UserInfo>();
            }
            var userInfos = sqliteConn.Table<UserInfo>();
            var userInfo = userInfos.Where(p => p.Pwd == pwd && p.UserName == userName).FirstOrDefault();
            if (userInfo == null)
            {
                Toast.MakeText(Forms.Context, "用户名或密码不正确", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(Forms.Context, "登录成功", ToastLength.Short).Show();
            }


        }

        //RegisterButton
        private ICommand register_;
        //register property
        public ICommand register
        {
            get { return register_; }
            set { register_ = value; }
        }
        private void RegisterExecute(object args)
        {
            var UserName = userNameText;
            var Pwd = pwdText;
            Register(UserName, Pwd);
        }
        private bool CanRegisterExecute(object args)
        {
            return true;
        }
        private void Register(string userName, string pwd)
        {

        }

    }
}
