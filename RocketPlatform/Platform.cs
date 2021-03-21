using System;
using System.Drawing;

namespace RocketPlatform {
    public class Platform {
        public string CanILandOn(Position position) {
            if (IsALandingPosition(position)) return "ok for landing";
            throw new NotImplementedException();
        }

        private static bool IsALandingPosition(Position position) {
            return (position.X >= 5 && position.Y <= 15) && (position.Y >= 5 && position.Y <= 15);
        }
    }

    public class Position {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y) {
            Y = y;
            X = x;
        }
    }
}