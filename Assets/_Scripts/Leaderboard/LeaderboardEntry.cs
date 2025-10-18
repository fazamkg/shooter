using System;

namespace Faza
{
    public class LeaderboardEntry
    {
        public readonly int Rank;
        public readonly string Name;
        public readonly TimeSpan Time;
        public readonly bool IsPlayer;

        public LeaderboardEntry(int rank, string name, TimeSpan time, bool isPlayer)
        {
            Rank = rank;
            Name = name;
            Time = time;
            IsPlayer = isPlayer;
        }
    }
}
