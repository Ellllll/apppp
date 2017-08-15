using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            BindingContext = new ToMatch();
		}

	}

    public class ToMatch
    {
        
    }
}