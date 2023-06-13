using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    // $G$ DSN-999 (-15) If this Enum is a nested enum of this class it shuold be private. Otherwise it should be in a seperate file (regarding all enums).
    // $G$ NTT-003 (-5) not all exeptions handaled.
    public class Program
    {
        public static void Main()
        {
            ConsoleUI consoleUI = new ConsoleUI();
            consoleUI.RunGarage();
        }
    }
}
