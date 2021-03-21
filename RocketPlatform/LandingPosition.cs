using System.Collections.Generic;

namespace RocketPlatform {
    public class LandingPosition {

        public int X { get; }
        public int Y { get; }
        public bool HasBeenChecked { get; private set; }

        public LandingPosition(int x, int y) {
            Y = y;
            X = x;
        }

        public void Reserve() {
            HasBeenChecked = true;
        }

        public bool IsLocatedOn(int x, int y) {
            return X == x && Y == y;
        }

        public List<LandingPosition> GetNeighbours() {
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