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
    public partial class QuizPage : ContentPage
    {
        public List<DictData> Dictionary;
        public Question QuestionList = new Question();

        public QuizPage()
        {
            InitializeComponent();
        }
        public async void GetList()
        {
            Dictionary = await App.Database.GetDictAsync();
            await Task.Delay(2000);
        }
        private void HomeClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }

       /*
        * Button responses for clicking correct or incorrect
        */

        private async void Ans1Click(object sender, EventArgs e)
        {
            // Process button clicks. All are the same, but different records affected
            answer1.IsEnabled = false; //Lock all buttons so only one is clicked and only clicked once
            answer2.IsEnabled = false;
            answer3.IsEnabled = false;
            answer4.IsEnabled = false;
            if (QuestionList.Ans1 == QuestionList.Answer) //Right answer
            {
                quiztext.Text = "Correct!";
                answer1.BackgroundColor = Color.Green;
            }
            else
            {
                quiztext.Text = "Incorrect!";           //Wrong answer
                answer1.BackgroundColor = Color.Red;
            }
            //Need to build the familiarity system
            await Task.Delay(2000);                     //Wait so we can actually see it
            App.Current.MainPage = new QuizPage();      //Reload page for new question
        }

        private async void Ans2Click(object sender, EventArgs e)
        {
            answer1.IsEnabled = false;
            answer2.IsEnabled = false;
            answer3.IsEnabled = false;
            answer4.IsEnabled = false;
            if (QuestionList.Ans2 == QuestionList.Answer)
            {
                quiztext.Text = "Correct!";
                answer2.BackgroundColor = Color.Green;
            }
            else
            {
                quiztext.Text = "Incorrect!";
                answer2.BackgroundColor = Color.Red;
            }
            await Task.Delay(2000);
            App.Current.MainPage = new QuizPage();
        }
        private async void Ans3Click(object sender, EventArgs e)
        {
            answer1.IsEnabled = false;
            answer2.IsEnabled = false;
            answer3.IsEnabled = false;
            answer4.IsEnabled = false;
            if (QuestionList.Ans3 == QuestionList.Answer)
            {
                quiztext.Text = "Correct!";
                answer3.BackgroundColor = Color.Green;
            }
            else
            {
                quiztext.Text = "Incorrect!";
                answer3.BackgroundColor = Color.Red;
            }
            await Task.Delay(2000);
            App.Current.MainPage = new QuizPage();
        }
        private async void Ans4Click(object sender, EventArgs e)
        {
            answer1.IsEnabled = false;
            answer2.IsEnabled = false;
            answer3.IsEnabled = false;
            answer4.IsEnabled = false;
            if (QuestionList.Ans4 == QuestionList.Answer)
            {
                quiztext.Text = "Correct!";
                answer4.BackgroundColor = Color.Green;
            }
            else
            {
                quiztext.Text = "Incorrect!";
                answer4.BackgroundColor = Color.Red;
            }
            await Task.Delay(2000);
            App.Current.MainPage = new QuizPage();
        }

        protected int GetWrong(List<int> Used,int size)
        {   //Let's get a wrong answer! Need the list of used items and the size of the dictionary

            Random rnd = new Random();
            int Pos = rnd.Next(0, size); //Pick a number
            int noloop = 0;             //Loop Avoidance
            while (Used.Contains(Pos) && noloop < 100) //if we chose the one that's already used, choose another
            {   //This is a bit hokey. Maybe improve somehow?
                Pos = rnd.Next(0, size);
                noloop++; //only try 100 times to avoid duplicate so we don't get stuck. Should not need more than that, but...
            }
            return Pos;  //Return the position in the dictionary of a valid wrong answer
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new MainPage();
            return true;
        }
        protected async void QuestionText()
        {
            /*
             * Function to retrieve a word from the word list and generate a multiple choice
             * question.
             */

            List<DictData> Dictionary = await App.Database.GetDictAsync(); //main dictionary list
            List<int> Used = new List<int>();   //keep track of which words we've already used as a choice

            int size = Dictionary.Count;        //number of dictionary records
            if (size < 4)                       //if we don't have enough records to generate even 1 question, don't do anything
            {
                QuestionList.Word = "Database too small.";
                return;
            }
            
            //Need to build the category system

            Random rnd = new Random();          //initialize the random number generator
            int corPos = rnd.Next(1, 5);        //Determine position of the correct answer
            int quizWord = rnd.Next(0, size);   //Choose the word
            int wrong;                          //placeholder for the 'wrong' answer chosen  
            QuestionList.AnsID = quizWord;      //track the ID of the correct answer for later processing
            Used.Add(quizWord);                 //Add correct answer to the list of words already chosen
            int isEng = rnd.Next(0, 2);         //Coin toss: English or Spanish?
            if (isEng == 0)     //Asked word is in Spanish
            {
                QuestionList.Word = Dictionary[quizWord].Spanish;
                QuestionList.Answer = Dictionary[quizWord].English;
                //Populate all answers with wrong answers
                QuestionList.Ans1 = Dictionary[wrong = GetWrong(Used, size)].English;
                Used.Add(wrong);
                QuestionList.Ans2 = Dictionary[wrong = GetWrong(Used, size)].English;
                Used.Add(wrong);
                QuestionList.Ans3 = Dictionary[wrong = GetWrong(Used, size)].English;
                Used.Add(wrong);
                QuestionList.Ans4 = Dictionary[wrong = GetWrong(Used, size)].English;

                switch (corPos)  //Populate correct answer position with the correct answer
                {
                    case 1:
                        QuestionList.Ans1 = Dictionary[quizWord].English;
                        break;
                    case 2:
                        QuestionList.Ans2 = Dictionary[quizWord].English;
                        break;
                    case 3:
                        QuestionList.Ans3 = Dictionary[quizWord].English;
                        break;
                    case 4:
                        QuestionList.Ans4 = Dictionary[quizWord].English;
                        break;
                }

            }
            else  //Same as above, but asked word is in English
            {
                QuestionList.Word = Dictionary[quizWord].English;
                QuestionList.Answer = Dictionary[quizWord].Spanish;

                QuestionList.Ans1 = Dictionary[wrong = GetWrong(Used, size)].Spanish;
                Used.Add(wrong);
                QuestionList.Ans2 = Dictionary[wrong = GetWrong(Used, size)].Spanish;
                Used.Add(wrong);
                QuestionList.Ans3 = Dictionary[wrong = GetWrong(Used, size)].Spanish;
                Used.Add(wrong);
                QuestionList.Ans4 = Dictionary[wrong = GetWrong(Used, size)].Spanish;

                switch (corPos)
                {
                    case 1:
                        QuestionList.Ans1 = Dictionary[quizWord].Spanish;
                        break;
                    case 2:
                        QuestionList.Ans2 = Dictionary[quizWord].Spanish;
                        break;
                    case 3:
                        QuestionList.Ans3 = Dictionary[quizWord].Spanish;
                        break;
                    case 4:
                        QuestionList.Ans4 = Dictionary[quizWord].Spanish;
                        break;
                }
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