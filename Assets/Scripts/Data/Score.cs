namespace BlindWizard.Data
{
    public static class Score {
        // TODO: save/load the current score on pause/resume

        public static int Blocks { get; set; }

        public static int Floors { get; set; }

        public static int Kills { get; set; }

        public static int Turns { get; set; }

        public static int Shimmers { get; set; }

        public static void Clear()
        {
            Blocks = 0;
            Floors = 0;
            Kills = 0;
            Turns = 0;
            Shimmers = 0;
        }

        /// <summary>
        /// increases the number of blocks the player has traveled
        /// </summary>
        public static void BlockUp() => Blocks++;

        /// <summary>
        /// increases the number of floors the player has traveled
        /// </summary>
        public static void FloorUp() => Floors++;

        /// <summary>
        /// increses the number of enemies the player has killed
        /// </summary>
        public static void KillsUp() => Kills++;

        /// <summary>
        /// increases the number of shimmers the player has collected
        /// </summary>
        public static void ShimmersUp() => Shimmers++;

        /// <summary>
        /// increases the number of turns the player has taken
        /// </summary>
        public static void Turnip() => Turns++;
    }
}
