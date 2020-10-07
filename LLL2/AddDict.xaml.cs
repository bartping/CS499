using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LLL2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDict : ContentPage
    {
        public AddDict()
        {
            InitializeComponent();
        }
        private void DictClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new DictPage();
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

 

        async void OnButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(engEntry.Text) && !string.IsNullOrWhiteSpace(espEntry.Text))
            {
                await App.Database.SaveDictAsync(new DictData
                {
                    English = engEntry.Text,
                    Spanish = espEntry.Text,
                    Familiarity = 0,
                    LastQuiz = 0
                });

                engEntry.Text = espEntry.Text = string.Empty;
            }
            else
                await DisplayAlert("Required", "You must enter both English and Spanish words.", "OK");
        }


    }
}