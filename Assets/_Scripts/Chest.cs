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
        [SerializeField] private Collider _collider;

        private bool _opened;

        private void OnTriggerEnter(Collider other)
        {
            if (_opened) return;
            if (other.GetComponent<PlayerInput>() == false) return;

            _opened = true;
            _collider.enabled = false;

            PlayerInput.Instance.Character.Warp(_playerMagnetPos.position);

            StartCoroutine(PlayerInput.Instance.Character.RotateTowardsCoroutine
                (transform.position, 1000f));

            PlayerInput.Instance.CharacterAnimator.PlayOutOpenChestAnimation(this);
        }

        public void PlayOpenChestAnimation()
        {
            var seq = DOTween.Sequence();

            seq.Append(_lid.DOLocalRotate(new(90f, 0f, 0f), 0.3f).SetEase(Ease.InOutCirc));

            for (var i = 0; i < _coinAmount; i++)
            {
                seq.AppendCallback(() =>
                {
                    var coin = Instantiate(_coinPrefab, transform.position, Quaternion.identity);
                    coin.ActivateColliderDelayed();

                    //var horizontalSpeed = Random.Range(0f, 1f);
                    var verticalSpeed = Random.Range(13f, 16f);

                    var angle = Random.Range(0, 360f);
                    var direction = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                    coin.Rigidbody.AddForce(direction * 1f, ForceMode.VelocityChange);
                    coin.Rigidbody.AddForce(Vector3.up * verticalSpeed, ForceMode.VelocityChange);
                    coin.Rigidbody.AddTorque(Random.onUnitSphere * 1000f, ForceMode.VelocityChange);
                });
                seq.AppendInterval(0.1f);
            }
            
            seq.Append(transform.DOScale(0f, 0.3f).SetEase(Ease.InOutCirc));
        }
    } 
}
