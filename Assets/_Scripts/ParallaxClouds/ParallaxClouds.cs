using UnityEngine;

namespace Faza
{
    [DefaultExecutionOrder(10)]
    public class ParallaxClouds : MonoBehaviour
    {
        private const float MAX_REPEAT = 100f;

        [SerializeField] private float _speed;
        [SerializeField] private float _speed2;

        private float _shift;
        private Vector3 _originalPos;

        private void Awake()
        {
            _originalPos = transform.localPosition;
        }

        private void LateUpdate()
        {
            var playerPos = PlayerInput.Instance.transform.position;
            var y = playerPos.z * _speed2;
            var x = playerPos.x * _speed2;

            _shift += _speed * Time.deltaTime;
            _shift = Mathf.Repeat(_shift, MAX_REPEAT);

            transform.localPosition = _originalPos + _shift * Vector3.right + new Vector3(-x, -y, 0f);
        }
    } 
}
