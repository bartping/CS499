using SQLite;
using LLL2.Droid;
using System.IO;
using static LLL2.App;
using Android.Content.Res;
using Java.IO;
using Android.Content;
using Xamarin.Forms.Platform.Android;
using Android.App;


[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection_Android))]
namespace LLL2.Droid
{
    public class DatabaseConnection_Android : IDatabaseConnection
    {
        public SQLiteConnection DbConnection()
        {

            var dbName = "dictdata.db3";
            string topath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);

            return new SQLiteConnection(topath);
        }
    }

}