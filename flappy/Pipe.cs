using System.Drawing;

namespace flappy
{
    public class Pipe
    {
        public int X { get; set; }
        public int TopHeight { get; private set; }
        public int Gap { get; private set; }
        public int Width { get; private set; }
        public int Speed { get; set; }
        public bool Scored { get; set; }
        private int formHeight;

        public Pipe(int x, int topHeight, int gap, int width, int speed, int formHeight)
        {
            X = x;
            TopHeight = topHeight;
            Gap = gap;
            Width = width;
            Speed = speed;
            this.formHeight = formHeight;
            Scored = false;
        }

        public void Update()
        {
            X -= Speed;
        }

        public void Draw(Graphics g)
        {
            // Top pipe
            g.FillRectangle(Brushes.Green, X, 0, Width, TopHeight);
            g.DrawRectangle(Pens.DarkGreen, X, 0, Width, TopHeight);

            // Top pipe cap
            g.FillRectangle(Brushes.DarkGreen, X - 5, TopHeight - 20, Width + 10, 20);

            // Bottom pipe
            int bottomY = TopHeight + Gap;
            int bottomHeight = formHeight - bottomY;
            g.FillRectangle(Brushes.Green, X, bottomY, Width, bottomHeight);
            g.DrawRectangle(Pens.DarkGreen, X, bottomY, Width, bottomHeight);

            // Bottom pipe cap
            g.FillRectangle(Brushes.DarkGreen, X - 5, bottomY, Width + 10, 20);
        }

        public Rectangle GetTopBounds()
        {
            return new Rectangle(X, 0, Width, TopHeight);
        }

        public Rectangle GetBottomBounds()
        {
            return new Rectangle(X, TopHeight + Gap, Width, formHeight - (TopHeight + Gap));
        }

        public bool IsOffScreen()
        {
            return X + Width < 0;
        }
    }
}