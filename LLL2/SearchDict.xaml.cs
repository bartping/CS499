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
    public partial class SearchDict : ContentPage
    {
        public SearchDict()
        {
            InitializeComponent();
        }
        private void DictClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new DictPage();
        }
        private async void SearchClick(object sender, EventArgs e)
        {
            string entered = searchEntry.Text;
            List<DictData> newlist = await App.Database.GetDictAsync();

            listView.ItemsSource = FilterList(newlist, entered);
        }
        private void HomeClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new DictPage();
            return true;
        }

        private List<DictData> FilterList(List<DictData> input, string filter)
        {
            List<DictData> matched = input.Where(obj => obj.Spanish.Contains(filter) || obj.English.Contains(filter)).ToList();
            if (matched != null) 
                return matched;
            
            return input;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await App.Database.GetDictAsync();
            
        }
    }
}