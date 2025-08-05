using UnityEngine;

namespace Faza
{
    public class LevelSelectionView : MonoBehaviour
    {
        [SerializeField] private Transform _levelButtonParent;
        [SerializeField] private LevelButtonView _levelButtonPrefab;

        private void Awake()
        {
            foreach (var level in LevelManager.Instance.Levels)
            {
                var levelButton = Instantiate(_levelButtonPrefab, _levelButtonParent);
                levelButton.Init(level);
            }
        }
    } 
}
