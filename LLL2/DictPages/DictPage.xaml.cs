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
    public partial class DictPage : ContentPage
    {
        public DictPage()
        {
            InitializeComponent();
        }

        /* Event Handlers */
        private void HomeClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }
        
        private void AddDictClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new AddDict();
        }

        private void ImpClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new Import();
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new MainPage();
            return true;
        }
    }
}