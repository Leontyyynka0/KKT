﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
using System.IO;

namespace PacMan
{
    public partial class Window1 : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        DispatcherTimer scoreTimer = new DispatcherTimer();
        private int seconds = 0;
        DispatcherTimer boostTimer = new DispatcherTimer();
        private int boostDuration = 6;

        bool goLeft, goRight, goUp, goDown;
        bool noLeft, noRight, noUp, noDown;

        private float speedX, speedY;

        int speed = 8;

        Rect pacmanHitbox;

        int ghostSpeed = 3;
        int ghostMoveStep = 300;
        int currentGhostStep = 100;
        int Score;


        public Window1()
        {
            InitializeComponent();

            GameSetup();

            StartScoreTimer();

        }
        private void StartScoreTimer()
        {
            scoreTimer.Tick += ScoreTimer_Tick;
            scoreTimer.Interval = TimeSpan.FromSeconds(1);
            scoreTimer.Start();
        }
        private void ScoreTimer_Tick(object sender, EventArgs e)
        {
            seconds++;
            txtTime.Text = "Time: " + seconds;
        }
        private void StartBoostTimer()
        {
            boostTimer.Tick += BoostTimer_Tick;
            boostTimer.Interval = TimeSpan.FromSeconds(boostDuration);
        }
        private void BoostTimer_Tick(object sender, EventArgs e)
        {
            // Po vypršení časového limitu obnovíme původní rychlost
            speed = 2;
            boostTimer.Stop();
        }
        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            // Zastavte aktuální pohyb
            goLeft = goRight = goUp = goDown = false;

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
                if ((string)x.Tag == "wall")
                {
                    
                    Rect pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);
                    Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (goLeft == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        noLeft = true;
                        goLeft = false;
                    }
                    // check if we are colliding with the wall while moving right if true then stop the pac man movement
                    if (goRight == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        noRight = true;
                        goRight = false;
                    }
                    // check if we are colliding with the wall while moving down if true then stop the pac man movement
                    if (goDown == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown = true;
                        goDown = false;
                    }
                    // check if we are colliding with the wall while moving up if true then stop the pac man movement
                    if (goUp == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp = true;
                        goUp = false;
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
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "boost" && x.Visibility == Visibility.Visible)
                {
                    if (pacmanHitbox.IntersectsWith(new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height)))
                    {
                        x.Visibility = Visibility.Hidden;
                        speed = 2;
                        boostTimer.Start(); // Spuštění časovače pro časové omezení boostu
                        break;
                    }
                }
            }

            // Detekce kolize s duchy ("ghost") a konec hry
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "ghost" && x.Visibility == Visibility.Visible)
                {
                    if (pacmanHitbox.IntersectsWith(new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height)))
                    {
                        GameOver("ghost killed you");
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

            pacmanHitbox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);

            //pohyb pacmana
            if (goRight)
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
            scoreTimer.Stop();
            MessageBox.Show(message, "pac man wpf");

            string result = $"Time: {seconds} s, Score: {Score}";
            File.AppendAllText("game_result.txt", result + Environment.NewLine);


            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}