using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final
{
    class Global
    {
        public static int lifePoint = 3;
        public static List<int> stgList = new List<int>();
    }
    class Info
    {
        public int gameCol;
        public int gameRow;
        public int playerCol;
        public int playerRow;
        public int currentStg;
        public Button[,] button;
        public Label stg;
        public Label life;
        public Info(int a, int b,int c, int d, int e)
        {
            gameCol = a;
            gameRow = b;
            playerCol = c;
            playerRow = d;
            currentStg = e;
            button = null;
            stg = null;
            life = null;
        }
    }
}
