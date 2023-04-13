namespace General
{
    public static class Constants
    {
        public static class Namings
        {
            public const string Player = "Player";
            public const string World = "World";
        }

        public static class Values
        {
            public const float PositionEpsilon = 0.01f;
            public const float Epsilon = 1E-6f;
            public const float EpsilonSqr = Epsilon * Epsilon;
            public const float MovementDeadzone = 0.15f;
            public const float MovementDeadzoneSquared = MovementDeadzone * MovementDeadzone;
        }
    }
}