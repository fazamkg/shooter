using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace Faza
{
    [CreateAssetMenu]
    public class LevelManager : ScriptableObject
    {
        [SerializeField] private LevelData[] _levels;
        [SerializeField] private LevelData _menuLevel;

        private static int LevelIndexPref
        {
            get => Storage.GetInt("faza_level");
            set => Storage.SetInt("faza_level", value);
        }

        public static LevelManager Instance => Resources.Load<LevelManager>("LevelManager");

        public LevelData[] Levels => _levels;

        public bool MenuUnlocked => LevelIndexPref >= _levels.Length;

        public void LoadLevel(LevelData levelData)
        {
            SceneManager.LoadScene(levelData.name);
        }

        public void LoadLevelFromSave(bool showAd = false)
        {
            void loadNextLevel()
            {
                YandexGame.CloseFullAdEvent -= loadNextLevel;
                YandexGame.ErrorFullAdEvent -= loadNextLevel;

                var index = LevelIndexPref;
                var levelToLoad = index >= _levels.Length ? _menuLevel : _levels[index];
                SceneManager.LoadScene(levelToLoad.name);
            }

            if (showAd && YandexGame.Instance.CanShowAd)
            {
                YandexGame.ErrorFullAdEvent += loadNextLevel;
                YandexGame.CloseFullAdEvent += loadNextLevel;
                YandexGame.FullscreenShow();
            }
            else
            {
                loadNextLevel();
            }
        }

        public void OnWinLevel(LevelData level)
        {
            var savedIndex = LevelIndexPref;
            var wonIndex = -1;
            for (var i = 0; i < _levels.Length; i++)
            {
                if (level == _levels[i])
                {
                    wonIndex = i + 1;
                    break;
                }
            }

            if (wonIndex <= savedIndex) return;

            LevelIndexPref = wonIndex;
        }
    } 
}
