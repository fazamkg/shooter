using System;
using System.Collections.Generic;
using UnityEngine;
using YG;
using YG.Utils.LB;

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

    [CreateAssetMenu]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private UpgradeGroupData[] _availableUpgrades;
        [SerializeField] private BoosterData[] _boostersToUnlock;

        private List<LeaderboardEntry> _leaderboard = new();

        public UpgradeGroupData[] AvailableUpgrades => _availableUpgrades;

        private string LeaderboardName => $"level{name.GetNumberPart()}time";

        public IReadOnlyList<LeaderboardEntry> Leaderboard => _leaderboard;

        public void Init()
        {
            YandexGame.onGetLeaderboard += OnGetLeaderboard;
            YandexGame.GetLeaderboard(LeaderboardName, 4, 3, 3, "small");
        }

        private void OnGetLeaderboard(LBData data)
        {
            YandexGame.onGetLeaderboard -= OnGetLeaderboard;
            _leaderboard.Clear();

            foreach (var entry in data.players)
            {
                var isPlayer = entry.uniqueID == YandexGame.playerId;

                var myEntry = new LeaderboardEntry(entry.rank,
                    entry.name, TimeSpan.FromSeconds(entry.score), isPlayer);

                _leaderboard.Add(myEntry);
            }
        }

        public void UnlockBoosters()
        {
            if (_boostersToUnlock != null && _boostersToUnlock.Length > 0)
            {
                foreach (var booster in _boostersToUnlock)
                {
                    if (booster.IsUnlocked == false)
                    {
                        booster.IsUnlocked = true;
                        booster.GiveFreeAmount();
                    }
                }
            }
        }

        public void SetCompletedTimespan(TimeSpan elapsed)
        {
            var stored = Storage.GetTimeSpan($"faza_{name}_timespan", TimeSpan.MaxValue);
            if (elapsed < stored)
            {
                Storage.SetTimeSpan($"faza_{name}_timespan", elapsed);

                YandexGame.NewLBScoreTimeConvert(LeaderboardName, (float)elapsed.TotalSeconds);
            }
        }

        public TimeSpan GetCompletedTimespan()
        {
            return Storage.GetTimeSpan($"faza_{name}_timespan", TimeSpan.MaxValue);
        }
    } 
}
