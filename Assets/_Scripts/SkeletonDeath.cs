using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Linq;

namespace Faza
{
    public class SkeletonDeath : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private SkinnedMeshRenderer _skin;
        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private GameObject _gibHolder;
        [SerializeField] private Rigidbody[] _gibs;
        [SerializeField] private AnimationCurve _curve;

        private void Awake()
        {
            _health.OnDeath += Health_OnDeath;
        }

        private void Health_OnDeath()
        {
            _gibHolder.SetActive(true);
            _skin.enabled = false;

            ThrowGibs();

            StartCoroutine(DisableGibs());
        }

        private IEnumerator DisableGibs()
        {
            yield return new WaitForSeconds(10f);
            foreach (var thing in _gibs)
            {
                thing.constraints = RigidbodyConstraints.FreezeAll;
                thing.detectCollisions = false;
            }
        }

        private IEnumerator ThrowCoins()
        {
            if (_coinPrefab != null)
            {
                var allDead = EnemyInput.AllEnemies.All(x => x.Health.IsDead);
                for (var i = 0; i < 5; i++)
                {
                    var coin = Instantiate(_coinPrefab, transform.position, Quaternion.identity);
                    coin.ActivateColliderDelayed();

                    var horizontalSpeed = Random.Range(3f, 5f);
                    var verticalSpeed = Random.Range(10f, 14f);

                    var angle = Random.Range(-10f, 10f);
                    var direction = Quaternion.Euler(0f, angle, 0f) * _health.LastDamageDirection;
                    coin.Rigidbody.AddForce(direction * horizontalSpeed, ForceMode.VelocityChange);
                    coin.Rigidbody.AddForce(Vector3.up * verticalSpeed, ForceMode.VelocityChange);
                    coin.Rigidbody.AddTorque(Random.onUnitSphere * 1000f, ForceMode.VelocityChange);

                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        private void ThrowGibs()
        {
            foreach (var gib in _gibs)
            {
                var horizontalSpeed = Random.Range(3f, 5f);
                var verticalSpeed = Random.Range(10f, 14f);

                var angle = Random.Range(-10f, 10f);
                var direction = Quaternion.Euler(0f, angle, 0f) * _health.LastDamageDirection;
                gib.AddForce(direction * horizontalSpeed, ForceMode.VelocityChange);
                gib.AddForce(Vector3.up * verticalSpeed, ForceMode.VelocityChange);
                gib.AddTorque(Random.onUnitSphere * 1000f, ForceMode.VelocityChange);
            }

            StartCoroutine(ThrowCoins());

            //for (var i = 0; i < 3; i++)
            //{
            //    var angle = Random.Range(-10f, 10f);
            //    var direction = Quaternion.Euler(0f, angle, 0f) * _health.LastDamageDirection;

            //    var coin = Instantiate(_coinPrefab, transform.position, Quaternion.identity);
            //    var pos = transform.position.DeltaY(0.5f);

            //    var seq = DOTween.Sequence();

            //    for (var j = 1; j <= 3; j++)
            //    {
            //        pos += direction * (1f / j) * 0.9f;
            //        var force = (1f / j) * 3f;
            //        var dur = (1f / Mathf.Pow(j, 1.5f));
            //        seq.Append(coin.transform.DOJump(pos, force, 1, dur).SetEase(Ease.Linear));
            //    }
            //}
        }
    }
}
