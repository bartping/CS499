using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.ComponentModel;

/*
 * This class contains all of the basic data loading, structures, and CRUD operation functions
 * for both the DictData (Dictionary Data) and CatData (Category Data) tables, as will as the 
 * structures for the Quiz page and other areas
 */

namespace LLL2
{
    
    public class Database
    {
        private readonly SQLiteConnection database; 
        private readonly static object collisionLock = new object(); //prevent multiple things from updating the database at once
        public ObservableCollection<DictData> Dictionary { get; set; }    
        public ObservableCollection<CatData> Categories { get; set; }


        //Connect to Database, create tables (which will initialize them if they don't exist
        public Database()
        {
            database =
              DependencyService.Get<App.IDatabaseConnection>().
              DbConnection();

            database.CreateTable<DictData>();
            this.Dictionary =
              new ObservableCollection<DictData>(database.Table<DictData>());

            database.CreateTable<CatData>();
            this.Categories =
              new ObservableCollection<CatData>(database.Table<CatData>());
        }

        //This is for observation purponses, and potentially for exporting the database
        public string GetDocsPath()
        {
            string docPath;
            docPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            return docPath;
        }

        public List<DictData> GetDictList() //Bring Sqlite Dictionary Data table into C# List for easier consumption
        {
            List<DictData> Dictionary;
            List<DictData> Result = new List<DictData>();
            Dictionary = database.Table<DictData>().ToList();
            foreach (DictData instance in Dictionary)
            {
                if (Result.Count == 0)
                {
                    Result.Add(instance);
                    continue;
                }
                int i = 0;
                foreach(DictData sorted in Result)
                {

                    if (String.Compare(instance.English, sorted.English) > 0)
                    {
                        i++;
                        continue;
                    }
                    else
                    {
                        Result.Insert(i, instance);
                        break;
                    }

                }

            }

            //Dictionary.Sort();
            return Result;
        }

        public List<DictData> GetSimilar(DictData Answer, int Lang) //Bring Sqlite Dictionary Data table into C# List for easier consumption
        {
            List<DictData> Dictionary;
            List<DictData> Result = new List<DictData>();
            Dictionary = database.Table<DictData>().ToList();
            int length = Answer.English.Length;

            foreach (DictData instance in Dictionary)
            {
                if (Lang == 0)
                {
                    if (String.Compare(instance.English.Substring(0,2),Answer.English.Substring(0,2)) == 0)
                    {
                        Result.Add(instance);
                        continue;
                    }
                }
                else
                {
                    if (instance.Spanish.Length > length - 2 || instance.Spanish.Length < length + 2)
                    {
                        Result.Add(instance);
                        continue;
                    }
                }
            }

            //Dictionary.Sort();
            return Result;
        }

        public List<CatData> GetCatList()   //Bring Sqlite Category Data table into C# List for easier consumption
        {
            return database.Table<CatData>().ToList();
        }

        public List<AllData> GetAllList(string filter) //Makes a filtered list by category, or all dictionary items depending on filter
        {
            List<AllData> queryList = new List<AllData>();
            List<DictData> dictdata = GetDictList();
            List<CatData> catdata = GetCatList();

            //Query by category
            var query1 = from dict in dictdata
                        join cat in catdata on dict.ID equals cat.Dict_ID
                         where cat.Category == filter
                        select new AllData {    Cat_ID = cat.ID,
                                        Dict_ID = dict.ID,
                                        English = dict.English,
                                        Spanish = dict.Spanish,
                                        Familiarity = dict.Familiarity,
                                        LastQuiz = dict.LastQuiz,
                                        Category = cat.Category
                        };

            //ignore category, give all words
            var query2 = from dict in dictdata
                         select new AllData
                         {
                             Cat_ID = -1,
                             Dict_ID = dict.ID,
                             English = dict.English,
                             Spanish = dict.Spanish,
                             Familiarity = dict.Familiarity,
                             LastQuiz = dict.LastQuiz,
                             Category = "All"
                         };

            if (filter.Equals("All")) //if we're looking for All, create the All version, otherwise, do the filtered one
            {
                foreach (var word_x_cat in query2)
                {
                    queryList.Add(word_x_cat);
                }
            }
            else
            {
                foreach (var word_x_cat in query1)
                {
                    queryList.Add(word_x_cat);
                }
            }
            return queryList;
        }

