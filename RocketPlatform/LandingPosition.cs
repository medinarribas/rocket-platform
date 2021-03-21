using System.Collections.Generic;

namespace RocketPlatform {
    public class LandingPosition {

        public int X { get; }
        public int Y { get; }
        public bool IsReserved { get; private set; }

        public LandingPosition(int x, int y) {
            Y = y;
            X = x;
        }

        public void Reserve() {
            IsReserved = true;
        }

        public bool IsLocatedOn(LandingPosition other) {
            return X == other.X && Y == other.Y;
        }

        public List<LandingPosition> GetNeightbours() {
            return new List<LandingPosition>() {
                new LandingPosition(X-1,  Y-1),
                new LandingPosition(X-1,  Y),
                new LandingPosition(X-1,  Y+1),
                new LandingPosition(X,  Y-1),
                new LandingPosition(X,  Y+1),
                new LandingPosition(X+1,  Y-1),
                new LandingPosition(X+1,  Y),
                new LandingPosition(X+1,  Y+1),
            };
        }
    }
}