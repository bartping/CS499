using System;
using System.IO;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace LLL2
{
    public partial class App : Application
    {
        // public static Database database;
        public static Database dataAccess;
        
        public interface IDatabaseConnection
        {
            SQLite.SQLiteConnection DbConnection();
        }

        public App()
         {
            InitializeComponent();
            dataAccess = new Database();
            if (dataAccess.GetDictList().Count==0) //run the import at start time if there's no data
                Import.DoImport(LLLSettings.impPath);
            App.Current.MainPage = new MainPage();
         }
        

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}