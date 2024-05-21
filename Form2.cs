using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InverseCsharp
{
    public partial class Form2 : Form
    {
        private double[,] invertedMatrixA;
        public Form2(double[,] matrix)
        {
            InitializeComponent();

            DisplayMatrixInTextBoxes(matrix, panel1);
        }

        private void DisplayMatrixInTextBoxes(double[,] matrix, Panel panel)
        {
            // Clear previous controls in the panel
            panel1.Controls.Clear();

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            int counter = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var tb = new TextBox
                    {
                        Name = $"MatrixNode_{counter}",
                        Text = matrix[i, j].ToString("0.00"),
                        Location = new Point(20 + (j * 50), 10 + (i * 30)), // Adjust position as needed
                        Size = new Size(45, 20),
                        Visible = true,
                        ReadOnly = true
                    };
                    panel1.Controls.Add(tb);
                    counter++;
                }
            }
        }



        private void Form2_Load(object sender, EventArgs e)
        {

           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


       

        

    }
}

