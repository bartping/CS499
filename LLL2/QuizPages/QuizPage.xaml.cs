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
    public partial class QuizPage : ContentPage         /* Establish initial page state */
    {
        public Question QuestionList = new Question();  /* Establish a new Question and list of answers */

        public QuizPage()                               /* Things to run when the page is initialized */
        {
            InitializeComponent();
        }

        private void HomeClick(object sender, EventArgs e)  /* Home button/logo click event */
        {
            App.Current.MainPage = new MainPage();
        }

        private void BackClick(object sender, EventArgs e)  /*Back button click event */
        {
            App.Current.MainPage = new QuizLand();
        }

        protected override bool OnBackButtonPressed()       /* Phone back button click event */
        {
            App.Current.MainPage = new QuizLand();
            return true;
        }

        /*
         * Answer Button responses for clicking correct or incorrect
         *  All events are basically the same, just checking which one you selected.
         *  Could probably modify this to use the sender so only one copy of this is needed
         */

        private async void Ans1Click(object sender, EventArgs e)    /* First answer button click event */
        {
            // Process button clicks. All are the same, but different records affected
            answer1.IsEnabled = false; //Lock all buttons so only one is clicked and only clicked once
            answer2.IsEnabled = false;
            answer3.IsEnabled = false;
            answer4.IsEnabled = false;

            if (QuestionList.Ans1 == QuestionList.Answer) //Right answer
            {
                quiztext.Text = "Correct!";
                App.dataAccess.DoScore(App.dataAccess.GetEntry(QuestionList.AnsID), 1);
                answer1.BackgroundColor = Color.Green;
            }
            else
            {
                App.dataAccess.DoScore(App.dataAccess.GetEntry(QuestionList.AnsID), -1);
                quiztext.Text = "Incorrect!";           //Wrong answer
                answer1.BackgroundColor = Color.Red;
            }
            //Need to build the familiarity system
            await Task.Delay(2000);                     //Wait so we can actually see it
            App.Current.MainPage = new QuizPage();      //Reload page for new question
        }
        private async void Ans2Click(object sender, EventArgs e)    /* Second answer button click event */
        {
            answer1.IsEnabled = false;
            answer2.IsEnabled = false;
            answer3.IsEnabled = false;
            answer4.IsEnabled = false;
            if (QuestionList.Ans2 == QuestionList.Answer)
            {
                App.dataAccess.DoScore(App.dataAccess.GetEntry(QuestionList.AnsID), 1);
                quiztext.Text = "Correct!";
                answer2.BackgroundColor = Color.Green;
            }
            else
            {
                App.dataAccess.DoScore(App.dataAccess.GetEntry(QuestionList.AnsID), -1);
                quiztext.Text = "Incorrect!";
                answer2.BackgroundColor = Color.Red;
            }
            await Task.Delay(2000);
            App.Current.MainPage = new QuizPage();
        }   
        private async void Ans3Click(object sender, EventArgs e)    /*Third answer button click event */
        {
            answer1.IsEnabled = false;
            answer2.IsEnabled = false;
            answer3.IsEnabled = false;
            answer4.IsEnabled = false;
            if (QuestionList.Ans3 == QuestionList.Answer)
            {
                App.dataAccess.DoScore(App.dataAccess.GetEntry(QuestionList.AnsID), 1);
                quiztext.Text = "Correct!";
                answer3.BackgroundColor = Color.Green;
            }
            else
            {
                App.dataAccess.DoScore(App.dataAccess.GetEntry(QuestionList.AnsID), -1);
                quiztext.Text = "Incorrect!";
                answer3.BackgroundColor = Color.Red;
            }
            await Task.Delay(2000);
            App.Current.MainPage = new QuizPage();
        }   
        private async void Ans4Click(object sender, EventArgs e)    /* Fourth answer button click event */
        {
            answer1.IsEnabled = false;
            answer2.IsEnabled = false;
            answer3.IsEnabled = false;
            answer4.IsEnabled = false;
            if (QuestionList.Ans4 == QuestionList.Answer)
            {
                App.dataAccess.DoScore(App.dataAccess.GetEntry(QuestionList.AnsID), 1);
                quiztext.Text = "Correct!";
                answer4.BackgroundColor = Color.Green;
            }
            else
            {
                App.dataAccess.DoScore(App.dataAccess.GetEntry(QuestionList.AnsID), -1);
                quiztext.Text = "Incorrect!";
                answer4.BackgroundColor = Color.Red;
            }
            await Task.Delay(2000);
            App.Current.MainPage = new QuizPage();
        }   

        protected string GetWrong(List<string> Used, int size, int lang)     /* Choose some wrong answers from the Database to fill in */
        {   //Let's get a wrong answer! Need the list of used items and the size of the dictionary

            Random rnd = new Random();
            int Pos = rnd.Next(0, size); //Pick a number
            string choice;
            if (lang == 0 )
                choice = App.dataAccess.GetEntry(Pos).English;
            else
                choice = App.dataAccess.GetEntry(Pos).Spanish;

            int noloop = 0;             //Loop Avoidance
            while (Used.Contains(choice) && noloop < 100) //if we chose the one that's already used, choose another
            {   //This is a bit hokey. Maybe improve somehow? perhaps increment and check again?
                Pos = rnd.Next(0, size);
                choice = App.dataAccess.GetEntry(Pos).English;
                noloop++; //only try 100 times to avoid duplicate so we don't get stuck. Should not need more than that, but...
            }
            if(lang == 0)
                return App.dataAccess.GetEntry(Pos).English;  //Return the position in the dictionary of a valid wrong answer
            else
                return App.dataAccess.GetEntry(Pos).Spanish;
        }

        protected string MakeWrong(List<int> Used, int size)     /* Choose some wrong answers from the Database to fill in */
        {   //Let's get a wrong answer! Need the list of used items and the size of the dictionary

            Random rnd = new Random();
            string word = QuestionList.Answer;
            List<int> Consonants = new List<int>();
            string test = "djrs";
            int i = 0;
            foreach (char letter in word){
                if (test.Contains(letter))
                    Consonants.Add(i++);
            }
             

            return word;
        }
        

        

        protected void QuestionText()               /* Retrieve a word from the word list and generate a multiple choice question */
        {
            string useCat = LLLSettings.currentCategory;
            List<AllData> FullDict = App.dataAccess.GetAllList(useCat);
            List<AllData> filtered;  // dictionary list filtered by category
            filtered = FullDict.Where(obj => obj.Category.Equals(useCat)).ToList(); 

            List<string> Used = new List<string>();   //keep track of which words we've already used as a choice

            int catSize = filtered.Count;
            if(catSize < 4)
            {   /* Popup alert to tell us we don't have enough words in this category to make a question
                 * Perhaps modify to pull words from the whole dictionary instead, or maybe a list of fakes?
                 */
                DisplayAlert("Required", "No List Found.", "OK");
                return;
            }

            Random rnd = new Random();          //initialize the random number generator
            int corPos = rnd.Next(1, 5);        //Determine position of the correct answer button
            int findWord = rnd.Next(0, catSize);
            int quizWord;                       //Get the ID for the chosen word
            quizWord = filtered[findWord].Dict_ID;

            QuestionList.AnsID = quizWord;      //track the DictData.ID of the correct answer for later processing
           
            int isEng = rnd.Next(0, 2);         //Coin toss: English or Spanish?

            if (isEng == 0)
            {
                QuestionList.Word = filtered[findWord].Spanish;
                QuestionList.Answer = filtered[findWord].English;
            }
            else
            {
                QuestionList.Word = filtered[findWord].English;
                QuestionList.Answer = filtered[findWord].Spanish;
            }

                Used.Add(QuestionList.Answer);                    //Add the correct answer to the list of words for answers
                //Populate all answers with wrong answers
                QuestionList.Ans1 = GetWrong(Used, catSize, isEng);
                Used.Add(QuestionList.Ans1);
                QuestionList.Ans2 = GetWrong(Used, catSize, isEng);
                Used.Add(QuestionList.Ans2);
                QuestionList.Ans3 = GetWrong(Used, catSize, isEng);
                Used.Add(QuestionList.Ans3);
                QuestionList.Ans4 = GetWrong(Used, catSize, isEng);

                switch (corPos)  //Populate correct answer position with the correct answer
                {
                    case 1:
                        QuestionList.Ans1 = QuestionList.Answer;
                        break;
                    case 2:
                        QuestionList.Ans2 = QuestionList.Answer;
                        break;
                    case 3:
                        QuestionList.Ans3 = QuestionList.Answer;
                        break;
                    case 4:
                        QuestionList.Ans4 = QuestionList.Answer;
                        break;
                }


            //Fill in the text on QuizPage.xaml
            quiztext.Text = QuestionList.Word;
            answer1.Text = QuestionList.Ans1;
            answer2.Text = QuestionList.Ans2;
            answer3.Text = QuestionList.Ans3;
            answer4.Text = QuestionList.Ans4;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            QuestionText();     //Load up a question!
        }
    }
}