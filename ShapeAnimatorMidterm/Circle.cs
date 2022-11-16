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
    internal class Circle
    {
        private Ellipse ell;

        public double X, Y; // Center coords
        public double Radius;
        public int DeltaX = 1;
        public int DeltaY = 1;

        public double Width
        {
            get => Radius * 2;
            set { }
        }

        public Circle(double x, double y, double r)
        {
            X = x;
            Y = y;
            Radius = r;

            ell = new Ellipse();
        }

        public void Draw(Canvas canvas)
        {
            ell.Width = Radius * 2;
            ell.Height = Radius * 2;
            ell.Stroke = Brushes.Orange;
            ell.StrokeThickness = 1;

            canvas.Children.Add(ell); // What happens when the same ellipse is added multiple times????

            Canvas.SetLeft(ell, X - Radius);
            Canvas.SetTop(ell, Y - Radius);
        }

        public void Shift()
        {
            X += DeltaX;
            Y += DeltaY;
            Canvas.SetLeft(ell, X - Radius);
            Canvas.SetTop(ell, Y - Radius);
        }
    }
}
