namespace RocketPlatform {
    public class LandingArea {
        public int X { get; }
        public int Width { get; }
        public int Y { get; }
        public int Height { get; }
        public int MaxPositionY => Y + Height;
        public int MaxPositionX => X + Width;

        public LandingArea(int x, int width, int y, int height) {
            X = x;
            Width = width;
            Y = y;
            Height = height;
        }

    }
}