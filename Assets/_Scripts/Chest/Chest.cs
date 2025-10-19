using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Faza
{
    public class Chest : MonoBehaviour
    {
        private const float OPEN_ANGLE = 90f;
        private const float OPEN_DURATION = 0.3f;
        private const Ease OPEN_EASE = Ease.InOutCirc;
        private const float DISAPPEAR_DURATION = 0.3f;
        private const float PLAYER_WARP_DURATION = 0.3f;
        private const Ease PLAYER_WARP_EASE = Ease.InOutCirc;
        private const Ease DISAPPEAR_EASE = Ease.InOutCirc;
        private const float COIN_MIN_VERTICAL_SPEED = 13f;
        private const float COIN_MAX_VERTICAL_SPEED = 16f;
        private const float COIN_TORQUE = 1000f;
        private const float COIN_INTERVAL = 0.1f;
        private const float PLAYER_ROTATE_TOWARDS_SPEED = 1000f;

        [SerializeField] private Transform _playerMagnetPos;
        [SerializeField] private Transform _lid;
        [SerializeField] private int _coinAmount;
        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private Collider _collider;
        [SerializeField] private AudioSource _source;

        private bool _opened;

        private void OnTriggerEnter(Collider other)
        {
            if (_opened) return;
            if (other.GetComponent<PlayerInput>() == false) return;

            _opened = true;
            _collider.enabled = false;

            StartCoroutine(MagnetPlayerCoroutine());
        }

        public void PlayOpenChestAnimation()
        {
            var seq = DOTween.Sequence();

            _source.Play();

            seq.Append(_lid.DOLocalRotate(new(OPEN_ANGLE, 0f, 0f), OPEN_DURATION).SetEase(OPEN_EASE));

            for (var i = 0; i < _coinAmount; i++)
            {
                seq.AppendCallback(() =>
                {
                    var coin = Instantiate(_coinPrefab, transform.position, Quaternion.identity);
                    coin.ActivateColliderDelayed();

                    var verticalSpeed = Random.Range(COIN_MIN_VERTICAL_SPEED, COIN_MAX_VERTICAL_SPEED);

                    var angle = Random.Range(0, 360f);
                    var direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                    coin.Rigidbody.AddForce(direction, ForceMode.VelocityChange);
                    coin.Rigidbody.AddForce(Vector3.up * verticalSpeed, ForceMode.VelocityChange);
                    coin.Rigidbody.AddTorque(Random.onUnitSphere * COIN_TORQUE, ForceMode.VelocityChange);
                });
                seq.AppendInterval(COIN_INTERVAL);
            }

            seq.Append(transform.DOScale(0f, DISAPPEAR_DURATION).SetEase(DISAPPEAR_EASE));
        }

        private IEnumerator MagnetPlayerCoroutine()
        {
            yield return PlayerInput.Instance.Character.SmoothWarp
                (_playerMagnetPos.position, PLAYER_WARP_DURATION).SetEase(PLAYER_WARP_EASE)
                .WaitForCompletion();

            yield return PlayerInput.Instance.Character.RotateTowardsCoroutine
                (transform.position, PLAYER_ROTATE_TOWARDS_SPEED);

            PlayerInput.Instance.CharacterAnimator.PlayOutOpenChestAnimation(this);
        }
    } 
}
