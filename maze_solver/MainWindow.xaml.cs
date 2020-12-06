using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Drawing;

namespace maze_solver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Uri fileUri;
        string ImagePath;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BTN_Solve_Copy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;
                fileUri = new Uri(openFileDialog.FileName);
                IMG_Maze.Source = new BitmapImage(fileUri);
            }
        }

        private void BTN_Solve_Click(object sender, RoutedEventArgs e)
        {
            DateTime startedTime;
            DateTime finalizedTime;

            // Get the file names that will be working on from command line
            string outputPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            // For this project these define the start, end, wall and path colors for the maze
            // Just change these if you want to solve a maze with different requirements
            System.Drawing.Color start = System.Drawing.Color.Red;
            System.Drawing.Color end = System.Drawing.Color.Blue;
            System.Drawing.Color wall = System.Drawing.Color.Black;
            System.Drawing.Color path = System.Drawing.Color.GreenYellow;

            // Load the maze from the first argument filename
            Bitmap mazeImage = new Bitmap(ImagePath);

            // initialize path finder with the image and colors to operate with
            PathFinder pf = new PathFinder(mazeImage, start, end, wall, path);

            // get the solved maze bitmap from pathfinder
            startedTime = DateTime.Now;
            MazeNode resultPath = pf.SolveMaze(COMBO_Type.SelectedValue.ToString());
            finalizedTime = DateTime.Now;
            Lbl_coordinates.Content = "Elapsed Time:" + finalizedTime.Subtract(startedTime).TotalSeconds.ToString() + " seconds. ";
            mazeImage = pf.Maze;
            if (mazeImage == null)
            {
                MessageBox.Show("Could not solve maze");
            }
            else
            {
                LIST_Solution.Items.Clear();
                MazeNode current = resultPath;
                while (current != null)
                {
                    LIST_Solution.Items.Add("("+current.GetX()+", "+ current.GetY()+")");
                    current = current.GetParent();
                }
                Lbl_coordinates.Content = Lbl_coordinates.Content + LIST_Solution.Items.Count.ToString()+ " elements";
                // Save the solved maze image into the output file path
                mazeImage.Save(outputPath + @"\solved_maze.png");
                fileUri = new Uri(outputPath + @"\solved_maze.png");
                IMG_Maze.Source = new BitmapImage(fileUri);
            }
            
        }

        private void IMG_Maze_MouseMove(object sender, MouseEventArgs e)
        {
            /*
             * Lbl_coordinates.Content = "(" + (Mouse.GetPosition(IMG_Maze).X+10).ToString() +
                ", "+ (Mouse.GetPosition(IMG_Maze).Y + 50).ToString() + ")";
            */
        }

        private void BTN_About_Click(object sender, RoutedEventArgs e)
        {
            About winAbout = new About();
            winAbout.Show();
        }
    }
}
