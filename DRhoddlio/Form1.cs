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
        //assigning variables 
        int x = 0;
        int y = 0;
        public int[,] gameBoardArr;
        GImageArray gameBoard;
        //Setting the directory that the images can be found in for the array and the GUI
        string imagesDir = Directory.GetCurrentDirectory() + "\\images\\";

        public Form1()
        {
            InitializeComponent();
            //Set the images in the GUI
            pictureBox2.Image = Image.FromFile(imagesDir + "0.png");
            pictureBox3.Image = Image.FromFile(imagesDir + "1.png");
        }

        public void populateGameBoardList()
        {
            //Crate a new array for the game board
            gameBoardArr = new int[8, 8];
            //Loop round all the rows and columnds setting the values to 10 for the default picture
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
            //Sets the values for the four middle counters in the field.
            gameBoardArr[3, 3] = 1;
            gameBoardArr[3, 4] = 0;
            gameBoardArr[4, 4] = 1;
            gameBoardArr[4, 3] = 0;

            //Loads the gameBoard array and creates a GImageArray object under the variable of gameBoard
            gameBoard = new GImageArray(this, gameBoardArr, 150, 0, 7, 7, 5, imagesDir);
            //Create an event handler for when a cell in the game board is clicked. 
            gameBoard.Which_Element_Clicked += new GImageArray.ImageClickedEventHandler(Which_Element_Clicked);
        }

        private void Which_Element_Clicked(object sender, EventArgs e)
        {
            //Returns the row and column of the clicked cell
            int col = gameBoard.Get_Col(sender);
            int row = gameBoard.Get_Row(sender);

            MessageBox.Show(col.ToString() + row.ToString());
            //Determines current player based on the label for the current player, sets the appropriate value and then changes to the other player's turn
            if (currentPlayer1.Visible == true)
            {
                isValidMove(sender, 1);
                currentPlayer1.Visible = false;
                currentPlayer0.Visible = true;
            }
            else
            {

                isValidMove(sender, 0);
                currentPlayer0.Visible = false;
                currentPlayer1.Visible = true;
            }
            //Updates the images in the board with the new values. 
            gameBoard.UpDateImages(gameBoardArr);
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Create the strings for the player names
            string playerBName = "";
            string playerWName = "";

            //Make sure the users have input a player name for both players
            while (string.IsNullOrEmpty(playerWName) || string.IsNullOrWhiteSpace(playerWName))
            {
                playerWName = My_Dialogs.InputBox("Enter the White player's name");
            }
            while (string.IsNullOrEmpty(playerBName) || string.IsNullOrWhiteSpace(playerBName))
            {
                playerBName = My_Dialogs.InputBox("Enter the Black player's name");
            }
            
            //runs the command to populate the game board and then sets the player names to be the appropriate players before indicating that white starts the game. 
            populateGameBoardList();
            bPlayerTxt.Text = playerBName;
            wPlayerTxt.Text = playerWName;
            currentPlayer1.Visible = true;
        }

        private void isValidMove(object sender, int currentPlayer)
        {
            int[,] dirToCheckArr = new int[8, 2] { { 0, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { -1, -1 }, { -1, 0 }, { -1, 1 } };

            int initCol = gameBoard.Get_Col(sender);
            int initRow = gameBoard.Get_Row(sender);
            int curCol;
            int curRow;
            bool isAdjToOpp = false;
            List<List<int>> boardPiecesToChange;


            int i = 0;
            while (i < dirToCheckArr.Length / 2)
            {
                boardPiecesToChange = new List<List<int>>();
                curCol = initCol;
                curRow = initRow;
                isAdjToOpp = false;

                curCol = curCol + dirToCheckArr[i, 0];
                curRow = curRow + dirToCheckArr[i, 1];

                int currentVal = gameBoardArr[curRow, curCol];

                if (currentPlayer1.Visible) //White
                {
                    if (currentVal == 0)
                    {
                        isAdjToOpp = true;
                    }
                }
                else if (currentPlayer0.Visible) //Black
                {
                    if (currentVal == 1)
                    {
                        isAdjToOpp = true;
                    }
                }

                if (isAdjToOpp == true)
                {
                    while (curRow >= 0 && curCol >= 0 && curRow < 8 && curCol < 8)
                    {
                        List<int> listToAdd = new List<int>();
                        currentVal = gameBoardArr[curRow, curCol];
                        if (currentVal != 10)
                        {
                            if (currentPlayer1.Visible)
                            {

                                if (currentVal == 0)
                                {
                                    listToAdd.Add(curRow);
                                    listToAdd.Add(curCol);

                                    boardPiecesToChange.Add(listToAdd);
                                }
                                if (currentVal == 1)
                                {
                                    int index = 0;
                                    while (index < boardPiecesToChange.Count)
                                    {
                                        gameBoardArr[boardPiecesToChange[index][0], boardPiecesToChange[index][1]] = 1;
                                        index++;
                                    }
                                    gameBoardArr[initRow, initCol] = 1;
                                    gameBoard.UpDateImages(gameBoardArr);
                                }
                            }
                            else if (currentPlayer0.Visible)
                            {

                                if (currentVal == 1)
                                {
                                    listToAdd.Add(curRow);
                                    listToAdd.Add(curCol);

                                    boardPiecesToChange.Add(listToAdd);
                                }
                                if (currentVal == 0)
                                {
                                    int index = 0;
                                    while (index < boardPiecesToChange.Count)
                                    {
                                        gameBoardArr[boardPiecesToChange[index][1], boardPiecesToChange[index][0]] = 0;
                                        index++;
                                    }
                                    gameBoardArr[initRow, initCol] = 0;
                                    gameBoard.UpDateImages(gameBoardArr);
                                }
                            }
                        }
                        MessageBox.Show(curCol.ToString() + curRow.ToString());
                        curCol = curCol + dirToCheckArr[i, 0];
                        curRow = curRow + dirToCheckArr[i, 1];
                        MessageBox.Show(curCol.ToString() + curRow.ToString());
                    }
                }
                i++;
            }
        }
    }
}
