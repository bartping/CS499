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
    public partial class CatPage : ContentPage
    {
        public CatPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new DictPage();
            return true;
        }
        private void DictClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new DictPage();
        }
        private void SearchClick(object sender, EventArgs e)
        {
            string entered = searchEntry.Text;
            List<CatList> newlist = App.dataAccess.CategoryList();
            listView.ItemsSource = newlist;
        }
        private void HomeClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = App.dataAccess.CategoryList();

        }
    }
}