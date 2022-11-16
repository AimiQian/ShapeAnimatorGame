using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapeAnimatorMidterm
{
    internal class Spaceship
    {
        Rectangle rect = new Rectangle();
        public double X;
        public double Y;
        public int deltaX = 0;
        public int deltaY = -10;

        public void Draw(Canvas canvas)
        {
            X = canvas.ActualWidth / 2 - 10;
            Y = canvas.ActualHeight - 80;
            rect.Stroke = Brushes.Yellow;
            rect.Fill = Brushes.Yellow;
            rect.Width = 20;
            rect.Height = 20;
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, X);
            Canvas.SetTop(rect, Y);
        }

        public void Move(Canvas canvas)
        {
            X += deltaX;
            Y += deltaY;
            Canvas.SetLeft(rect, X);
            Canvas.SetTop(rect, Y);
        }

        public void Left()
        {
            deltaX = -10;
            deltaY = 0;
        }
        public void Right()
        {
            deltaX = +10;
            deltaY = 0;
        }
        public void Up()
        {
            deltaX = 0;
            deltaY = -10;
        }
        public void Down()
        {
            deltaX = 0;
            deltaY = +10;
        }

    }
}
