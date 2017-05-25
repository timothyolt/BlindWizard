namespace Blindwizard.Data
{
    public class RoomParameters
    {
        public int RoomSize { get; set; }
        public bool HasFloor { get; set; }
        public bool HasWallNorth { get; set; }
        public bool HasWallSouth { get; set; }
        public bool HasWallEast { get; set; }
        public bool HasWallWest { get; set; }
        public bool HasEnemy { get; set; }
        public bool HasShimmer { get; set; }
    }
}