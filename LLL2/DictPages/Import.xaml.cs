using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LLL2
{
    

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Import : ContentPage
    {
        
        public Import()
        {
            InitializeComponent();
            usePath.Text = LLLSettings.impPath;                 /* Populate the Import field with default location */
            usePath.Placeholder = LLLSettings.impPath;
        }

        /* Event Handlers */

        private void ImpClick(object sender, EventArgs e)
        {
            DoImport(usePath.Text);
            App.Current.MainPage = new DictPage();
        }

        private void DictClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new DictPage();
        }

        /* Utility Functions */

        public static void DoImport(string doPath)              /* Import dictionary and category data from a tab-delimited text file */
        {
            WebClient client = new WebClient();                 /* start a background client so we can go get our file */
            Stream stream = client.OpenRead(doPath);
            StreamReader reader = new StreamReader(stream);

            string line;                                        /* Make some placeholder string values */
            string newEng;
            string newSpan;

            string[] cats = new string[LLLSettings.MAXIMPORTCAT];  /* Array of strings to keep track of our categories */

            while ((line = reader.ReadLine()) != null)          /* Read each line and get it ready for import */
            {
                int cols = line.Split('\t').Length;
                if (cols < 2) continue;                         /* Don't try to import a line with fewer than 2 cols */
                newEng = line.Split('\t')[0].Trim().ToUpper();
                if (newEng.Length < 1) continue;
                newSpan = line.Split('\t')[1].Trim().ToUpper();
                if (newSpan.Length < 1) continue;

                int i = 2;
                int j = 0;

                while(j < LLLSettings.MAXIMPORTCAT && i < cols && (cats[j] = line.Split('\t')[i].Trim().ToUpper()) != null )
                {
                    i++;
                    j++;
                }

                List<DictData> currDict = App.dataAccess.GetDictList();
                List<DictData> match;
                int wordID;
                match = currDict.Where(obj => obj.English.Contains(newEng) || obj.Spanish.Contains(newSpan)).ToList();
                                     /* Check the current dictionary to see if the word exists
                                        This might come out with definition support later */

                if (match.Count > 0)
                {
                    App.dataAccess.SaveEntry(new DictData
                    {                                           /* If we found a match, update it. */
                        ID = match[0].ID,
                        English = newEng,
                        Spanish = newSpan,
                        Familiarity = match[0].Familiarity,
                        LastQuiz = match[0].LastQuiz
                    });
                    wordID = match[0].ID;                       /* Save that ID for tying to categories */
                }
                else
                {
                    wordID = App.dataAccess.SaveEntry(new DictData
                    {                                           /* Not found? Create an entry */
                        English = newEng,
                        Spanish = newSpan,
                        Familiarity = 0,
                        LastQuiz = DateTime.MinValue
                    });
                }

                /* Now, let's do the categories */

                List<CatData> currCat = App.dataAccess.GetCatList();
                List<CatData> catMatch;
                for (i = 0; i < j; i++)
                {
                    catMatch = currCat.Where(obj => obj.Category.Contains(cats[i]) && obj.Dict_ID.Equals(wordID)).ToList();
                    if (catMatch.Count > 0)
                        continue;                   /* Category already set for this word, go to the next */
                    App.dataAccess.SaveCatEntry(new CatData
                    {
                        Dict_ID = wordID,
                        Category = cats[i]
                    });
                }
            }

 
        }
    }
    
}