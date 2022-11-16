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

namespace ShapeAnimatorMidterm
{

    public partial class MainWindow : Window
    {
        double x1, y1, x2, y2, x3, y3;
        //bool isFirstClick = true;
        Ellipse tempCirc = new Ellipse();
        Ellipse tempDot = new Ellipse();
        Line tempLine = new Line();
        MediaPlayer playMedia = new MediaPlayer();

        List<Circle> circleArr = new List<Circle>();

        Boolean isMouseDown = false;

        DispatcherTimer timer;
        DispatcherTimer shipTimer;
        Spaceship ship = new Spaceship();
        private void Step_Click(object sender, RoutedEventArgs e)
        {
            GameMessage.Text = "Game Start!";
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            timer.Start();


            shipTimer = new DispatcherTimer();
            shipTimer.Tick += ShipTimer_Tick;
            shipTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            shipTimer.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ShapeCanvas.IsHitTestVisible = false;
            ship.Draw(ShapeCanvas);

        }
        private void ShipTimer_Tick(object sender, EventArgs e)
        {
            ship.Move(ShapeCanvas);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Step();
        }

        //Stop the game. Stop the spaceship and circles moving.
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            shipTimer.Stop();
            GameMessage.Text = "Game Paused";
        }

        //Use Arrow keys to move the spaceship
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.Key.ToString());
            switch (e.Key.ToString())
            {
                case "Left":
                    ship.Left();
                    break;
                case "Right":
                    ship.Right();
                    break;
                case "Up":
                    ship.Up();
                    break;
                case "Down":
                    ship.Down();
                    break;
                default:
                    break;
            }

        }

        //Set the max number of circles added. If exceeds, the user cannot add the circle
        private void MaxCircleBtn_Click(object sender, RoutedEventArgs e)
        {
            ShapeCanvas.IsHitTestVisible = true;
            if (circlemax.Text != "")
            {
                MaxMessage.Text = "Maximum " + circlemax.Text + " circles";
                if (circleArr.Count >= Convert.ToInt32(circlemax.Text))
                {
                    ShapeCanvas.IsHitTestVisible = false;
                }
            }

        }

        //Check if the circle amount is under the maximum number
        private void DrawCircleCheck()
        {
            if (circlemax.Text != "")
            {
                MaxMessage.Text = "Maximum " + circlemax.Text + " circles";
                if (circleArr.Count >= Convert.ToInt32(circlemax.Text))
                {
                    ShapeCanvas.IsHitTestVisible = false;
                }
            }
        }

        //Increase the spaceship speed
        private void SpaceSpeedAdd_Click(object sender, RoutedEventArgs e)
        {
            shipTimer.Tick += ShipTimer_Tick;
        }

        //Restart the Game, clean the screen and put back to the original 
        private void RestartGame_Click(object sender, RoutedEventArgs e)
        {
            ShapeCanvas.Children.Clear();
            Window_Loaded(sender, e);
            GameMessage.Text = "**Note: Please click 'Add Circle' Button and drag circles before each Game";
            circleArr.Clear();
            circlemax.Text = "";
            playMedia.Pause();
            timer.Stop();
            shipTimer.Stop();
        }

        //If the user clicks the 'add cirlce' button, he/she will be able to add circle
        private void AddCircle_Click(object sender, RoutedEventArgs e)
        {
            ShapeCanvas.IsHitTestVisible = true;
        }

        //Decrease the spaceship speed
        private void SpaceSpeedMinus_Click(object sender, RoutedEventArgs e)
        {
            shipTimer.Tick -= ShipTimer_Tick;
        }

        //Bounce the spaceship and circle back from the four walls
        public void Step()
        {
            //Bounce the circle back from the four walls
            foreach (Circle c in circleArr)
            {
                if (c.X + c.Radius > ShapeCanvas.ActualWidth || c.X - c.Radius < 0)
                {
                    c.DeltaX = -c.DeltaX;
                }

                if (c.Y + c.Radius > ShapeCanvas.ActualHeight || c.Y - c.Radius < 0)
                {
                    c.DeltaY = -c.DeltaY;
                }

                c.Shift();
            }

            //Bounce the spaceship back from the four walls
            if (ship.X + 20 >= ShapeCanvas.ActualWidth)
            {
                ship.Left();
            }
            else if(ship.X <= 0){
                ship.Right();
            }

            if (ship.Y + 20 >= ShapeCanvas.ActualHeight)
            {
                ship.Up();
            }
            else if (ship.Y <= 0)
            {
                ship.Down();
            }

            //Explode the spaceship when the spaceship hits the circles
            foreach (Circle c in circleArr)
            {
                bool Xcross = ship.X < c.X + c.Radius && ship.X + 20 > c.X - c.Radius;
                bool Ycross = ship.Y < c.Y + c.Radius && ship.Y + 20 > c.Y - c.Radius;
                if (Xcross && Ycross)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + @"../../../SOUND/explodesound.wav";
                    Uri uri = new Uri(path);
                    playMedia.Open(uri);
                    playMedia.Play();

                    for (int i = 0; i < ShapeCanvas.Children.Count; i++)
                    {
                        UIElement child = ShapeCanvas.Children[i];
                        if (child is Rectangle)
                        {
                            ShapeCanvas.Children.Remove(child);
                            timer.Stop();
                            shipTimer.Stop();
                            GameMessage.Text = "Game is Over! You Lose!";
                        }
                    }
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShapeCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DrawCircleCheck();
            // Save the center point coords
            x1 = e.GetPosition(this).X;
            y1 = e.GetPosition(this).Y;

            tempDot.Width = 4;
            tempDot.Height = 4;
            tempDot.Stroke = Brushes.Red;
            tempDot.Fill = Brushes.Red;

            // Add temp geometries on to the canvas as visual cues
            ShapeCanvas.Children.Add(tempDot);
            ShapeCanvas.Children.Add(tempCirc);
            ShapeCanvas.Children.Add(tempLine);

            Canvas.SetLeft(tempDot, x1 - 2);
            Canvas.SetTop(tempDot, y1 - 2);

            isMouseDown = true;
        }
        private void ShapeCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DrawCircleCheck();
            // Capture the rim point coords
            x2 = e.GetPosition(this).X;
            y2 = e.GetPosition(this).Y;

            // Draw the circ
            double radius = Math.Sqrt(
                    (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

            Circle c = new Circle(x1, y1, radius);
            c.Draw(ShapeCanvas);
            circleArr.Add(c);

            // Remove all the temp goemetries
            ShapeCanvas.Children.Remove(tempDot);
            ShapeCanvas.Children.Remove(tempCirc);
            ShapeCanvas.Children.Remove(tempLine);

            isMouseDown = false;
        }

        private void ShapeCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            DrawCircleCheck();
            if (!isMouseDown)
            {
                return;
            }

            // Capture the coords of current point position
            x3 = e.GetPosition(this).X;
            y3 = e.GetPosition(this).Y;

            // Adjust the tempCirc size
            double radius = Math.Sqrt(
                    (x3 - x1) * (x3 - x1)
                    + (y3 - y1) * (y3 - y1));
            tempCirc.Width = radius * 2;
            tempCirc.Height = radius * 2;
            tempCirc.Stroke = Brushes.Red;
            Canvas.SetLeft(tempCirc, x1 - radius);
            Canvas.SetTop(tempCirc, y1 - radius);

            tempLine.Stroke = Brushes.Red;
            tempLine.X1 = x1;
            tempLine.Y1 = y1;
            tempLine.X2 = x3;
            tempLine.Y2 = y3;
        }
    }
}
