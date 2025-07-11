using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Faza
{
    public class SkeletonDeath : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private SkinnedMeshRenderer _skin;
        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private GameObject _gibHolder;
        [SerializeField] private Rigidbody[] _gibs;

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

            for (var i = 0; i < 3; i++)
            {
                var angle = Random.Range(-10f, 10f);
                var direction = Quaternion.Euler(0f, angle, 0f) * _health.LastDamageDirection;
                var pos = transform.position.DeltaY(1f) + direction * 4f;

                var coin = Instantiate(_coinPrefab, transform.position, Quaternion.identity);
                coin.transform.DOJump(pos, 1f, 5, 1.5f).SetEase(Ease.Linear);
            }
        }
    }
}
