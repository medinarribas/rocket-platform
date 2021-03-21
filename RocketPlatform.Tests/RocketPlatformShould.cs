using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace RocketPlatform.Tests {
    public class RocketPlatformShould {
        private Platform platform;

        [SetUp]
        public void Setup() {
            platform = new Platform(5, 10, 5, 10);
        }

        [TestCase(5,5)]
        [TestCase(5,15)]
        [TestCase(10,10)]
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
            var position = new Position(6, 6);
            await platform.CanILandOn(position);

            var response =await platform.CanILandOn(position);
            
            response.Should().Be("clash");
        }

        [TestCase(5,5)]
        [TestCase(5,6)]
        [TestCase(5,7)]
        [TestCase(6,5)]
        [TestCase(6,7)]
        [TestCase(7,5)]
        [TestCase(7,6)]
        [TestCase(7,7)]
        public async Task response_clash_when_position_is_next_to_checked_position(int x, int y) {
            var checkedPosition = new Position(6, 6);
            await platform.CanILandOn(checkedPosition);

            var response =await platform.CanILandOn(new Position(x, y));
            
            response.Should().Be("clash");
        }

        [Test]
        public async Task response_clash_when_position_has_been_checked_previously() {
            var position = new Position(6, 6);
            var request = platform.CanILandOn(position);
            var otherRequest = platform.CanILandOn(position);

            await Task.WhenAll(new List<Task> {request, otherRequest});

            var responses = new List<string> { request.Result, otherRequest.Result};

            responses.Should().Contain("ok for landing");
            responses.Should().Contain("clash");
        }

        [Test]
        public async Task response_in_parallel() {
            var request = platform.CanILandOn(new Position(6, 6));
            var clashRequest = platform.CanILandOn(new Position(7, 6));
            var outRequest = platform.CanILandOn(new Position(3,4));

            await Task.WhenAll(new List<Task> {request, clashRequest, outRequest });

            request.Result.Should().Contain("ok for landing");
            clashRequest.Result.Should().Contain("clash");
            outRequest.Result.Should().Contain("out of platform");
        }

    }
}