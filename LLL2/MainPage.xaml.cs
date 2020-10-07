using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LLL2
{

        public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

        }

        private void DictClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new DictPage();
        }

        private void QuizClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new QuizPage();
        }
    }
}
