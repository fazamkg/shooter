using UnityEngine;
using DG.Tweening;

namespace Faza
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private CharacterInput _input;
        [SerializeField] private AbstractAnimation _weaponAnim;
        [SerializeField] private float _shootCooldown;
        [SerializeField] private Transform _origin;
        [SerializeField] private GameObject _bulletHolePrefab;
        [SerializeField] private LineRenderer _line;
        [SerializeField] private Transform _bulletOrigin;
        [SerializeField] private float _lineShowDuration;

        private float _cooldown;

        private void Awake()
        {
            var positions = new Vector3[2];
            var end = _origin.position + _origin.forward * 3.78f;
            end = _line.transform.InverseTransformPoint(end);
            positions[0] = end;
            positions[1] = Vector3.zero;

            _line.SetPositions(positions);
        }

        private void LateUpdate()
        {
            _cooldown -= Time.deltaTime;
            if (_cooldown < 0f)
            {
                _cooldown = 0f;
            }

            if (_cooldown > 0f)
            {
                return;
            }

            if (_input.IsFire())
            {
                _cooldown = _shootCooldown;

                _weaponAnim.Play();

                _line.gameObject.SetActive(true);

                var seq = DOTween.Sequence();
                seq.AppendInterval(_lineShowDuration);
                seq.AppendCallback(() => _line.gameObject.SetActive(false));

                var ray = new Ray(_origin.position, _origin.forward);
                var hit = Physics.Raycast(ray, out var info);
                if (hit)
                {
                    var pos = info.point + info.normal * 0.1f;
                    var rot = Quaternion.LookRotation(-info.normal);
                    var bulletHole = Instantiate(_bulletHolePrefab, pos, rot, info.collider.transform);
                    bulletHole.transform.Rotate(0f, 0f, Random.Range(0f, 360f), Space.Self);
                }
            }
        }
    } 
}
