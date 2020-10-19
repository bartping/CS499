using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;

/*
 * This class contains all of the setting data, such as current category and other such variables
 */

namespace LLL2
{

    public class LLLSettings
    {
        public static string currentCategory = "All";
        public static int MAXFAMILIARITY = 10;
        public static int MAXIMPORTCAT = 5;
        public static string impPath = "http://cs.indstate.edu/~bping1/lll/esp.lex"; /* Default import path. Can be any web address */
    }
    
}