using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace LLL2
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<DictData>().Wait();
        }

        public Task<List<DictData>> GetDictAsync()
        {
            return _database.Table<DictData>().ToListAsync();
        }


        public Task<int> SaveDictAsync(DictData dictdata)
        {
            return _database.InsertAsync(dictdata);
        }

    }
    public class DictData
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string English { get; set; }
        public string Spanish { get; set; }
        public int Familiarity { get; set; }
        public int LastQuiz { get; set; }
    }
    public class CatData
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int Dict_ID { get; set; }
        public string Category { get; set; }
    }
    public class Question
    {
        public string Word { get; set; }
        public string Ans1 { get; set; }
        public string Ans2 { get; set; }
        public string Ans3 { get; set; }
        public string Ans4 { get; set; }
        public string Answer { get; set; }
        public int AnsID { get; set; }
    }

}