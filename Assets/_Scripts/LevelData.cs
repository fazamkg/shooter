using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;
using YG.Utils.LB;
using System.Collections;

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

        [NonSerialized] private List<LeaderboardEntry> _leaderboard = new();
        [NonSerialized] private bool _leaderboardSuccess;

        public UpgradeGroupData[] AvailableUpgrades => _availableUpgrades;

        private string LeaderboardName => $"level{name.GetNumberPart()}time";

        public IReadOnlyList<LeaderboardEntry> Leaderboard => _leaderboard;
        public LeaderboardEntry FirstPlace => _leaderboard.FirstOrDefault(x => x.Rank == 1);
        public LeaderboardEntry SecondPlace => _leaderboard.FirstOrDefault(x => x.Rank == 2);
        public LeaderboardEntry ThirdPlace => _leaderboard.FirstOrDefault(x => x.Rank == 3);
        public LeaderboardEntry PlayerPlace => _leaderboard.FirstOrDefault(x => x.IsPlayer);

        public void LoadLeaderboard(Action onSuccess, Action onFailure)
        {
            Routines.StartCoroutine_(WaitForLeaderboardCoroutine(onSuccess, onFailure));

            YandexGame.onGetLeaderboard += OnGetLeaderboard;
            var maxAmountPlayers = 4;
            var amountTop = 3;
            var amountAround = 0;
            YandexGame.GetLeaderboard(LeaderboardName,
                maxAmountPlayers, amountTop, amountAround, "nonePhoto");
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
            var player = PlayerPlace;
            if (player == null)
            {
                Storage.SetTimeSpan($"faza_{name}_timespan", elapsed);
                YandexGame.NewLBScoreTimeConvert(LeaderboardName, (float)elapsed.TotalMilliseconds);
            }
            else
            {
                if (elapsed < player.Time)
                {
                    Storage.SetTimeSpan($"faza_{name}_timespan", elapsed);
                    YandexGame.NewLBScoreTimeConvert(LeaderboardName, (float)elapsed.TotalMilliseconds);
                }
            }
        }

        public TimeSpan GetCompletedTimespan()
        {
            return Storage.GetTimeSpan($"faza_{name}_timespan", TimeSpan.MaxValue);
        }

        public void SavePlayerRank(int rank)
        {
            Storage.SetInt($"faza_{name}_rank", rank);
        }

        public int GetPlayerRank()
        {
            return Storage.GetInt($"faza_{name}_rank", -1);
        }

        private IEnumerator WaitForLeaderboardCoroutine(Action onSuccess, Action onFailure)
        {
            _leaderboardSuccess = false;
            _leaderboard.Clear();

            yield return new WaitForSeconds(1f);
            YandexGame.onGetLeaderboard -= OnGetLeaderboard;

            if (_leaderboardSuccess)
            {
                onSuccess?.Invoke();
            }
            else
            {
                onFailure?.Invoke();
            }
        }

        private void OnGetLeaderboard(LBData data)
        {
            YandexGame.onGetLeaderboard -= OnGetLeaderboard;
            _leaderboard.Clear();

            _leaderboardSuccess = data.entries != "no data";

            foreach (var entry in data.players)
            {
                var isPlayer = entry.uniqueID == YandexGame.playerId;

                var myEntry = new LeaderboardEntry(entry.rank,
                    entry.name, TimeSpan.FromMilliseconds(entry.score), isPlayer);

                if (isPlayer)
                {
                    SavePlayerRank(entry.rank);
                }

                _leaderboard.Add(myEntry);
            }
        }
    } 
}
