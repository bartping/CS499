using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LLL2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WordPage : ContentPage
    {
        public WordPage(EventArgs e)
        {
            InitializeComponent();
        }

        private void SearchClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new SearchDict();
        }
    }
   
}