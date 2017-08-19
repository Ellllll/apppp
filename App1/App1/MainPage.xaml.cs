
using Android.Content;
using Android.Content.Res;
using Android.Widget;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
    }

    [Table("UserLocalInfo")]
    public class UserLocalInfo
    {
        [PrimaryKey, AutoIncrement]
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
        public ICommand BUttonClickCommand
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
        private void Login(string userName, string pwd)
        {

            string loginUrl = string.Format("http://192.168.1.102:/api/user/LogOn?userName={0}&pwd={1}", userName, pwd);
            var httpReq = (HttpWebRequest)WebRequest.Create(new Uri(loginUrl));
            var httpRes = (HttpWebResponse)httpReq.GetResponse();
            if (httpRes.StatusCode == HttpStatusCode.OK)
            {
                string result = new StreamReader(httpRes.GetResponseStream()).ReadToEnd();
                result = result.Replace("\"", "'");
                ReturnModel s = JsonConvert.DeserializeObject<ReturnModel>(result);
                if (s.Code == "00000")
                {
                    Toast.MakeText(Forms.Context, "登录成功", ToastLength.Short).Show();
                    var sqliteInfo = new Sqlite2();
                    sqliteInfo.CreateTable<UserLocalInfo>();
                    UserLocalInfo model = new UserLocalInfo() { Id = 1, UserName = userName, Pwd = pwd };
                    sqliteInfo.Insert(model);
                    var page = new Match();
                    Application.Current.MainPage = new Match();
                }
                else
                {
                    Toast.MakeText(Forms.Context, "用户名或密码不正确", ToastLength.Short).Show();
                    return;
                }
            }



        }

        //RegisterButton
        private ICommand register_;
        //register property
        public ICommand REgister
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

            string loginUrl = string.Format("http://192.168.1.102:/api/user/Regist?userName={0}&pwd={1}", userName, pwd);
            var httpReq = (HttpWebRequest)WebRequest.Create(new Uri(loginUrl));
            var httpRes = (HttpWebResponse)httpReq.GetResponse();
            if (httpRes.StatusCode == HttpStatusCode.OK)
            {
                string result = new StreamReader(httpRes.GetResponseStream()).ReadToEnd();
                result = result.Replace("\"", "'");
                ReturnModel s = JsonConvert.DeserializeObject<ReturnModel>(result);
                if (s.Code == "00000")
                {
                    Toast.MakeText(Forms.Context, "注册成功", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(Forms.Context, "此账号已经被注册", ToastLength.Short).Show();
                }
            }
 

        }

    }
    public class ReturnModel
    {
        public string Code { get; set; }
        public string Msg { get; set; }
    }
}
