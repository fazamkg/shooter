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
            get => Storage.GetBool("faza_tutorial_1");
            set => Storage.SetBool("faza_tutorial_1", value);
        }

        private bool TutorialCompletedPref_2
        {
            get => Storage.GetBool("faza_tutorial_2");
            set => Storage.SetBool("faza_tutorial_2", value);
        }

        private bool TutorialCompletedPref_3
        {
            get => Storage.GetBool("faza_tutorial_3");
            set => Storage.SetBool("faza_tutorial_3", value);
        }

        private bool TutorialCompletedPref_4
        {
            get => Storage.GetBool("faza_tutorial_4");
            set => Storage.SetBool("faza_tutorial_4", value);
        }

        private bool TutorialCompletedPref_5
        {
            get => Storage.GetBool("faza_tutorial_5");
            set => Storage.SetBool("faza_tutorial_5", value);
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

                _material.SetVector("_Position", (Vector4)_position);

                _material.SetFloat("_Radius", (120f + Mathf.Sin(Time.time * 3f) * 20f) * _canvas.scaleFactor);
            }
        }

        public void StartTutorial1()
        {
            if (TutorialCompletedPref_1) return;

            SetTarget(_initialTarget, TransformType.Screen, true);

            _material.SetColor("_Color", new(0f, 0f, 0f, 0f));

            _triggerTouched = false;
            TutorialTrigger.OnEnter += TutorialTrigger_OnEnter;

            StartCoroutine(Tutorial1Coroutine());
        }

        public void StartTutorial2()
        {
            if (TutorialCompletedPref_2) return;

            SetTarget(_initialTarget, TransformType.Screen, true);

            _material.SetColor("_Color", new(0f, 0f, 0f, 0f));

            _triggerTouched = false;
            TutorialTrigger.OnEnter += TutorialTrigger_OnEnter;

            StartCoroutine(Tutorial2Coroutine());
        }

        public void StartTutorial3()
        {
            if (TutorialCompletedPref_3) return;

            SetTarget(_initialTarget, TransformType.Screen, true);

            _material.SetColor("_Color", new(0f, 0f, 0f, 0f));

            _triggerTouched = false;

            StartCoroutine(Tutorial3Coroutine());
        }

        public void StartTutorial4()
        {
            if (TutorialCompletedPref_4) return;

            SetTarget(_initialTarget, TransformType.Screen, true);

            _material.SetColor("_Color", new(0f, 0f, 0f, 0f));

            _triggerTouched = false;

            StartCoroutine(Tutorial4Coroutine());
        }

        public void StartTutorial5()
        {
            if (TutorialCompletedPref_5) return;

            SetTarget(_initialTarget, TransformType.Screen, true);

            _material.SetColor("_Color", new(0f, 0f, 0f, 0f));

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

            yield return TweenAlpha(0.95f, 0.3f).WaitForCompletion();

            SetTarget(PlayerInput.Instance.transform, TransformType.World);

            var pop = Instantiate(_popViewPrefab, transform);

            var mobile = YandexGame.EnvironmentData.isMobile ||
                YandexGame.EnvironmentData.isTablet;

            var key = mobile ? "tutorial_1_mobile" : "tutorial_1_pc";

            pop.Init(Localization.Get(key));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            PlayerInput.Instance.Enable();

            yield return new WaitUntil(() => PlayerInput.Instance.GetRawMove() != Vector3.zero);

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, 0.3f));

            yield return seq.WaitForCompletion();

            yield return new WaitUntil(() => _triggerTouched);

            pop.Init(Localization.Get("tutorial_2"));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => PlayerInput.Instance.Shooter.StartedShooting);

            TutorialCompletedPref_1 = true;

            yield return pop.Disappear().WaitForCompletion();
        }

        private IEnumerator Tutorial2Coroutine()
        {
            yield return new WaitForSeconds(0.5f);

            var pop = Instantiate(_popViewPrefab, transform);
            pop.Init(Localization.Get("tutorial_3"));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => _triggerTouched);

            PlayerInput.Instance.Disable();

            yield return pop.Disappear().WaitForCompletion();

            yield return TweenAlpha(0.95f, 0.2f).WaitForCompletion();

            SetTarget(FindFirstObjectByType<Chest>().transform, TransformType.World);

            pop.Init(Localization.Get("tutorial_4"));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitForSeconds(0.4f);

            PlayerInput.Instance.Enable();

            TutorialCompletedPref_2 = true;

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, 0.3f));

            yield return seq.WaitForCompletion();
        }

        private IEnumerator Tutorial3Coroutine()
        {
            PlayerInput.Instance.Disable();

            yield return TweenAlpha(0.95f, 0.3f).WaitForCompletion();

            SetTarget(FindFirstObjectByType<BoosterView>().transform, TransformType.Screen);

            var pop = Instantiate(_popViewPrefab, transform);
            pop.Init(Localization.Get("tutorial_5"));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => BoosterData.AnyBoosterRunning());

            PlayerInput.Instance.Enable();

            TutorialCompletedPref_3 = true;

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, 0.3f));

            yield return seq.WaitForCompletion();
        }

        private IEnumerator Tutorial4Coroutine()
        {
            PlayerInput.Instance.Disable();

            yield return TweenAlpha(0.95f, 0.3f).WaitForCompletion();

            var views = FindObjectsByType<BoosterView>(FindObjectsSortMode.None);
            var view = views.First(x => x.Data.name == "HasteBooster");

            SetTarget(view.transform, TransformType.Screen);

            var pop = Instantiate(_popViewPrefab, transform);
            pop.Init(Localization.Get("tutorial_6"));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => BoosterData.AnyBoosterRunning());

            PlayerInput.Instance.Enable();

            TutorialCompletedPref_4 = true;

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, 0.3f));

            yield return seq.WaitForCompletion();
        }

        private IEnumerator Tutorial5Coroutine()
        {
            PlayerInput.Instance.Disable();

            yield return TweenAlpha(0.95f, 0.3f).WaitForCompletion();

            var views = FindObjectsByType<BoosterView>(FindObjectsSortMode.None);
            var view = views.First(x => x.Data.name == "ArmorBooster");

            SetTarget(view.transform, TransformType.Screen);

            var pop = Instantiate(_popViewPrefab, transform);
            pop.Init(Localization.Get("tutorial_7"));
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => BoosterData.AnyBoosterRunning());

            PlayerInput.Instance.Enable();

            TutorialCompletedPref_5 = true;

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, 0.3f));

            yield return seq.WaitForCompletion();
        }

        private Tween TweenAlpha(float to, float duration)
        {
            return DOTween.To(() => _material.GetColor("_Color").a,
                x => _material.SetColor("_Color", new(0f, 0f, 0f, x)),
                to, duration);
        }
    } 
}
