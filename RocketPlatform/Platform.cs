using System;
using System.Drawing;

namespace RocketPlatform {
    public class Platform {
        private const string OkForLanding = "ok for landing";
        private const string OutOfPlatform = "out of platform";

        public string CanILandOn(Position position) {
            if (IsALandingPosition(position)) return OkForLanding;
            return OutOfPlatform;
        }

        private static bool IsALandingPosition(Position position) {
            return (position.X >= 5 && position.X <= 15) && (position.Y >= 5 && position.Y <= 15);
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