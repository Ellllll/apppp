using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Match : MasterDetailPage
	{
        
        public Match ()
		{
			InitializeComponent ();
            BindingContext = new Tomatch();
		}

	}

    public class Tomatch
    {
        
        public Tomatch()
        {
            toMatch = new Command(Matching,CanMatchExecute);

        }
        private ICommand toMatch;
        public ICommand ToMatch
        {
            get { return toMatch; }
            set { toMatch = value; }
        }
        private void Matching(object args)
        {
            var TCPlistener = new TCPListener();
           
            Byte[] data=new Byte[12];
                // String to store the response ASCII representation.
            String responseData = String.Empty;
                // Read the first batch of the TcpServer response bytes.
            TCPListener.stream.Read(data, 0, data.Length);
            responseData = Encoding.UTF8.GetString(data);
            if (responseData == "匹配成功")
            {
                Toast.MakeText(Forms.Context, "匹配成功", ToastLength.Short).Show();
                var page = new Call();
                Application.Current.MainPage = new Call();
            }

        }
        private bool CanMatchExecute(object args)
        {
            return true;///////////////////////////////////////////////////////
        }

    }
}