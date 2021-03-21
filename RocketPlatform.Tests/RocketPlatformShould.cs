using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task response_ok_when_rocket_ask_for_a_position_whithin_the_boundaries(int x, int y) {

            var response = await platform.CanILandOn(new Position(x, y));

            response.Should().Be("ok for landing");
        }

        [TestCase(4,5)]
        [TestCase(5,4)]
        [TestCase(16,15)]
        [TestCase(15,16)]
        public async Task response_out_when_rocket_ask_for_a_position_out_of_the_boundaries(int x, int y) {

            var response = await platform.CanILandOn(new Position(x, y));
            
            response.Should().Be("out of platform");
        }

        [Test]
        public async Task response_clash_when_position_has_been_checked() {
            position = new Position(6, 6);
            await platform.CanILandOn(position);

            var response =await platform.CanILandOn(position);
            
            response.Should().Be("clash");
        }

        [Test]
        public async Task response_clash_when_position_has_been_checked_by_other_rocket() {
            position = new Position(6, 6);
            var request = platform.CanILandOn(position);
            var otherRequest = platform.CanILandOn(position);

            await Task.WhenAll(new List<Task> {request, otherRequest});

            var responses = new List<string> { request.Result, otherRequest.Result};

            responses.Should().Contain("ok for landing");
            responses.Should().Contain("clash");
        }
    }
}