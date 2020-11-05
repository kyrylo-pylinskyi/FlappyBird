using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Flappy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Game Timer
        DispatcherTimer timer = new DispatcherTimer();
        // Gravity
        int gravity = 8;
        // Game score
        int score;
        // GameOver check
        bool gameEnd = false;
        // Random
        Random random = new Random();
        
        /// <summary>
        /// Display the main game window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // set the timer
            timer.Interval = TimeSpan.FromMilliseconds(20);
            // every timer tick call the gameplay method
            timer.Tick += GamePlay;
            // start the game
            GameStart();
        }

        /// <summary>
        /// Method to start game
        /// </summary>
        private void GameStart()
        {
            // Set Pipe 1 to their positions
            int pipeRandom1 = random.Next(-250, 0);
            // set the vertical position
            Canvas.SetLeft(PipeOne, 200);
            // set the horizontal position
            Canvas.SetTop(PipeOne, pipeRandom1);

            // Set Pipe 1 to their positions
            int pipeRandom2 = random.Next(-250, 0);
            // set the vertical position
            Canvas.SetLeft(PipeTwo, 400);
            // set the horizontal position
            Canvas.SetTop(PipeTwo, pipeRandom2);

            // set the flappy bird to their position
            Canvas.SetTop(FlappyBird, 158);

            // start the timer
            timer.Start();
        }

        private void GamePlay(object sender, EventArgs e)
        {
            // make the flappy bird falling
            Canvas.SetTop(FlappyBird, Canvas.GetTop(FlappyBird) + gravity);

            // for each pipe move to the left side of screen 
            foreach(var x in MyCanvas.Children.OfType<Image>())
            {
                if((string)x.Tag == "Pipe")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);
                }
            }
            
            // array with pipes
            Image[] pipes = { PipeOne, PipeTwo };

            // check every pipe
            for(int i = 0; i < pipes.Length; i++)
            {
                // spawn new pipes
                if (Canvas.GetLeft(pipes[i]) <= -52)
                {
                    SpawnPipes(pipes[i]);
                    // score + 1
                    score++;
                }
            }

            // update score
            gameScore.Content = score;

            // gameover if collision check == true
            if (CollisionCheck())
            {
                GameOver();
            }
        }

        /// <summary>
        /// This method spawn new pipes
        /// </summary>
        /// <param name="pipeName">Pipe Number</param>
        private void SpawnPipes(Image pipeName)
        {
            int pipeRandom = random.Next(-250, 0);
            Canvas.SetLeft(pipeName, 348);
            Canvas.SetTop(pipeName, pipeRandom);
        }

        /// <summary>
        /// Method to stop the game
        /// </summary>
        private void GameOver()
        {
            gameEnd = true;
            timer.Stop();
        }

        /// <summary>
        /// Method to check the collision
        /// </summary>
        /// <returns></returns>
        private bool CollisionCheck()
        {
            double birdHeight = 24;
            double birdWidth = 32;
            double birdX1 = Canvas.GetLeft(FlappyBird);
            double birdY1 = Canvas.GetTop(FlappyBird);
            double birdX2 = birdX1 + birdHeight;
            double birdY2 = birdY1 + birdWidth;

            if (// Intersect with pipe 1
                birdX2 > GetPipeXY(PipeOne).Item3 && birdX1 < GetPipeXY(PipeOne).Item4 && birdY1 < GetPipeXY(PipeOne).Item1 ||
                birdX2 > GetPipeXY(PipeOne).Item3 && birdX1 < GetPipeXY(PipeOne).Item4 && birdY2 > GetPipeXY(PipeOne).Item2 ||
                // Intersect with pipe 2
                birdX2 > GetPipeXY(PipeTwo).Item3 && birdX1 < GetPipeXY(PipeTwo).Item4 && birdY1 < GetPipeXY(PipeTwo).Item1 ||
                birdX2 > GetPipeXY(PipeTwo).Item3 && birdX1 < GetPipeXY(PipeTwo).Item4 && birdY2 > GetPipeXY(PipeTwo).Item2)
            {
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// Method returns the tuple with pipe position
        /// </summary>
        /// <param name="pipe">The pipe</param>
        private static (double, double, double, double) GetPipeXY(Image pipe)
        {

            var tuple = (Canvas.GetTop(pipe) + 320,
                         Canvas.GetTop(pipe) + 450,
                         Canvas.GetLeft(pipe),
                         Canvas.GetLeft(pipe) + 52);

            var result = (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
            return result;
        }
        
        /// <summary>
        /// Method changes the gravity when SPACE pressed
        /// </summary>
        private void GameKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                gravity = -8;
            }
            if (e.Key == Key.Space && gameEnd == true)
            {
                score = 0;
                gameEnd = false;
                GameStart();
            }
        }

        /// <summary>
        /// Method changes the gravity to normal when SPACE is not pressed
        /// </summary>
        private void GameKeyUp(object sender, KeyEventArgs e)
        {
            gravity = 8;
        }
    }
}
