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
    public partial class QuizLand : ContentPage
    {
        private Database Database;
        public List<CatList> CatList;

        public QuizLand()
        {
            InitializeComponent();
            this.Database = new Database();
            fillPick();
        }
        private void fillPick()
        {
            CatList = Database.CategoryList();
            int n = CatList.Count;

            for (int i = 0; i < n; i++)
                catpick.Items.Add(CatList[i].Category);
        }
        private void HomeClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }

        private void pickChange(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                LLLSettings.currentCategory = picker.Items[selectedIndex];
            }
        }

        private void QuizClick(object sender, EventArgs e)
        {
            if (catpick.SelectedIndex != -1)
            {
                LLLSettings.currentCategory = catpick.Items[catpick.SelectedIndex];
            }
            App.Current.MainPage = new QuizPage();
        }
    }
}