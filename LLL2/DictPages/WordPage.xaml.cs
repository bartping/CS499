using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace LLL2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WordPage : ContentPage
    {

        DictData activeWord;
        List<CatList> picked;

        public WordPage(DictData detailWord)
        {
            
            InitializeComponent();
            this.activeWord = detailWord;

            EngWord.Text = detailWord.English;
            SpanWord.Text = detailWord.Spanish;

            catlistView.ItemsSource = App.dataAccess.catData.Where(obj => obj.Dict_ID.Equals(this.activeWord.ID)).ToList();
            fillPick();
        }

        private void BackClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new AddDict();
        }

        private void DelClick(object sender, EventArgs e)
        {
            CatData selectedCat = catlistView.SelectedItem as CatData;
            if (selectedCat is null)
                return;
            App.dataAccess.DeleteCatEntry(selectedCat);
            catlistView.ItemsSource = App.dataAccess.catData.Where(obj => obj.Dict_ID.Equals(this.activeWord.ID)).ToList();
        }

        private void pickChange(object sender, EventArgs e)
        {
            catEntry.Text = catpick.SelectedItem.ToString();
        }
        private void fillPick()
        {
            this.picked = App.dataAccess.CategoryList(0);
            foreach(var cat in this.picked)
                catpick.Items.Add(cat.Category);
        }
        private void AddCatClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(catEntry.Text))
            {
                List<CatData> catlist = App.dataAccess.catData.Where(obj => obj.Dict_ID.Equals(this.activeWord.ID)).ToList();
                List<CatData> matched = catlist.Where(obj => obj.Category.Equals(catEntry.Text)).ToList();
                if (matched.Count > 0)
                {
                    DisplayAlert("Required", "Category already exists.", "OK");
                    return;
                }
                App.dataAccess.SaveCatEntry(new CatData
                {
                    Dict_ID = this.activeWord.ID,
                    Category = catEntry.Text,
                });
                App.dataAccess = new Database(); //refresh database on change
                catlistView.ItemsSource = App.dataAccess.catData.Where(obj => obj.Dict_ID.Equals(this.activeWord.ID)).ToList();
                catEntry.Text = string.Empty;
            }
        }
       
    }
   
}