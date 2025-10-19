using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;
using YG;

namespace Faza
{
    public enum TransformType
    {
        World,
        Screen
    }

    public class Tutorial : MonoBehaviour
    {
        private const string HASTE_BOOSTER = "HasteBooster";
        private const string ARMOR_BOOSTER = "ArmorBooster";
        private const float TARGET_FADE = 0.95f;
        private const float TARGET_FADE_DURATION1 = 0.3f;
        private const float TARGET_FADE_DURATION2 = 0.2f;
        private const float TUTORIAL2_DELAY = 0.5f;
        private const float TUTORIAL2_DELAY2 = 0.4f;
        private const float RADIUS = 120f;
        private const float RADIUS_TIME_SPEED = 3f;
        private const float RADIUS_SHIFT = 20f;

        [SerializeField] private Material _material;
        [SerializeField] private TutorialPopView _popViewPrefab;
        [SerializeField] private Transform _defaultPopPos;
        [SerializeField] private Transform _initialTarget;
        [SerializeField] private float _targetSpeed;
        [SerializeField] private Canvas _canvas;

        private Transform _target;
        private TransformType _transformType;
        private Vector2 _position;

        private bool _triggerTouched;

        private bool TutorialCompletedPref_1
        {
            get => Storage.GetBool(StorageKey.TUTORIAL1);
            set => Storage.SetBool(StorageKey.TUTORIAL1, value);
        }

        private bool TutorialCompletedPref_2
        {
            get => Storage.GetBool(StorageKey.TUTORIAL2);
            set => Storage.SetBool(StorageKey.TUTORIAL2, value);
        }

        private bool TutorialCompletedPref_3
        {
            get => Storage.GetBool(StorageKey.TUTORIAL3);
            set => Storage.SetBool(StorageKey.TUTORIAL3, value);
        }

        private bool TutorialCompletedPref_4
        {
            get => Storage.GetBool(StorageKey.TUTORIAL4);
            set => Storage.SetBool(StorageKey.TUTORIAL4, value);
        }

        private bool TutorialCompletedPref_5
        {
            get => Storage.GetBool(StorageKey.TUTORIAL5);
            set => Storage.SetBool(StorageKey.TUTORIAL5, value);
        }

        private void OnDestroy()
        {
            TutorialTrigger.OnEnter -= TutorialTrigger_OnEnter;
        }

        private void Update()
        {
            if (_target != null)
            {
                var targetPosition = _transformType switch
                {
                    TransformType.World => Camera.main.WorldToScreenPoint(_target.position),
                    TransformType.Screen => _target.position,
                    _ => Vector3.zero
                };

                _position = Vector2.MoveTowards(_position, targetPosition,
                    Time.deltaTime * _targetSpeed * _canvas.scaleFactor);

                _material.SetVector(ShaderKey.Position, (Vector4)_position);

                _material.SetFloat(ShaderKey.Radius, 
                    (RADIUS + Mathf.Sin(Time.time * RADIUS_TIME_SPEED) * RADIUS_SHIFT) * _canvas.scaleFactor);
            }
        }

        public void StartTutorial1()
        {
            if (TutorialCompletedPref_1) return;

            SetTarget(_initialTarget, TransformType.Screen, true);

            _material.SetColor(ShaderKey.Color, new(0f, 0f, 0f, 0f));

            _triggerTouched = false;
            TutorialTrigger.OnEnter += TutorialTrigger_OnEnter;

            StartCoroutine(Tutorial1Coroutine());
        }

        public void StartTutorial2()
        {
            if (TutorialCompletedPref_2) return;

            SetTarget(_initialTarget, TransformType.Screen, true);

            _material.SetColor(ShaderKey.Color, new(0f, 0f, 0f, 0f));

            _triggerTouched = false;
            TutorialTrigger.OnEnter += TutorialTrigger_OnEnter;

            StartCoroutine(Tutorial2Coroutine());
        }

        public void StartTutorial3()
        {
            if (TutorialCompletedPref_3) return;

            SetTarget(_initialTarget, TransformType.Screen, true);

            _material.SetColor(ShaderKey.Color, new(0f, 0f, 0f, 0f));

            _triggerTouched = false;

            StartCoroutine(Tutorial3Coroutine());
        }

        public void StartTutorial4()
        {
            if (TutorialCompletedPref_4) return;

            SetTarget(_initialTarget, TransformType.Screen, true);

            _material.SetColor(ShaderKey.Color, new(0f, 0f, 0f, 0f));

            _triggerTouched = false;

            StartCoroutine(Tutorial4Coroutine());
        }

        public void StartTutorial5()
        {
            if (TutorialCompletedPref_5) return;

            SetTarget(_initialTarget, TransformType.Screen, true);

            _material.SetColor(ShaderKey.Color, new(0f, 0f, 0f, 0f));

            _triggerTouched = false;

            StartCoroutine(Tutorial5Coroutine());
        }

