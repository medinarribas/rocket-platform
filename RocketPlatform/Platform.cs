using System;
using System.Collections.Concurrent;
using System.Linq;

using System.Threading.Tasks;

namespace RocketPlatform {
    public class Platform {
        private readonly int x;
        private readonly int width;
        private readonly int y;
        private readonly int height;
        private const string OkForLanding = "ok for landing";
        private const string OutOfPlatform = "out of platform";
        private const string Clash = "clash";
        private readonly BlockingCollection<LandingPosition> landingPositions;

        public Platform(int x, int width, int y, int height) {
            this.x = x;
            this.width = width;
            this.y = y;
            this.height = height;
            Validate();
            landingPositions = new BlockingCollection<LandingPosition>();
        }

        private void Validate() {
            if (x < 0) throw new ArgumentException();
            if (width < 0) throw new ArgumentException();
            if (y < 0) throw new ArgumentException();
            if (height < 0) throw new ArgumentException();
        }

        public async Task<string> CanILandOn(Position position) {
            return await Task.FromResult(GetLandingAvailability(position));
        }

        private string GetLandingAvailability(Position position) {
            if (!IsALandingPosition(position)) return OutOfPlatform;
            if (IsReserved(position)) return Clash;
            if (IsNextToReservedPosition(position)) return Clash;
            
            ReserveLandingPosition(position);
            return OkForLanding;
        }

        private bool IsALandingPosition(Position position) {
            return (position.X >= x && position.X <= x + width) && 
                   (position.Y >= y && position.Y <= y + height);
        }

        private bool IsReserved(Position position) {
            return GetReservedLandingPositionAt(position.X, position.Y) != null;
        }

        private bool IsNextToReservedPosition(Position position) {
            var landingPosition = new LandingPosition(position.X, position.Y);
            var neighbours = landingPosition.GetNeightbours();
            foreach (var neighbour in neighbours) {
                var pos = GetReservedLandingPositionAt(neighbour.X, neighbour.Y);
                if (pos == null) continue;
                if (pos.HasBeenChecked) return true;
            }
            return false;
        }

        private LandingPosition GetReservedLandingPositionAt(int x, int y) {
            return landingPositions.FirstOrDefault(landingPosition => landingPosition.IsLocatedOn(x, y));
        }

        private void ReserveLandingPosition(Position position) {
            var landingPosition = NewLandingPosition(position);
            landingPosition.Reserve();
            landingPositions.Add(landingPosition);
        }

        private static LandingPosition NewLandingPosition(Position position) {
            return new LandingPosition(position.X, position.Y);
        }

    }
}