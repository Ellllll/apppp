using System;
using System.Collections.Generic;
using System.Linq;
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
            var sqliteInfo = new Sqlite2();


        }
        private bool CanMatchExecute(object args)
        {
            return true;
        }

    }
}