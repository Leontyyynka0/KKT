﻿using System;
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
using System.Windows.Threading;

namespace PACMAN_ZP
{
    /// <summary>
    /// Interakční logika pro Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        bool goLeft, goRight, goUp, goDown;
        bool noLeft, noRight, noUp, noDown;

        private float speedX, speedY;

        int speed = 8;

        Rect pacmanHitbox;

        int ghostSpeed = 10;
        int ghostMoveStep = 130;
        int currentGhostStep;
        int Score;


        public Page1()
        {
            InitializeComponent();

            GameSetup();
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            //zobrazení pacmana když se bude otáčet
            if (e.Key == Key.Left && noLeft == false)
            {
                goRight = goUp = goDown = false;
                noRight = noUp = noDown = false;

                goLeft = true;

                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2);
            }

            else if (e.Key == Key.Right && noRight == false)
            {
                noLeft = noUp = noDown = false;
                goLeft = goUp = goDown = false;

                goRight = true;

                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2);
            }

            else if (e.Key == Key.Up && noUp == false)
            {
                noRight = noDown = noLeft = false;
                goRight = goDown = goLeft = false;

                goUp = true;

                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2);
            }

            else if (e.Key == Key.Down && noDown == false)
            {
                noUp = noRight = noLeft = false;
                goUp = goRight = goLeft = false;

                goDown = true;

                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2);

            }
        }
        private void GameSetup()
        {
            MyCanvas.Focus();

            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
            currentGhostStep = ghostMoveStep;

            //zobrazení obrázků pacmana2 a duchů2
            ImageBrush pacmanImage = new ImageBrush();
            pacmanImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Obrázky/Pacman.jpg"));
            pacman.Fill = pacmanImage;

            ImageBrush redGhost = new ImageBrush();
            redGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Obrázky/redG.jpg"));
            red.Fill = redGhost;

            ImageBrush pinkGhost = new ImageBrush();
            pinkGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Obrázky/pinkG.jpg"));
            pink.Fill = pinkGhost;

            ImageBrush blueGhost = new ImageBrush();
            blueGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Obrázky/blueG.jpg"));
            blue.Fill = blueGhost;


        }
        void Collide(string Dir)
        {


            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                // Detekce kolize s bariérama ("wall")
                if ((string)x.Tag == "wall")
                {
                    Rect pacmanHitbox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);
                    Rect wallHitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (pacmanHitbox.IntersectsWith(wallHitbox))
                    {
                        if (goRight && speedX > 0)
                        {
                            Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speedX);
                        }
                        else if (goLeft && speedX < 0)
                        {
                            Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speedX);
                        }
                        else if (goUp && speedY < 0)
                        {
                            Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speedY);
                        }
                        else if (goDown && speedY > 0)
                        {
                            Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speedY);
                        }

                    }
                }
            }

            // Detekce kolize s mincemi ("coin") a zvýšení skóre
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "coin" && x.Visibility == Visibility.Visible)
                {
                    if (pacmanHitbox.IntersectsWith(new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height)))
                    {
                        x.Visibility = Visibility.Hidden;
                        Score++;
                    }
                }
            }

            // Detekce kolize s duchy ("ghost") a konec hry
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "ghost")
                {
                    Rect ghostHitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (pacmanHitbox.IntersectsWith(ghostHitbox))
                    {
                        GameOver("Ghost killed you, click to play again");
                    }

                    if (x.Name == "red")
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                    }
                    else
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + ghostSpeed);
                    }

                    currentGhostStep--;

                    if (currentGhostStep < 1)
                    {
                        currentGhostStep = ghostMoveStep;
                        ghostSpeed = -ghostSpeed;
                    }
                }
            }

            // Konec hry při dosažení určitého skóre
            if (Score == 100)
            {
                GameOver("You win");
            }
        }
        private void GameLoop(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + Score;

            //pohyb pacmana
            /*if (goRight)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed);
            }
            else if (goLeft)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speed);
            }
            else if (goUp)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed);
            }
            else if (goDown)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed);
            }
            */
            //nastavení hranice mapy
            if (goDown && Canvas.GetTop(pacman) + 80 > Application.Current.MainWindow.Height)
            {
                noDown = true;
                goDown = false;
            }
            else if (goUp && Canvas.GetTop(pacman) < 1)
            {
                noUp = true;
                goUp = false;
            }
            else if (goLeft && Canvas.GetLeft(pacman) - 10 < 1)
            {
                noLeft = true;
                goLeft = false;
            }
            else if (goRight && Canvas.GetLeft(pacman) + 60 > Application.Current.MainWindow.Width)
            {
                noRight = true;
                goRight = false;
            }

            Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speedX);
            Collide("x");
            Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speedY);
            Collide("y");

        }




        private void GameOver(string message)
        {
            gameTimer.Stop();
            MessageBox.Show(message, "pac man wpf");

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
