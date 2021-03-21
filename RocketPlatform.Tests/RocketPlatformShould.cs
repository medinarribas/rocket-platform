using FluentAssertions;
using NUnit.Framework;

namespace RocketPlatform.Tests {
    public class RocketPlatformShould {
        private Platform platform;
        private Position position;

        [SetUp]
        public void Setup() {
            platform = new Platform();
        }

        [TestCase(5,5)]
        [TestCase(5,15)]
        [TestCase(15,5)]
        [TestCase(15,15)]
        public void response_ok_when_rocket_ask_for_a_position_whithin_the_boundaries(int x, int y) {

            var response = platform.CanILandOn(new Position(x, y));

            response.Should().Be("ok for landing");
        }

        [TestCase(4,5)]
        [TestCase(5,4)]
        [TestCase(16,15)]
        [TestCase(15,16)]
        public void response_out_when_rocket_ask_for_a_position_out_of_the_boundaries(int x, int y) {

            var response = platform.CanILandOn(new Position(x, y));
            
            response.Should().Be("out of platform");
        }

        [Test]
        public void response_clash_when_position_has_been_checked() {
            position = new Position(6, 6);
            platform.CanILandOn(position);

            var response = platform.CanILandOn(position);
            
            response.Should().Be("clash");
        }
    }
}