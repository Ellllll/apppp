
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
using System.Net.Http;
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
        //static string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userinfo.db3");
        //private const string TableName = "UserInfo";

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
        static async void Login(string userName, string pwd)
        {

            // Create a New HttpClient object.
            HttpClient client = new HttpClient();

            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://39.108.122.78:63439/api/connect?userName={userName}&userPwd={pwd}");
                //response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                ReturnModel s = JsonConvert.DeserializeObject<ReturnModel>(responseBody);
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);
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
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            // Need to call dispose on the HttpClient object
            // when done using it, so the app doesn't leak resources
            client.Dispose();
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
        private async void Register(string userName, string pwd)
        {
            // Create a New HttpClient object.
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync($"http://39.108.122.78:63439/api/connectregister?userName={userName}&pwd={pwd}");
            //response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            ReturnModel s = JsonConvert.DeserializeObject<ReturnModel>(responseBody);
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);
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
    public class ReturnModel
    {
        public string Code { get; set; }
        public string Msg { get; set; }
    }
}
