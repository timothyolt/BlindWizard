namespace BlindWizard.Data
{
    public class StaticLevel0 : WizardLevel
    {
        public StaticLevel0(int level) : base(level)
        {
        }

        public override void Generate()
        {
            GenerateRooms();
            // oposite corner has hole
            Rooms[3, 3].FloorGen = false;

            // other 2 corners have shimmers
            Rooms[0, 3].ShimmerGen = true;
            Rooms[3, 0].ShimmerGen = true;

            // L shaped walls leading towards pit
            GenerateWalls();
        }

        protected override void GenerateWalls()
        {
            base.GenerateWalls();
            Rooms[0, 0].Walls[Direction.North] = new Wall {Gen = false};
            Rooms[0, 0].Walls[Direction.East] = new Wall {Gen = false};

            // Room 0,1
            Rooms[0, 1].Walls[Direction.South] = new Wall {Gen = false};
            Rooms[0, 1].Walls[Direction.North] = new Wall {Gen = false};
            Rooms[0, 1].Walls[Direction.East] = new Wall {Gen = false};

            //Room 0,2
            Rooms[0, 2].Walls[Direction.North] = new Wall {Gen = false};
            Rooms[0, 2].Walls[Direction.South] = new Wall {Gen = false};

            //Room 0,3
            Rooms[0, 3].Walls[Direction.South] = new Wall {Gen = false};
            Rooms[0, 3].Walls[Direction.East] = new Wall {Gen = false};

            //Room 1,0
            Rooms[1, 0].Walls[Direction.West] = new Wall {Gen = false};
            Rooms[1, 0].Walls[Direction.North] = new Wall {Gen = false};
            Rooms[1, 0].Walls[Direction.East] = new Wall {Gen = false};

            //Room 1,
            Rooms[1, 1].Walls[Direction.North] = new Wall {Gen = false};
            Rooms[1, 1].Walls[Direction.South] = new Wall {Gen = false};
            Rooms[1, 1].Walls[Direction.East] = new Wall {Gen = false};
            Rooms[1, 1].Walls[Direction.West] = new Wall {Gen = false};

            //Room 1,2
            Rooms[1, 2].Walls[Direction.South] = new Wall {Gen = false};
            Rooms[1, 2].Walls[Direction.East] = new Wall {Gen = false};

            //Room 1,3
            Rooms[1, 3].Walls[Direction.West] = new Wall {Gen = false};
            Rooms[1, 3].Walls[Direction.East] = new Wall {Gen = false};

            //Room 2,0
            Rooms[2, 0].Walls[Direction.West] = new Wall {Gen = false};
            Rooms[2, 0].Walls[Direction.East] = new Wall {Gen = false};

            //Room 2,1
            Rooms[2, 1].Walls[Direction.North] = new Wall {Gen = false};
            Rooms[2, 1].Walls[Direction.West] = new Wall {Gen = false};

            //Room 2,2
            Rooms[2, 2].Walls[Direction.South] = new Wall {Gen = false};
            Rooms[2, 2].Walls[Direction.North] = new Wall {Gen = false};
            Rooms[2, 2].Walls[Direction.West] = new Wall {Gen = false};
            Rooms[2, 2].Walls[Direction.East] = new Wall {Gen = false};

            //Room 2,3
            Rooms[2, 3].Walls[Direction.South] = new Wall {Gen = false};
            Rooms[2, 3].Walls[Direction.East] = new Wall {Gen = false};
            Rooms[2, 3].Walls[Direction.West] = new Wall {Gen = false};

            //Room 3,0
            Rooms[3, 0].Walls[Direction.North] = new Wall {Gen = false};
            Rooms[3, 0].Walls[Direction.West] = new Wall {Gen = false};

            //Room 3,1
            Rooms[3, 1].Walls[Direction.South] = new Wall {Gen = false};
            Rooms[3, 1].Walls[Direction.North] = new Wall {Gen = false};

            //Room 3,2
            Rooms[3, 2].Walls[Direction.North] = new Wall {Gen = false};
            Rooms[3, 2].Walls[Direction.South] = new Wall {Gen = false};
            Rooms[3, 2].Walls[Direction.West] = new Wall {Gen = false};

            //Room 3,3
            Rooms[3, 3].Walls[Direction.West] = new Wall {Gen = false};
            Rooms[3, 3].Walls[Direction.South] = new Wall {Gen = false};
        }

        public override bool IsDone => true;
    }
}