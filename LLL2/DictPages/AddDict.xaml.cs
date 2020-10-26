using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http.Headers;

namespace LLL2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDict : ContentPage
    {
        public AddDict()
        {
            InitializeComponent();
            listView.ItemsSource = FilterList(App.dataAccess.GetDictList()); /* Initialize the List View */
        }

        /* Event Handlers */


        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new DictPage();
            return true;
        }

        private void DictClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new DictPage();
        }

        private void HomeClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }

        private void SearchClick(object sender, EventArgs e)
        {
            List<DictData> newList = new List<DictData>();
            listView.ItemsSource = FilterList(newList);
        }

        private void listView_ItemSelected(object sender, EventArgs e)
        {
            DictData selectedWord = listView.SelectedItem as DictData;
            App.Current.MainPage = new WordPage(selectedWord);
        }

        private void AddClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(engEntry.Text) && !string.IsNullOrWhiteSpace(espEntry.Text))
            {
                App.dataAccess.SaveEntry(new DictData
                {
                    English = engEntry.Text,
                    Spanish = espEntry.Text,
                    Familiarity = 0,
                    LastQuiz = DateTime.MinValue
                });

                App.dataAccess = new Database(); //refresh database on change
                engEntry.Text = espEntry.Text = string.Empty;
            }
            else
                DisplayAlert("Required", "You must enter both English and Spanish words.", "OK");
        }

        /* Utility functions */

        private List<DictData> FilterList(List<DictData> input)
        {

            List<DictData> matched = App.dataAccess.GetDictList().Where(obj => obj.Spanish.Contains(espEntry.Text) && obj.English.Contains(engEntry.Text)).ToList();
            if (matched != null)
                return matched;

            return input;
        }
    }
}