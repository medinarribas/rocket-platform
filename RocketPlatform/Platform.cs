using System;
using System.Collections.Concurrent;
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
            var landingPosition = RegisterLandingPosition(new LandingPosition(position.X, position.Y));
            if (!IsALandingPosition(landingPosition)) return OutOfPlatform;
            if (landingPosition.IsReserved) return Clash;
            if (IsNextToCheckedPosition(position)) return Clash;
        
            landingPosition.Reserve();
            return OkForLanding;
        }

        private bool IsNextToCheckedPosition(Position position) {
            var landingPosition = new LandingPosition(position.X, position.Y);
            var neighbours = landingPosition.GetNeightbours();
            foreach (var neighbour in neighbours) {
                var pos = GetLandingPosition(neighbour);
                if (pos == null) continue;
                if (pos.IsReserved) return true;
            }
            return false;
        }

        private LandingPosition RegisterLandingPosition(LandingPosition position) {
            var pos = GetLandingPosition(position);
            if (pos != null) return pos;

            var newPos = new LandingPosition(position.X, position.Y);
            landingPositions.Add(newPos);
            return newPos;
        }

        private LandingPosition GetLandingPosition(LandingPosition position) {
            return landingPositions.FirstOrDefault(landingPosition => landingPosition.IsLocatedOn(position));
        }

        private static bool IsALandingPosition(LandingPosition position) {
            return (position.X >= 5 && position.X <= 15) && (position.Y >= 5 && position.Y <= 15);
        }
    }
}