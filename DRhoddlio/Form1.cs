using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GUIImageArray;
using MyDialogs;

namespace DRhoddlio
{
    public partial class Form1 : Form
    {
        int x = 0;
        int y = 0;
        public int[,] gameBoardArr;
        GImageArray gameBoard;
        string imagesDir = Directory.GetCurrentDirectory() + "\\images\\";

        public Form1()
        {
            InitializeComponent();
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

            gameBoard = new GImageArray(this, gameBoardArr, 150, 0, 7, 7, 5, imagesDir);
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

            if (currentPlayer1.Visible == true)
            {
                gameBoardArr[row, col] = 1;
                currentPlayer1.Visible = false;
                currentPlayer0.Visible = true;
            }
            else
            {
                gameBoardArr[row, col] = 0;
                currentPlayer0.Visible = false;
                currentPlayer1.Visible = true;
            }

            gameBoard.UpDateImages(gameBoardArr);
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string playerBName = "";
            string playerWName = "";

            while (string.IsNullOrEmpty(playerWName) || string.IsNullOrWhiteSpace(playerWName))
            {
                playerWName = My_Dialogs.InputBox("Enter the White player's name");
            }
            while (string.IsNullOrEmpty(playerBName) || string.IsNullOrWhiteSpace(playerBName))
            {
                playerBName = My_Dialogs.InputBox("Enter the Black player's name");
            }
            
            populateGameBoardList();
            bPlayerTxt.Text = playerBName;
            wPlayerTxt.Text = playerWName;
            currentPlayer1.Visible = true;
        }
    }
}
