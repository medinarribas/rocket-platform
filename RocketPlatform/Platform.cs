using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using System.Threading.Tasks;

namespace RocketPlatform {
    public class Platform {
        private const string OkForLanding = "ok for landing";
        private const string OutOfPlatform = "out of platform";
        private const string Clash = "clash";
        private readonly BlockingCollection<LandingPosition> landingPositions;

        public Platform() {
            landingPositions = new BlockingCollection<LandingPosition>();
        }

        public async Task<string> CanILandOn(Position position) {
            return await Task.FromResult(GetLandingAvailability(position));
        }

        public string GetLandingAvailability(Position position) {
            var landingPosition = RegisterLandingPosition(position);
            if (!IsALandingPosition(landingPosition)) return  OutOfPlatform;
            if (landingPosition.IsReserved) return Clash;
        
            landingPosition.Reserve();
            return OkForLanding;
        }

        private LandingPosition RegisterLandingPosition(Position position) {
            var pos = landingPositions.FirstOrDefault(pos => pos.X == position.X && pos.Y == position.Y);
            if (pos != null) return pos;

            var newPos = new LandingPosition(position);
            landingPositions.Add(newPos);
            return newPos;
        }

        private static bool IsALandingPosition(LandingPosition position) {
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

    public class LandingPosition {
        public int X { get; }
        public int Y { get; }
        public bool IsReserved { get; private set; }

        public LandingPosition(Position position) {
            Y = position.Y;
            X = position.X;
        }

        public void Reserve() {
            IsReserved = true;
        }
    }
}