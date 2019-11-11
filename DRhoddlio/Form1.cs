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
        bool boardMade = false;
        public int[,] gameBoardArr;
        GImageArray gameBoard;
        //Setting the directory that the images can be found in for the array and the GUI
        string imagesDir = Directory.GetCurrentDirectory() + "\\images\\";

        public Form1()
        {
            InitializeComponent();
            
        }

        public void populateGameBoardList()
        {
            if (gameBoardArr == null)
            {
                //Crate a new array for the game board
                gameBoardArr = new int[8, 8];
            }
            
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

            gameBoard.UpDateImages(gameBoardArr);
            //Create an event handler for when a cell in the game board is clicked. 
            gameBoard.Which_Element_Clicked += new GImageArray.ImageClickedEventHandler(Which_Element_Clicked);
        }

        private void Which_Element_Clicked(object sender, EventArgs e)
        {
            //Returns the row and column of the clicked cell
            int col = gameBoard.Get_Col(sender);
            int row = gameBoard.Get_Row(sender);

            //Check to see if cell clicked has not been clicked before
            int curVal = gameBoardArr[row, col];

            if (curVal == 10)
            {

                isValidMove(sender);
                calcScore();
            }
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
            if (boardMade == false)
            {
                populateGameBoardList();
                boardMade = true;
            }
            else
            {
                int x = 0;
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

                gameBoard.UpDateImages(gameBoardArr);
            }
            bPlayerTxt.Text = playerBName;
            wPlayerTxt.Text = playerWName;
            currentPlayer1.Visible = true;
            currentPlayer0.Visible = false;
            player0Score.Visible = true;
            player1Score.Visible = true;
        }

        private void isValidMove(object sender)
        {
            //Creates the array with the directions that need to be checked.
            int[,] dirToCheckArr = new int[8, 2] { { 0, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { -1, -1 }, { -1, 0 }, { -1, 1 } };

            //Sets the variables that are going to be used and need to be reset once the function is called. 
            int initCol = gameBoard.Get_Col(sender);
            int initRow = gameBoard.Get_Row(sender);
            int curCol;
            int curRow;
            bool isAdjToOpp = false;
            bool piecesChanged = false;
            List<List<int>> boardPiecesToChange;

            //While loop to check for the oppPiece in each direction 
            int i = 0;
            while (i < dirToCheckArr.Length / 2)
            {
                //Create a new list of boardPiecesToChange as a 2D list of ints in order to add values to later
                boardPiecesToChange = new List<List<int>>();
                //Set the current XY of the cell as the clicked XY
                curCol = initCol;
                curRow = initRow;
                //Set isAdjToOpp to false because if it was left true it would check all the cells in the game from the cell clicked
                isAdjToOpp = false;

                //Make the XY increment in the direction it is going for the check
                curCol = curCol + dirToCheckArr[i, 0];
                curRow = curRow + dirToCheckArr[i, 1];

                //Error handling to ensure the curCol and curRow are not out of bounds of the array
                if (curRow >= 0 && curCol >= 0 && curRow < 8 && curCol < 8)
                {
                    //Gets the current value of the cell that is being checked
                    int currentVal = gameBoardArr[curRow, curCol];

                    //Checks if white or black player is the one who is playing
                    if (currentPlayer1.Visible) //White
                    {
                        //Checks if the opp piece is next to the clicked cell
                        if (currentVal == 0)
                        {
                            isAdjToOpp = true;
                        }
                    }
                    //Same code but for black
                    else //Black
                    {
                        if (currentVal == 1)
                        {
                            isAdjToOpp = true;
                        }
                    }

                    if (isAdjToOpp == true)
                    {
                        bool flag = false;
                        //Now checks all the values in the same direction that there is an adjecet piece to 
                        while (curRow >= 0 && curCol >= 0 && curRow < 8 && curCol < 8 && flag == false)
                        {
                            //Grabs the currentValue of the cell that is being checked
                            currentVal = gameBoardArr[curRow, curCol];
                            //Checks to ensure that the current value is not an empty cell
                            if (currentVal != 10)
                            {
                                if (currentPlayer1.Visible)//White
                                {
                                    //If the current cell is the players value add the row and colum and adds the row and column to the list of pieces to flip
                                    if (currentVal == 0)
                                    {
                                        //Creates a list int that is used to add into the 2D list of all the pieces to flip
                                        List<int> listToAdd = new List<int>();
                                        listToAdd.Add(curRow);
                                        listToAdd.Add(curCol);

                                        boardPiecesToChange.Add(listToAdd);
                                    }
                                    //The piece will be the same as your own colour and therefore flips all the values in the pieces to change arr
                                    else
                                    {
                                        int index = 0;
                                        while (index < boardPiecesToChange.Count)
                                        {
                                            gameBoardArr[boardPiecesToChange[index][0], boardPiecesToChange[index][1]] = 1;
                                            index++;
                                        }
                                        gameBoardArr[initRow, initCol] = 1;
                                        gameBoard.UpDateImages(gameBoardArr);
                                        piecesChanged = true;
                                    }
                                }
                                else //Black
                                {
                                    //As above but for black
                                    if (currentVal == 1)
                                    {
                                        List<int> listToAdd = new List<int>();
                                        listToAdd.Add(curRow);
                                        listToAdd.Add(curCol);

                                        boardPiecesToChange.Add(listToAdd);
                                    }
                                    else
                                    {
                                        int index = 0;
                                        while (index < boardPiecesToChange.Count)
                                        {
                                            gameBoardArr[boardPiecesToChange[index][0], boardPiecesToChange[index][1]] = 0;
                                            index++;
                                        }
                                        gameBoardArr[initRow, initCol] = 0;
                                        gameBoard.UpDateImages(gameBoardArr);
                                        piecesChanged = true;
                                    }
                                }
                            }
                            //Sets the flag to true so that no more cells are checked reducing the amount of CPU time the algorithm takes up
                            else
                            {
                                flag = true;
                            }
                            //Increments the cell that is going to be checked
                            curCol = curCol + dirToCheckArr[i, 0];
                            curRow = curRow + dirToCheckArr[i, 1];
                        }
                    }
                }
                i++;
            }
            //Changes the player turn if pieces have been flipped
            if(piecesChanged == true)
            {
                if (currentPlayer0.Visible == true)
                {
                    currentPlayer0.Visible = false;
                    currentPlayer1.Visible = true;
                }
                else
                {
                    currentPlayer1.Visible = false;
                    currentPlayer0.Visible = true;
                }
            }
        }

        //Calculates the current score for each of the players
        private void calcScore()
        {
            int wScore = 0;
            int bScore = 0;

            int x = 0;
            while (x < 8)
            {
                y = 0;
                while (y < 8)
                {
                    int curVal = gameBoardArr[x, y];
                    if (curVal == 0)
                    {
                        bScore++;
                    }
                    else if (curVal == 1)
                    {
                        wScore++;
                    }
                    y++;
                }
                x++;
            }
            player0Score.Text = "x" + bScore.ToString();
            player1Score.Text = "x" + wScore.ToString();
        }

        //Shows the about form when clicked
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            infoForm newForm = new infoForm();

            newForm.Show();
        }

        //When save game clicked
        private void saveGameToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //Checks if there is a game loaded
            if (gameBoardArr == null)
            {
                MessageBox.Show("you need to have a game to save it");
            }
            else
            {
                //Sets the path for the saveGames
                string path = Directory.GetCurrentDirectory() + "\\saveGames\\";
                //sets name of new file
                path = path + My_Dialogs.InputBox("Game Name:") + ".txt";

                //Creates the file 
                FileStream fs = File.Create(path);
                //Loops through and saves the rows with spaces between the values that would be in the columns and ,'s separating the rows
                int x = 0;
                while (x < 8)
                {
                    string textToWrite = "";
                    int y = 0;
                    while (y < 8)
                    {
                        textToWrite = textToWrite + gameBoardArr[x, y].ToString() + " ";
                        y++;
                    }
                    byte[] bdata = Encoding.Default.GetBytes(textToWrite + ",");
                    fs.Write(bdata, 0, bdata.Length);
                    x++;
                }
                //Adds the player names
                byte[] playerNames = Encoding.Default.GetBytes(wPlayerTxt.Text + "," + bPlayerTxt.Text + ",");
                fs.Write(playerNames, 0, playerNames.Length);

                //Adds the current player value
                if (currentPlayer0.Visible)
                {
                    byte[] bData = Encoding.Default.GetBytes("0");
                    fs.Write(bData, 0, bData.Length);
                }
                else
                {
                    byte[] bData = Encoding.Default.GetBytes("1");
                    fs.Write(bData, 0, bData.Length);
                }

                fs.Close();
            }
        }

        //LoadGame clicked
        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Checks to see if there is already a gameBoard initialised if not creates a new gameBoard and loads or if not just loads
            if (gameBoardArr == null)
            {
                populateGameBoardList();
                loadGame();
            }
            else
            {
                loadGame();
            }
        }

        //Load game function
        private void loadGame()
        {
            //Opens a file Dialog to select the game 
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\saveGames\\";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var sr = new StreamReader(fileDialog.FileName);
                string readString = sr.ReadToEnd();
                string[] readArr = readString.Split(',');

                int x = 0;
                while (x < 8)
                {
                    string[] strToAddArr = readArr[x].Split(' ');
                    int y = 0;

                    while (y < 8)
                    {
                        gameBoardArr[x, y] = int.Parse(strToAddArr[y]);
                        y++;
                    }
                    x++;
                }
                gameBoard.UpDateImages(gameBoardArr);

                wPlayerTxt.Text = readArr[8];
                bPlayerTxt.Text = readArr[9];

                if (readArr[10] == "0")
                {
                    currentPlayer0.Visible = true;
                    currentPlayer1.Visible = false;
                }
                else
                {
                    currentPlayer1.Visible = true;
                    currentPlayer0.Visible = false;
                }
            }
        }

    }
}
