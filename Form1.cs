using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace InverseCsharp
{
    public partial class Form1 : Form

    {

        private double[,] matrixA;
        private double[,] invertedMatrixA;
        private int matrSize = 0;

        public Form1()
        {



            InitializeComponent();

            InitializeComponent1();
            InitializeComboBox1();
            InitializeComboBox2();
           
            GenerateMatrixTextBoxes(matrSize);

        }






        private void GenerateMatrixTextBoxes(int size)
        {
            // Clear previous text boxes from the panel
            panel.Controls.Clear();

            int counter = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var tb = new TextBox
                    {
                        Name = $"Node_{counter}",
                        Location = new Point(20 + (j * 50), 20 + (i * 30)),
                        Size = new Size(45, 20),
                        Visible = true
                    };
                    tb.TextChanged += TextBox_TextChanged; // Add event handler
                    panel.Controls.Add(tb);
                    counter++;
                }
            }
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                // Check if the text is empty or only contains "-"
                if (string.IsNullOrEmpty(tb.Text) || tb.Text == "-")
                {
                    // Allow empty or "-" input
                    return;
                }

                // Remove leading "-" if present (to handle negative numbers)
                string text = tb.Text;
                if (text.StartsWith("-"))
                {
                    text = text.Substring(1);
                }

                double value;
                if (double.TryParse(text, out value))
                {
                    if (value > 1000 || value < -1000)
                    {
                        MessageBox.Show("Value must be between -1000 and 1000.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tb.Text = ""; // Clear the invalid input
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tb.Text = ""; // Clear the invalid input
                }
            }
        }






        private bool ValidateAndProcessMatrix()
        {
            if (matrSize == 0)
            {
                MessageBox.Show("Please select a matrix dimension.");
                return false;
            }

            matrixA = new double[matrSize, matrSize];

            // Fill matrixA with values from TextBoxes
            for (int i = 0; i < matrSize; i++)
            {
                for (int j = 0; j < matrSize; j++)
                {
                    TextBox tb = (TextBox)panel.Controls.Find($"Node_{i * matrSize + j}", true).FirstOrDefault();
                    if (tb != null && double.TryParse(tb.Text, out double value))
                    {
                        
                        matrixA[i, j] = value;
                    }
                   
                }
            }
            return true;
        }


        private void InitializeComponent1()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }

        private void InitializeComboBox1()
        {
            for (int i = 2; i <= 10; i++)
            {
                comboBox1.Items.Add($"{i}x{i}");
            }
        }

        private void InitializeComboBox2()
        {
            comboBox2.Items.Add("LU Decomposition Method");
            comboBox2.Items.Add("Division Cell Method");
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string selectedMatrix = comboBox1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedMatrix))
            {
                string[] dimensions = selectedMatrix.Split('x');
                matrSize = int.Parse(dimensions[0]);

                GenerateMatrixTextBoxes(matrSize);
            }

            if (matrSize == 0)
            {
                MessageBox.Show("Please select a matrix dimension.");
                return;
            }

            matrixA = new double[matrSize, matrSize];

            // Fill matrixA with values from TextBoxes
            for (int i = 0; i < matrSize; i++)
            {
                for (int j = 0; j < matrSize; j++)
                {
                    TextBox tb = (TextBox)this.Controls.Find($"Node_{i * matrSize + j}", true).FirstOrDefault();
                    if (tb != null && double.TryParse(tb.Text, out double value))
                    {
                        matrixA[i, j] = value;
                    }
                    else
                    {
                       // MessageBox.Show($"Invalid input in cell ({i + 1}, {j + 1}). Please enter a valid number.");
                        return;
                    }
                }
            }

            // Display the matrix in dataGridView1 (if needed)
            DisplayMatrixInTextBoxes(matrixA, panel);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string selectedMatrix = comboBox1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedMatrix))
            {
                string[] dimensions = selectedMatrix.Split('x');
                int rows = int.Parse(dimensions[0]);
                int cols = int.Parse(dimensions[1]);

                matrixA = GenerateRandomMatrix(rows, cols);
                DisplayMatrixInTextBoxes(matrixA, panel);
            }
            else
            {
                MessageBox.Show("Please select a matrix dimension.");
            }
        }

        private double[,] GenerateRandomMatrix(int rows, int cols)
        {
            double[,] matrix = new double[rows, cols];
            Random rand = new Random();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // Generate a random double number between -50 and 50 with two decimal places
                    matrix[i, j] = Math.Round(rand.NextDouble() * 100 - 50, 2);
                }
            }

            return matrix;
        }





        private void DisplayMatrixInTextBoxes(double[,] matrix, Panel panel)
        {
            panel.Controls.Clear();
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var tb = new TextBox
                    {
                        Name = $"Node_{i * cols + j}",
                        Location = new Point(20 + (j * 50), 50 + (i * 30)),
                        Size = new Size(45, 20),
                        Text = matrix[i, j].ToString("0.00")
                    };

                    tb.TextChanged += TextBox_TextChanged;
                    panel.Controls.Add(tb);
                }
            }
        }






        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMethod = comboBox2.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedMethod))
            {
               // MessageBox.Show($"You selected {selectedMethod}.");
            }
        }




        private void button1_Click(object sender, EventArgs e)
        {
            if (matrixA == null)
            {
                MessageBox.Show("Please generate a matrix first.");
                return;
            }

            string selectedMethod = comboBox2.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedMethod))
            {
                MessageBox.Show("Please select an inversion method.");
                return;
            }

            if (ValidateAndProcessMatrix())
            {
                try

                {
                    if (selectedMethod == "LU Decomposition Method")
                    {
                        invertedMatrixA = SolveUsingLU(matrixA);
                    }
                    else if (selectedMethod == "Division Cell Method")
                    {
                        invertedMatrixA = SolveUsingDivision(matrixA);
                    }
                 //  DisplayMatrixInTextBoxes(invertedMatrixA, panel);
                    Form2 inverseForm = new Form2(invertedMatrixA);
                    inverseForm.Show();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inverting matrix: {ex.Message}");
                }
            }
        }
        private double[,] SolveUsingLU(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[,] lower = new double[n, n];
            double[,] upper = new double[n, n];
            int[] perm;
            double[,] result = new double[n, n];

            if (LUDecomposition(matrix, out lower, out upper, out perm))
            {
                result = InvertMatrix(lower, upper, perm);
            }
            else
            {
                throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
            }

            return result;
        }


        private bool LUDecomposition(double[,] matrix, out double[,] lower, out double[,] upper, out int[] perm)
        {
            int n = matrix.GetLength(0);
            lower = new double[n, n];
            upper = new double[n, n];
            perm = new int[n];

            // Initialize permutation array and identity matrix for lower
            for (int i = 0; i < n; i++)
            {
                perm[i] = i;
                lower[i, i] = 1.0; // Identity matrix
            }

            // Create a deep copy of the matrix
            double[,] a = new double[n, n];
            Array.Copy(matrix, a, matrix.Length);

            for (int i = 0; i < n; i++)
            {
                // Partial pivoting
                int maxIndex = i;
                for (int k = i + 1; k < n; k++)
                {
                    if (Math.Abs(a[k, i]) > Math.Abs(a[maxIndex, i]))
                    {
                        maxIndex = k;
                    }
                }

                if (maxIndex != i)
                {
                    // Swap rows in permutation array
                    int temp = perm[i];
                    perm[i] = perm[maxIndex];
                    perm[maxIndex] = temp;

                    // Swap rows in matrix a
                    for (int k = 0; k < n; k++)
                    {
                        double tempValue = a[i, k];
                        a[i, k] = a[maxIndex, k];
                        a[maxIndex, k] = tempValue;
                    }
                }

                // Check for singularity
                if (Math.Abs(a[i, i]) < 1e-10)
                {
                    lower = null;
                    upper = null;
                    perm = null;
                    return false;
                }

                for (int j = i + 1; j < n; j++)
                {
                    a[j, i] /= a[i, i];
                    for (int k = i + 1; k < n; k++)
                    {
                        a[j, k] -= a[j, i] * a[i, k];
                    }
                }
            }

            // Separate the result into lower and upper triangular matrices
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i > j)
                    {
                        lower[i, j] = a[i, j];
                        upper[i, j] = 0;
                    }
                    else if (i == j)
                    {
                        lower[i, j] = 1;
                        upper[i, j] = a[i, j];
                    }
                    else
                    {
                        lower[i, j] = 0;
                        upper[i, j] = a[i, j];
                    }
                }
            }

            return true;
        }

        private double[,] InvertMatrix(double[,] lower, double[,] upper, int[] perm)
        {
            int n = lower.GetLength(0);
            double[,] inv = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                double[] e = new double[n];
                e[i] = 1.0;

                double[] y = ForwardSubstitution(lower, e);
                double[] x = BackwardSubstitution(upper, y);

                for (int j = 0; j < n; j++)
                {
                    inv[j, perm[i]] = x[j];
                }
            }

            return inv;
        }

        private double[] ForwardSubstitution(double[,] lower, double[] b)
        {
            int n = lower.GetLength(0);
            double[] y = new double[n];

            for (int i = 0; i < n; i++)
            {
                y[i] = b[i];
                for (int j = 0; j < i; j++)
                {
                    y[i] -= lower[i, j] * y[j];
                }
            }

            return y;
        }

        private double[] BackwardSubstitution(double[,] upper, double[] y)
        {
            int n = upper.GetLength(0);
            double[] x = new double[n];

            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = y[i];
                for (int j = i + 1; j < n; j++)
                {
                    x[i] -= upper[i, j] * x[j];
                }
                x[i] /= upper[i, i];
            }

            return x;
        }



        private double[,] SolveUsingDivision(double[,] matrix)
        {
            int n = matrix.GetLength(0);

          
            double[,] augmentedMatrix = new double[n, 2 * n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmentedMatrix[i, j] = matrix[i, j];
                }
                augmentedMatrix[i, i + n] = 1.0;
            }

            for (int i = 0; i < n; i++)
            {
               
                double diagElement = augmentedMatrix[i, i];
                if (diagElement == 0)
                {
                    for (int k = i + 1; k < n; k++)
                    {
                        if (augmentedMatrix[k, i] != 0)
                        {
                            for (int j = 0; j < 2 * n; j++)
                            {
                                double temp = augmentedMatrix[i, j];
                                augmentedMatrix[i, j] = augmentedMatrix[k, j];
                                augmentedMatrix[k, j] = temp;
                            }
                            diagElement = augmentedMatrix[i, i];
                            break;
                        }
                    }
                }

                if (diagElement == 0)
                {
                    throw new InvalidOperationException("Matrix is not invertible.");
                }

                for (int j = 0; j < 2 * n; j++)
                {
                    augmentedMatrix[i, j] /= diagElement;
                }

                for (int k = 0; k < n; k++)
                {
                    if (k != i)
                    {
                        double factor = augmentedMatrix[k, i];
                        for (int j = 0; j < 2 * n; j++)
                        {
                            augmentedMatrix[k, j] -= factor * augmentedMatrix[i, j];
                        }
                    }
                }
            }

         
            double[,] result = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = augmentedMatrix[i, j + n];
                }
            }

            return result;
        }

   
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (matrSize == 0)
            {
                MessageBox.Show("Please select a matrix dimension.");
                return;
            }

            matrixA = new double[matrSize, matrSize];

            for (int i = 0; i < matrSize; i++)
            {
                for (int j = 0; j < matrSize; j++)
                {
                    TextBox tb = (TextBox)this.Controls.Find($"Node_{i * matrSize + j}", true).FirstOrDefault();
                    if (tb != null && double.TryParse(tb.Text, out double value))
                    {
                        matrixA[i, j] = value;
                    }
                    else
                    {
                        MessageBox.Show($"Invalid input in cell ({i + 1}, {j + 1}). Please enter a valid number.");
                        return;
                    }
                }
            }

            DisplayMatrixInTextBoxes(matrixA, panel);
        }

        private void buttonSaveMatrix_Click(object sender, EventArgs e)
        {
            
        }

        private void SaveMatrixToFile(double[,] matrix, string filePath)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < rows; i++)
                {
                    string[] row = new string[cols];
                    for (int j = 0; j < cols; j++)
                    {
                        row[j] = matrix[i, j].ToString("0.00");
                    }
                    writer.WriteLine(string.Join(",", row));
                }
            }

            MessageBox.Show($"Matrix successfully saved to {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (invertedMatrixA != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    DefaultExt = "txt",
                    AddExtension = true
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    SaveMatrixToFile(invertedMatrixA, filePath);
                }
            }
            else
            {
                MessageBox.Show("No inverted matrix available to save.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
    }

