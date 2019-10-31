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

            int i = 0;
            while (i + 1 < dirToCheckArr.Length)
            {
                curCol = initCol;
                curRow = initRow;

                curCol = curCol + dirToCheckArr[i, 0];
                curRow = curRow + dirToCheckArr[i, 1];
                MessageBox.Show(curCol.ToString() + "" + curRow.ToString());

                if (curRow >= 0 && curCol >= 0 && curRow < 8 && curCol < 8)
                {
                    if (currentPlayer == 0)
                    {
                        if (gameBoardArr[curCol, curRow] == 1)
                        {
                            isAdjToOpp = true;
                        }

                    }
                    else if (currentPlayer == 1)
                    {
                        if (gameBoardArr[curCol, curRow] == 0)
                        {
                            isAdjToOpp = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Big Wrong because there isnt a current player");
                    }
                }

                if (isAdjToOpp == true)
                {



                    bool playerCounterBeforeWhiteSpace = false;
                    List<List<int>> currentOppInDir = new List<List<int>>();

                    while (curCol > 0 && curRow > 0 && curCol < 8 && curRow < 8 && isAdjToOpp == true)
                    {
                        currentOppInDir = new List<List<int>>();
                        playerCounterBeforeWhiteSpace = false;
                        curCol = curCol + dirToCheckArr[i, 0];
                        curRow = curRow + dirToCheckArr[i, 1];
                        int valToCheck = gameBoardArr[curCol, curRow];

                        if (valToCheck != 10)
                        {
                            if (currentPlayer == 0)
                            {
                                if (valToCheck == 1)
                                {
                                    List<int> listToAdd = new List<int> { curRow, curCol };
                                    currentOppInDir.Add(listToAdd);
                                }
                                else if (valToCheck == 0)
                                {
                                    playerCounterBeforeWhiteSpace = true;
                                }
                            }
                            else if (currentPlayer == 1)
                            {
                                if (valToCheck == 0)
                                {
                                    List<int> listToAdd = new List<int> { curRow, curCol };
                                    currentOppInDir.Add(listToAdd);
                                }
                                else if (valToCheck == 1)
                                {
                                    playerCounterBeforeWhiteSpace = true;
                                }
                            }
                        }
                    }

                    if (playerCounterBeforeWhiteSpace == true)
                    {
                        int index = 0;
                        while (index < currentOppInDir.Count)
                        {
                            int colToChange = currentOppInDir[i][0];
                            int rowToChange = currentOppInDir[i][1];
                            if (currentPlayer == 0)
                            {
                                gameBoardArr[colToChange, rowToChange] = 0;
                            }
                            else if (currentPlayer == 1)
                            {
                                gameBoardArr[colToChange, rowToChange] = 1;
                            }
                        }

                        if (currentPlayer == 0)
                        {
                            gameBoardArr[initCol, initRow] = 0;
                        }
                        else if (currentPlayer == 1)
                        {
                            gameBoardArr[initCol, initRow] = 1;
                        }
                    }
                    
                }
                i++;
            }
        }
    }
}