        private void SetTarget(Transform target, TransformType type, bool instant = false)
        {
            _target = target;
            _transformType = type;

            if (instant)
            {
                var targetPosition = type switch
                {
                    TransformType.World => Camera.main.WorldToScreenPoint(target.position),
                    TransformType.Screen => target.position,
                    _ => Vector3.zero
                };

                _position = targetPosition;
            }
        }

        private void TutorialTrigger_OnEnter()
        {
            _triggerTouched = true;
        }

        private IEnumerator Tutorial1Coroutine()
        {
            PlayerInput.Instance.Disable();

            yield return TweenAlpha(TARGET_FADE, TARGET_FADE_DURATION1).WaitForCompletion();

            SetTarget(PlayerInput.Instance.transform, TransformType.World);

            var pop = Instantiate(_popViewPrefab, transform);

            var mobile = YandexGame.EnvironmentData.isMobile ||
                YandexGame.EnvironmentData.isTablet;

            var key = mobile ? LocalizationKey.TUTORIAL_MOBILE : LocalizationKey.TUTORIAL_PC;

            pop.Init(Localization.Get(key));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            PlayerInput.Instance.Enable();

            yield return new WaitUntil(() => PlayerInput.Instance.GetRawMove() != Vector3.zero);

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, TARGET_FADE_DURATION1));

            yield return seq.WaitForCompletion();

            yield return new WaitUntil(() => _triggerTouched);

            pop.Init(Localization.Get(LocalizationKey.TUTORIAL2));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => PlayerInput.Instance.Shooter.StartedShooting);

            TutorialCompletedPref_1 = true;

            yield return pop.Disappear().WaitForCompletion();
        }

        private IEnumerator Tutorial2Coroutine()
        {
            yield return new WaitForSeconds(TUTORIAL2_DELAY);

            var pop = Instantiate(_popViewPrefab, transform);
            pop.Init(Localization.Get(LocalizationKey.TUTORIAL3));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => _triggerTouched);

            PlayerInput.Instance.Disable();

            yield return pop.Disappear().WaitForCompletion();

            yield return TweenAlpha(TARGET_FADE, TARGET_FADE_DURATION2).WaitForCompletion();

            SetTarget(FindFirstObjectByType<Chest>().transform, TransformType.World);

            pop.Init(Localization.Get(LocalizationKey.TUTORIAL4));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitForSeconds(TUTORIAL2_DELAY2);

            PlayerInput.Instance.Enable();

            TutorialCompletedPref_2 = true;

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, TARGET_FADE_DURATION1));

            yield return seq.WaitForCompletion();
        }

        private IEnumerator Tutorial3Coroutine()
        {
            PlayerInput.Instance.Disable();

            yield return TweenAlpha(TARGET_FADE, TARGET_FADE_DURATION1).WaitForCompletion();

            SetTarget(FindFirstObjectByType<BoosterView>().transform, TransformType.Screen);

            var pop = Instantiate(_popViewPrefab, transform);
            pop.Init(Localization.Get(LocalizationKey.TUTORIAL5));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => BoosterData.AnyBoosterRunning());

            PlayerInput.Instance.Enable();

            TutorialCompletedPref_3 = true;

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, TARGET_FADE_DURATION1));

            yield return seq.WaitForCompletion();
        }

        private IEnumerator Tutorial4Coroutine()
        {
            PlayerInput.Instance.Disable();

            yield return TweenAlpha(TARGET_FADE, TARGET_FADE_DURATION1).WaitForCompletion();

            var views = FindObjectsByType<BoosterView>(FindObjectsSortMode.None);
            var view = views.First(x => x.Data.name == HASTE_BOOSTER);

            SetTarget(view.transform, TransformType.Screen);

            var pop = Instantiate(_popViewPrefab, transform);
            pop.Init(Localization.Get(LocalizationKey.TUTORIAL6));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => BoosterData.AnyBoosterRunning());

            PlayerInput.Instance.Enable();

            TutorialCompletedPref_4 = true;

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, TARGET_FADE_DURATION1));

            yield return seq.WaitForCompletion();
        }

        private IEnumerator Tutorial5Coroutine()
        {
            PlayerInput.Instance.Disable();

            yield return TweenAlpha(TARGET_FADE, TARGET_FADE_DURATION1).WaitForCompletion();

            var views = FindObjectsByType<BoosterView>(FindObjectsSortMode.None);
            var view = views.First(x => x.Data.name == ARMOR_BOOSTER);

            SetTarget(view.transform, TransformType.Screen);

            var pop = Instantiate(_popViewPrefab, transform);
            pop.Init(Localization.Get(LocalizationKey.TUTORIAL7));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => BoosterData.AnyBoosterRunning());

            PlayerInput.Instance.Enable();

            TutorialCompletedPref_5 = true;

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, TARGET_FADE_DURATION1));

            yield return seq.WaitForCompletion();
        }

        private Tween TweenAlpha(float to, float duration)
        {
            return DOTween.To(() => _material.GetColor(ShaderKey.Color).a,
                x => _material.SetColor(ShaderKey.Color, new(0f, 0f, 0f, x)),
                to, duration);
        }
    } 
}
