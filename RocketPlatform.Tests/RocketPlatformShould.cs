using FluentAssertions;
using NUnit.Framework;

namespace RocketPlatform.Tests {
    public class RocketPlatformShould {

        [Test]
        public void response_ok_when_rocket_ask_for_a_position_whithin_the_boundaries() {

            var platform = new Platform();

            var response = platform.CanILandOn(new Position(5, 5));

            response.Should().Be("ok for landing");
        }
    }
}