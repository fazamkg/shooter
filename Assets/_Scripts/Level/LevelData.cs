using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;
using YG.Utils.LB;
using System.Collections;

namespace Faza
{
    [CreateAssetMenu]
    public class LevelData : ScriptableObject
    {
        private const string PHOTO_SIZE = "nonePhoto";

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
            Routines.StartCoroutineNew(WaitForLeaderboardCoroutine(onSuccess, onFailure));

            YandexGame.onGetLeaderboard += OnGetLeaderboard;
            var maxAmountPlayers = 4;
            var amountTop = 3;
            var amountAround = 0;
            YandexGame.GetLeaderboard(LeaderboardName,
                maxAmountPlayers, amountTop, amountAround, PHOTO_SIZE);
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
                Storage.SetTimeSpan(StorageKey.GetLeaderboardTimeKey(name), elapsed);
                YandexGame.NewLBScoreTimeConvert(LeaderboardName, (float)elapsed.TotalMilliseconds);
            }
            else
            {
                if (elapsed < player.Time)
                {
                    Storage.SetTimeSpan(StorageKey.GetLeaderboardTimeKey(name), elapsed);
                    YandexGame.NewLBScoreTimeConvert(LeaderboardName, (float)elapsed.TotalMilliseconds);
                }
            }
        }

        public TimeSpan GetCompletedTimespan()
        {
            return Storage.GetTimeSpan(StorageKey.GetLeaderboardTimeKey(name), TimeSpan.MaxValue);
        }

        public void SavePlayerRank(int rank)
        {
            Storage.SetInt(StorageKey.GetLeaderboardRankKey(name), rank);
        }

        public int GetPlayerRank()
        {
            return Storage.GetInt(StorageKey.GetLeaderboardRankKey(name), -1);
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