        public List<CatList> CategoryList(int includeAll) /* Makes a list of all current categories */
        {
            List<CatList> queryList = new List<CatList>();
            List<string> catdata = GetCatList().Select(obj => obj.Category).Distinct().ToList(); //string list of distinct categories from GetCatList()

            //int n = catdata.Count; //number of categories we found


            //Add to the querylist in alphabetical order
            foreach (var newcat in catdata)
            {
                if (queryList.Count() == 0)
                {
                    queryList.Add(new CatList { Category = newcat }); //Add each category value to the new public list
                    continue;
                }
                int i = 0;
                int added = 0;
                foreach (var inlist in queryList)
                {
                    if (String.Compare(newcat, inlist.Category) > 0)
                    {
                        i++;
                        continue;
                    }
                    else
                    {
                        queryList.Insert(i,new CatList { Category = newcat });
                        added = 1;
                        break;
                    }
                }
                if(added == 0)
                    queryList.Add(new CatList { Category = newcat });
            }
            if(includeAll==1)
                queryList.Insert(0, new CatList { Category = "All" }); //Add an 'All' category to the beginning

            return queryList; //return the list
        }

        public DictData GetEntry(int id) /* Get a specific entry by ID */
        {
            lock (collisionLock)
            {
                return database.Table<DictData>().
                  FirstOrDefault(dict => dict.ID == id);
            }
        }

        public DictData GetEntry(string english) /* Get a specific entry by English word*/
        {
            lock (collisionLock)
            {
                return database.Table<DictData>().
                  FirstOrDefault(dict => dict.English == english);
            }
        }

        public int SaveEntry(DictData dictInstance) /* Save a new entry or edit */
        {
            lock (collisionLock)
            {
                if (dictInstance.ID != 0)
                {
                    database.Update(dictInstance);
                    return dictInstance.ID;
                }
                else
                {
                    database.Insert(dictInstance);
                    return dictInstance.ID;
                }
            }
        }

        public int SaveCatEntry(CatData catInstance) /* Save a new category entry or edit */
        {
            lock (collisionLock)
            {
                if (catInstance.ID != 0)
                {
                    database.Update(catInstance);
                    return catInstance.ID;
                }
                else
                {
                    database.Insert(catInstance);
                    return catInstance.ID;
                }
            }
        }

        public int DeleteCatEntry(CatData catInstance) /* delete an entry by ID */
        {
            var id = catInstance.ID;
            if (id != 0)
            {
                lock (collisionLock)
                {
                    database.Delete<CatData>(id);
                }
            }
            this.Categories.Remove(catInstance);
            return id;
        }

        public int DeleteEntry(DictData dictInstance) /* delete an entry by ID */
        {
            var id = dictInstance.ID;
            if (id != 0)
            {
                lock (collisionLock)
                {
                    List<CatData> removeList = this.Categories.Where(obj => obj.Dict_ID.Equals(id)).ToList();
                    foreach (CatData category in removeList)
                        DeleteCatEntry(category);
                    database.Delete<DictData>(id);
                }
            }
            this.Dictionary.Remove(dictInstance);
            return id;
        }

        public void DoScore(DictData dictInstance, int points) /* Take a score value and add it to Familiarity */
        {
            lock (collisionLock)
            {
                if (points > 0)
                {
                    if (dictInstance.Familiarity >= LLLSettings.MAXFAMILIARITY)
                        return; //no need to update if already max or higher
                    dictInstance.Familiarity += points;
                    dictInstance.LastQuiz = DateTime.Now;
                }
                else
                {
                    if (dictInstance.Familiarity < 0)
                        return; //Can't take away points that aren't there

                }
                database.Update(dictInstance);
            }
        }   
    }

/*
 * Class information for each table
 */
    public class DictData /* Dictionary Table */
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string English { get; set; }
        public string Spanish { get; set; }
        public int Familiarity { get; set; }
        public DateTime LastQuiz { get; set; }
    }
    public class CatData /* Category Table */
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int Dict_ID { get; set; }
        public string Category { get; set; }
    }
    public class Question /* Quiz Question structure */
    {
        public string Word { get; set; }
        public string Ans1 { get; set; }
        public string Ans2 { get; set; }
        public string Ans3 { get; set; }
        public string Ans4 { get; set; }
        public string Answer { get; set; }
        public int AnsID { get; set; }
    }
    public class AllData /* Dictionary X Category cross reference */
    {
        public int Cat_ID { get; set; }
        public int Dict_ID { get; set; }
        public string English { get; set; }
        public string Spanish { get; set; }
        public int Familiarity { get; set; }
        public DateTime LastQuiz { get; set; }
        public string Category { get; set; }
    }
    public class CatList /* Category List */
    {
        public string Category { get; set; }
        
        /* Probably doesn't need to be its own class, but this
         * allows for expansion, so...
         */
    }
}