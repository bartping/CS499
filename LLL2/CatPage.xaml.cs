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
            List<CatData> newlist = App.dataAccess.GetCatList();
            listView.ItemsSource = FilterList(newlist, entered);
        }
        private void HomeClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }

        private List<CatData> FilterList(List<CatData> input, string filter)
        {
            List<CatData> matched = input.Where(obj => obj.Category.Contains(filter)).ToList();
            if (matched != null)
                return matched;

            return input;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = App.dataAccess.GetCatList();

        }
    }
}