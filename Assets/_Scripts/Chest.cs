using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private Transform _playerMagnetPos;
        [SerializeField] private Transform _lid;
        [SerializeField] private int _coinAmount;
        [SerializeField] private Coin _coinPrefab;

        private bool _opened;

        private void OnTriggerEnter(Collider other)
        {
            if (_opened) return;
            if (other.GetComponent<PlayerInput>() == false) return;

            _opened = true;

            StartCoroutine(PlayerInput.Instance.Character.RotateTowardsCoroutine
                (transform.position, 1000f));

            PlayerInput.Instance.Character.Warp(_playerMagnetPos.position);
            PlayerInput.Instance.CharacterAnimator.PlayOutOpenChestAnimation(this);
        }

        public void PlayOpenChestAnimation()
        {
            var seq = DOTween.Sequence();

            seq.Append(_lid.DORotate(new(90f, 0f, 0f), 0.3f).SetEase(Ease.InOutCirc));
            seq.Append(transform.DOScale(0f, 0.3f).SetEase(Ease.InOutCirc));
        }
    } 
}
