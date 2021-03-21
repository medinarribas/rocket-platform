using System;

namespace RocketPlatform {
    public class Platform {
        public string CanILandOn(int posx, int posy) {
            if ((posx >= 5 && posx <= 15) && (posy >= 5 && posy <= 15)) return "ok for landing";
            throw new NotImplementedException();
        }
    }
}