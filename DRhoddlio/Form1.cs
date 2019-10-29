using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUIImageArray;

namespace DRhoddlio
{
    public partial class Form1 : Form
    {
        int x = 0;
        int y = 0;
        public int[,] gameBoardArr;
        GImageArray gameBoard;

        public Form1()
        {
            InitializeComponent();
            populateGameBoardList();
        }

        public void populateGameBoardList()
        {
            gameBoardArr = new int[8, 8];
            while (x < 8)
            {
                y = 0;
                while (y < 8)
                {
                    
                    gameBoardArr[x, y] = 10;
                    y++;
                }
                x++;
            }
            gameBoardArr[3, 3] = 1;
            gameBoardArr[3, 4] = 0;
            gameBoardArr[4, 4] = 1;
            gameBoardArr[4, 3] = 0;

            gameBoard = new GImageArray(this, gameBoardArr, 150, 0, 7, 7, 5, "C:/Users/b9026473/source/repos/DRhoddlio/DRhoddlio/images/");
            gameBoard.Which_Element_Clicked += new GImageArray.ImageClickedEventHandler(Which_Element_Clicked);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            populateGameBoardList();
        }

        private void Which_Element_Clicked(object sender, EventArgs e)
        {
            int col = gameBoard.Get_Col(sender);
            int row = gameBoard.Get_Row(sender);
            gameBoardArr[row, col] = 1;

            gameBoard.UpDateImages(gameBoardArr);
        }
    }
}
