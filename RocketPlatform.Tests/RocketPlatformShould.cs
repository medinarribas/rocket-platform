using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace RocketPlatform.Tests {
    public class RocketPlatformShould {
        private Platform platform;
        private string rocketId;
        private string otherRocketId;
        const byte PlatformWidth = 100;
        const byte PlatformHeight = 100;

        [SetUp]
        public void Setup() {
            var landingArea = new LandingArea(5, 10, 5, 10);
            platform = new Platform(landingArea, PlatformWidth, PlatformHeight);
            rocketId = Guid.NewGuid().ToString();
            otherRocketId = Guid.NewGuid().ToString();
        }

        [TestCase(-1, 10, 5, 10)]
        [TestCase(5, -1, 5, 10)]
        [TestCase(5, 10, -1, 10)]
        [TestCase(5, 10, 5, -1)]
        public void throw_if_landing_area_boundaries_are_negative(int x, int width, int y, int height) {
            var landingArea = new LandingArea(x, width, y, height);
            Action action = () => new Platform(landingArea, PlatformWidth,PlatformHeight);

            action.Should().Throw<ArgumentException>();
        }

        [TestCase(5, 96, 5, 95)]
        [TestCase(5, 95, 5, 96)]
        public void throw_if_landing_area_boundaries_are_bigger_than_limits(int x, int width, int y, int height) {
            var landingArea = new LandingArea(x, width, y, height);
            Action action = () => new Platform(landingArea, PlatformWidth,PlatformHeight);

            action.Should().Throw<ArgumentException>();
        }

        [TestCase(5, 5)]
        [TestCase(5, 15)]
        [TestCase(10, 10)]
        [TestCase(15, 5)]
        [TestCase(15, 15)]
        public async Task response_ok_when_rocket_ask_for_a_position_whithin_the_boundaries(int x, int y) {

            var response = await platform.CanILandOn(new Position(x, y), rocketId);

            response.Should().Be("ok for landing");
        }

        [TestCase(4, 5)]
        [TestCase(5, 4)]
        [TestCase(16, 15)]
        [TestCase(15, 16)]
        public async Task response_out_when_rocket_ask_for_a_position_out_of_the_boundaries(int x, int y) {

            var response = await platform.CanILandOn(new Position(x, y), rocketId);

            response.Should().Be("out of platform");
        }

        [TestCase(5, 5)]
        [TestCase(5, 6)]
        [TestCase(5, 7)]
        [TestCase(6, 5)]
        [TestCase(6, 7)]
        [TestCase(7, 5)]
        [TestCase(7, 6)]
        [TestCase(7, 7)]
        public async Task response_clash_when_position_is_next_to_checked_position(int x, int y) {
            var checkedPosition = new Position(6, 6);
            await platform.CanILandOn(checkedPosition, rocketId);

            var response = await platform.CanILandOn(new Position(x, y), rocketId);

            response.Should().Be("clash");
        }

        [Test]
        public async Task response_ok_when_position_has_been_checked_previously_by_the_same_rocket() {
            var position = new Position(6, 6);
            var request = platform.CanILandOn(position, rocketId);
            var otherRequest = platform.CanILandOn(position, rocketId);

            await Task.WhenAll(new List<Task> { request, otherRequest });

            var responses = new List<string> { request.Result, otherRequest.Result };

            responses.Should().Contain("ok for landing");
            responses.Should().Contain("ok for landing");
        }

        [Test]
        public async Task response_clash_when_position_has_been_checked_previously_by_other_rocket() {
            var position = new Position(6, 6);
            var request = platform.CanILandOn(position, rocketId);
            var otherRequest = platform.CanILandOn(position, otherRocketId);

            await Task.WhenAll(new List<Task> { request, otherRequest });

            var responses = new List<string> { request.Result, otherRequest.Result };

            responses.Should().Contain("ok for landing");
            responses.Should().Contain("clash");
        }

        [Test]
        public async Task response_in_parallel() {
            var request = platform.CanILandOn(new Position(6, 6), rocketId);
            var otherRequest = platform.CanILandOn(new Position(7, 6), otherRocketId);
            var outRequest = platform.CanILandOn(new Position(3, 4), rocketId);

            await Task.WhenAll(new List<Task> { request, otherRequest, outRequest });

            var responses = new List<string> { request.Result, otherRequest.Result, outRequest.Result };

            responses.Should().Contain("ok for landing");
            responses.Should().Contain("clash");
            outRequest.Result.Should().Contain("out of platform");
        }

    }
}