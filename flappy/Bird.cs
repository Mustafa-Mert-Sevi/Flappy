using System.Drawing;

namespace flappy
{
    public class Bird
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Velocity { get; set; }
        private int gravity = 2;
        private int jumpStrength = -15;
        private Image birdImage;

        public Bird(int x, int y, int width, int height, Image image = null)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Velocity = 0;
            birdImage = image;
        }

        public void Jump()
        {
            Velocity = jumpStrength;
        }

        public void Update()
        {
            Velocity += gravity;
            Y += Velocity;
        }

        public void Draw(Graphics g)
        {
            if (birdImage != null)
            {
                g.DrawImage(birdImage, X, Y, Width, Height);
            }
            else
            {
                // Default bird drawing
                g.FillEllipse(Brushes.Yellow, X, Y, Width, Height);
                // Eye
                g.FillEllipse(Brushes.Black, X + 10, Y + 10, 5, 5);
                // Beak
                g.FillPolygon(Brushes.Orange, new Point[] {
                    new Point(X + Width, Y + Height / 2),
                    new Point(X + Width + 10, Y + Height / 2 - 5),
                    new Point(X + Width + 10, Y + Height / 2 + 5)
                });
            }
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(X, Y, Width, Height);
        }
    }
}