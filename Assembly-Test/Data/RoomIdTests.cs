using BlindWizard.Data;
using NUnit.Framework;

namespace BlindWizard.Test.Data
{
    [TestFixture]
    public class RoomIdTests
    {
        private const Direction North = Direction.North;
        private const Direction South = Direction.South;
        private const Direction East = Direction.East;
        private const Direction West = Direction.West;
        
        // No diff case: North bias
        [TestCase(0, 0, 0, 0, North)]
        // One step cases
        [TestCase(1, 1, 1, 2, North)]
        [TestCase(1, 1, 1, 0, South)]
        [TestCase(1, 1, 2, 1, East)]
        [TestCase(1, 1, 0, 1, West)]
        // Multi step caases
        [TestCase(4, 4, 4, 8, North)]
        [TestCase(4, 4, 4, 0, South)]
        [TestCase(4, 4, 8, 4, East)]
        [TestCase(4, 4, 0, 4, West)]
        // One diagonal step caases: Z bias
        [TestCase(1, 1, 2, 2, North)]
        [TestCase(1, 1, 0, 0, South)]
        [TestCase(1, 1, 2, 0, South)]
        [TestCase(1, 1, 0, 2, North)]
        // Multi diagonal step caases: Z bias
        [TestCase(4, 4, 8, 8, North)]
        [TestCase(4, 4, 0, 0, South)]
        [TestCase(4, 4, 8, 0, South)]
        [TestCase(4, 4, 0, 8, North)]
        // Near diagonal step cases, south going clockwise
        [TestCase(2, 2, 3, 0, South)]
        [TestCase(2, 2, 1, 0, South)]
        [TestCase(2, 2, 0, 1, West)]
        [TestCase(2, 2, 0, 3, West)]
        [TestCase(2, 2, 1, 4, North)]
        [TestCase(2, 2, 3, 4, North)]
        [TestCase(2, 2, 4, 3, East)]
        [TestCase(2, 2, 4, 1, East)]
        public void OrientTest(int fromX, int fromZ, int toX, int toZ, Direction direction)
        {
            var from = new RoomId(fromX, fromZ);
            var to = new RoomId(toX, toZ);
            var orient = from.Orient(to);
            Assert.AreEqual(direction, orient, $"{orient} expected {direction}");
        }
        
        // No diff case: NEWS bias
        [TestCase(0, 0, 0, 0, new [] {North, East,  West,  South})]
        // One step cases
        [TestCase(1, 1, 1, 2, new [] {North, East,  West,  South})]
        [TestCase(1, 1, 1, 0, new [] {South, East,  West,  North})]
        [TestCase(1, 1, 2, 1, new [] {East,  North, South, West })]
        [TestCase(1, 1, 0, 1, new [] {West,  North, South, East })]
        // Multi step caases
        [TestCase(4, 4, 4, 8, new [] {North, East,  West,  South})]
        [TestCase(4, 4, 4, 0, new [] {South, East,  West,  North})]
        [TestCase(4, 4, 8, 4, new [] {East,  North, South, West })]
        [TestCase(4, 4, 0, 4, new [] {West,  North, South, East })]
        // One diagonal step caases: Z bias
        [TestCase(1, 1, 2, 2, new [] {North, East,  West,  South})]
        [TestCase(1, 1, 0, 0, new [] {South, West,  East,  North})]
        [TestCase(1, 1, 2, 0, new [] {South, East,  West,  North})]
        [TestCase(1, 1, 0, 2, new [] {North, West,  East,  South})]
        // Multi diagonal step caases: Z bias
        [TestCase(4, 4, 8, 8, new [] {North, East,  West,  South})]
        [TestCase(4, 4, 0, 0, new [] {South, West,  East,  North})]
        [TestCase(4, 4, 8, 0, new [] {South, East,  West,  North})]
        [TestCase(4, 4, 0, 8, new [] {North, West,  East,  South})]
        // Near diagonal step cases, south going clockwise
        [TestCase(2, 2, 3, 0, new [] {South, East,  West,  North})]
        [TestCase(2, 2, 1, 0, new [] {South, West,  East,  North})]
        [TestCase(2, 2, 0, 1, new [] {West,  South, North, East })]
        [TestCase(2, 2, 0, 3, new [] {West,  North, South, East })]
        [TestCase(2, 2, 1, 4, new [] {North, West,  East,  South})]
        [TestCase(2, 2, 3, 4, new [] {North, East,  West,  South})]
        [TestCase(2, 2, 4, 3, new [] {East,  North, South, West })]
        [TestCase(2, 2, 4, 1, new [] {East,  South, North, West })]
        public void OrientOptionsTest(int fromX, int fromZ, int toX, int toZ, Direction[] directions)
        {
            var from = new RoomId(fromX, fromZ);
            var to = new RoomId(toX, toZ);
            var orients = from.OrientPriority(to);
            Assert.AreEqual(directions.Length, orients.Length, 
                $"Test length {directions.Length} does not match output length {orients.Length}");
            for (var i = 0; i < directions.Length; i++)
                Assert.AreEqual(directions[i], orients[i], $"{orients[i]} expected {directions[i]}");
        }
    }
}