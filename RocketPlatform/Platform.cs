using System;
using System.Collections.Concurrent;
using System.Linq;

using System.Threading.Tasks;

namespace RocketPlatform {
    public class Platform {
        private readonly LandingArea landingArea;
        private readonly byte width;
        private readonly byte height;
        private const string OkForLanding = "ok for landing";
        private const string OutOfPlatform = "out of platform";
        private const string Clash = "clash";
        private readonly BlockingCollection<LandingPosition> landingPositions;

        public Platform(LandingArea landingArea, byte width, byte height) {
            this.landingArea = landingArea;
            this.width = width;
            this.height = height;
            Validate();
            landingPositions = new BlockingCollection<LandingPosition>();
        }

        public async Task<string> CanILandOn(Position position, string rocketId) {
            return await Task.FromResult(GetLandingAvailabilityFor(position, rocketId));
        }

        private void Validate() {
            if (landingArea.X < 0) throw new ArgumentException();
            if (landingArea.Width < 0) throw new ArgumentException();
            if (landingArea.Y < 0) throw new ArgumentException();
            if (landingArea.Height < 0) throw new ArgumentException();
            if (landingArea.MaxPositionX > width) throw new ArgumentException();
            if (landingArea.MaxPositionY > height) throw new ArgumentException();
        }

        private string GetLandingAvailabilityFor(Position position, string rocketId) {
            if (!IsALandingPosition(position)) return OutOfPlatform;
            if (IsReserved(position, rocketId)) return Clash;
            if (IsNextToReservedPosition(position)) return Clash;
            
            ReserveLandingPosition(position, rocketId);
            return OkForLanding;
        }

        private bool IsALandingPosition(Position position) {
            return (position.X >= landingArea.X && position.X <= landingArea.X + landingArea.Width) && 
                   (position.Y >= landingArea.Y && position.Y <= landingArea.Y + landingArea.Height);
        }

        private bool IsReserved(Position position, string rocketId) {
            var reservedPosition = GetReservedLandingPositionAt(position.X, position.Y);
            if (reservedPosition == null) return false;
            if (reservedPosition.IsReservedBy(rocketId)) return false;
            return true;
        }

        private bool IsNextToReservedPosition(Position position) {
            var landingPosition = new LandingPosition(position.X, position.Y);
            var neighbours = landingPosition.GetNeighbours();
            foreach (var neighbour in neighbours) {
                var reservedPosition = GetReservedLandingPositionAt(neighbour.X, neighbour.Y);
                if (reservedPosition != null) return true;
            }
            return false;
        }

        private LandingPosition GetReservedLandingPositionAt(int x, int y) {
            return landingPositions.FirstOrDefault(landingPosition => landingPosition.IsLocatedOn(x, y));
        }

        private void ReserveLandingPosition(Position position, string rocketId) {
            var landingPosition = NewLandingPosition(position);
            landingPosition.ReserveTo(rocketId);
            landingPositions.Add(landingPosition);
        }

        private static LandingPosition NewLandingPosition(Position position) {
            return new LandingPosition(position.X, position.Y);
        }

    }
}