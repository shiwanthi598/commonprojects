using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperApp
{
    public partial class Form1 : Form
    {
        
        private int[,] field;//Matrix to hold the game field
        private Button[,] buttons;//Matrix to hold the UI buttons
        public Form1()
        {
            InitializeComponent();
            
        }
        //Initialize the game field with specified size and no of mines
        private void init(int width, int height, int bombs)
        { 
            field = new int[width, height];
            buttons = new Button[width, height];
            if(bombs>0.8*width*height)
                bombs=(int)(0.8*width*height);
                    
            Random rand = new Random();
            //Bomb placement and counting logic
            while (bombs > 0)
            {
                int x=rand.Next(width);
                int y=rand.Next(height);
                if (field[x, y] == -1)
                    continue;
                field[x, y] = -1;
                for (int dx = -1; dx < 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (x + dx < 0) continue; 
                        if(y+dy<0) continue;
                        if(x + dx >= width) continue;
                        if (y + dy >= height) continue;

                        if (field[x + dx, y + dy] != -1)
                            field[x + dx, y + dy]++;
                    }
                }
                bombs--;
            }
            field[1, 1] = -1;
            field[0, 0] = 1; 
            field[1, 0]=2;
        }
        Random rnd = new Random();
                           

        private void ButtonCilck(object sender, EventArgs e)
        {          
        }       
                
        private void BtnMouseUp(object sender, MouseEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //UI creation for setting up game view
            Label lblRow = new Label();
            lblRow.Left = 400;
            lblRow.Text = "No of Rows";
            Controls.Add(lblRow);
            TextBox txtRows=new TextBox();
            txtRows.Left = 500;
            txtRows.Text = "10";
            Controls.Add(txtRows);
            Label lblCol = new Label();
            lblCol.Left = 600;
            lblCol.Text = "No of Columns";
            Controls.Add(lblCol);
            TextBox txtCol = new TextBox();
            txtCol.Left = 720;
            txtCol.Text="10";   
            Controls.Add(txtCol);
            Label lblNoofMines = new Label();
            lblNoofMines.Left = 820;
            lblNoofMines.Text = "No of Mines";
            Controls.Add(lblNoofMines);
            TextBox txtNoofMines = new TextBox();
            txtNoofMines.Left = 920;
            txtNoofMines.Text = "10";
            Controls.Add(txtNoofMines);
            //size of the grid and no of mines hard codded as 10,10,10
            //later implementation to get values as user inputs from text fields
            init(10, 10, 10);
            //Loop to create buttons representing the game grid
            for (int x = 0; x < field.GetLength(0); x++)
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    //button creation and setup
                    Button b = new Button();
                    b.Font = new Font("Arial", 23);
                    buttons[x, y] = b;
                    b.Left = x * 40;
                    b.Top = y * 40;
                    b.Width = 40;
                    b.Height = 40;
                    b.Text = "";
                    Controls.Add(b);
                    b.Click += B_Click;

                }
            

            
        }

        private void B_Click(object sender, EventArgs e)
        {
            //Get the button triggering the event
           Button b=(Button)sender;
            //Calculate the grid position based on button coordinates
            int x = b.Left / 80;
            int y = b.Top / 80; 
            //check if the button clicked is a mine(-1) or empty(0)
            if (field[x, y] == -1)
                b.Text = "\U0001F4A3";//Display mine image
            else
            {
                //if it is not a mine,proceed to uncover cell
                if (field[x, y] == 0)
                {
                    b.Text = "";//clear button text for empty cell
                    uncover(x,y);//uncover adjacent cells if it is empty
                }
                else b.Text = "" + field[x, y];//Show the no of adjacent mines
            }
            b.Enabled = false;//disable the clicked button to prevent further interaction

        }

        private void uncover(int x, int y)
        { 
            //create a stack to track cells to be uncovered
            Stack<Point> stack = new Stack<Point>();
            stack.Push(new Point(x, y));//start with the clicked cell
            while (stack.Count > 0)
            {
                Point p = stack.Pop();//get the next cell from the stack
                //check if the cell is out of bounds or already uncovered
                if(p.X<0 || p.Y<0) continue;
                if (p.X >= field.GetLength(0) || p.Y >= field.GetLength(1)) continue;
                if (!buttons[p.X, p.Y].Enabled) continue;
                
                buttons[p.X,p.Y].Enabled= false;//disable the button
                if (field[p.X, p.Y] != 0)
                buttons[p.X, p.Y].Text = ""+field[p.X,p.Y];
                //if the cell is empty, continue uncovering its neighbours
                if (field[p.X, p.Y] != 0) continue;
                stack.Push(new Point(p.X-1, p.Y));
                stack.Push(new Point(p.X + 1, p.Y));
                stack.Push(new Point(p.X, p.Y-1));
                stack.Push(new Point(p.X, p.Y+1));
            }
        }
    }
}
