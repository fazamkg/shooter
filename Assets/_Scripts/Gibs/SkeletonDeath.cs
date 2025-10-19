using UnityEngine;
using System.Collections;
using System.Linq;

namespace Faza
{
    public class SkeletonDeath : MonoBehaviour
    {
        private const float TIME_TO_DISABLE = 10f;
        private const float COIN_MIN_HORIZONTAL_SPEED = 3f;
        private const float COIN_MAX_HORIZONTAL_SPEED = 5f;
        private const float COIN_MIN_VERTICAL_SPEED = 10f;
        private const float COIN_MAX_VERTICAL_SPEED = 14f;
        private const float COIN_MIN_ANGLE = -10f;
        private const float COIN_MAX_ANGLE = 10f;
        private const float COIN_TORQUE = 1000f;
        private const int COIN_COUNT = 5;
        private const float COIN_INTERVAL = 0.1f;
        private const float GIB_MIN_HORIZONTAL_SPEED = 3f;
        private const float GIB_MAX_HORIZONTAL_SPEED = 5f;
        private const float GIB_MIN_VERTICAL_SPEED = 10f;
        private const float GIB_MAX_VERTICAL_SPEED = 14f;
        private const float GIB_MIN_ANGLE = -10f;
        private const float GIB_MAX_ANGLE = 10f;
        private const float GIB_TORQUE = 1000f;

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

            StartCoroutine(DisableGibsCoroutine());
        }

        private IEnumerator DisableGibsCoroutine()
        {
            yield return new WaitForSeconds(TIME_TO_DISABLE);
            foreach (var thing in _gibs)
            {
                thing.constraints = RigidbodyConstraints.FreezeAll;
                thing.detectCollisions = false;
            }
        }

        private IEnumerator ThrowCoinsCoroutine()
        {
            if (_coinPrefab != null)
            {
                var allDead = EnemyInput.AllEnemies.All(x => x.Health.IsDead);
                for (var i = 0; i < COIN_COUNT; i++)
                {
                    var coin = Instantiate(_coinPrefab, transform.position, Quaternion.identity);
                    coin.ActivateColliderDelayed();

                    var horizontalSpeed = Random.Range(COIN_MIN_HORIZONTAL_SPEED, COIN_MAX_HORIZONTAL_SPEED);
                    var verticalSpeed = Random.Range(COIN_MIN_VERTICAL_SPEED, COIN_MAX_VERTICAL_SPEED);

                    var angle = Random.Range(COIN_MIN_ANGLE, COIN_MAX_ANGLE);
                    var direction = Quaternion.Euler(0f, angle, 0f) * _health.LastDamageDirection;
                    coin.Rigidbody.AddForce(direction * horizontalSpeed, ForceMode.VelocityChange);
                    coin.Rigidbody.AddForce(Vector3.up * verticalSpeed, ForceMode.VelocityChange);
                    coin.Rigidbody.AddTorque(Random.onUnitSphere * COIN_TORQUE, ForceMode.VelocityChange);

                    yield return new WaitForSeconds(COIN_INTERVAL);
                }
            }
        }

        private void ThrowGibs()
        {
            foreach (var gib in _gibs)
            {
                var horizontalSpeed = Random.Range(GIB_MIN_HORIZONTAL_SPEED, GIB_MAX_HORIZONTAL_SPEED);
                var verticalSpeed = Random.Range(GIB_MIN_VERTICAL_SPEED, GIB_MAX_VERTICAL_SPEED);

                var angle = Random.Range(GIB_MIN_ANGLE, GIB_MAX_ANGLE);
                var direction = Quaternion.Euler(0f, angle, 0f) * _health.LastDamageDirection;
                gib.AddForce(direction * horizontalSpeed, ForceMode.VelocityChange);
                gib.AddForce(Vector3.up * verticalSpeed, ForceMode.VelocityChange);
                gib.AddTorque(Random.onUnitSphere * GIB_TORQUE, ForceMode.VelocityChange);
            }

            StartCoroutine(ThrowCoinsCoroutine());
        }
    }
}
