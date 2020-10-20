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
        private void SearchClick(object sender, EventArgs e)
        {
            string entered = searchEntry.Text;
            List<DictData> dictlist = App.dataAccess.GetDictList();
            listView.ItemsSource = FilterList(dictlist, entered);
        }
        private void HomeClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }
        private void WordClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new WordPage(e);
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = App.dataAccess.GetDictList();
            
        }
    }
}