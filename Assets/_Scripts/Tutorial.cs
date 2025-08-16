using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;

namespace Faza
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private Material _material;
        [SerializeField] private TutorialPopView _popViewPrefab;
        [SerializeField] private Transform _defaultPopPos;

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

        public void StartTutorial_1()
        {
            if (TutorialCompletedPref_1) return;

            var width = Screen.width;
            var height = Screen.height;

            var radius = _material.GetFloat("_Radius");

            var x = width / 2;
            var y = height + radius;

            _material.SetVector("_Position", new(x, y, 0f, 0f));
            _material.SetColor("_Color", new(0f, 0f, 0f, 0f));

            StartCoroutine(TutorialCoroutine_1());
        }

        private IEnumerator TutorialCoroutine_1()
        {
            PlayerInput.Instance.Disable();

            yield return TweenAlpha(0.9f, 0.3f).WaitForCompletion();

            var playerWorld = PlayerInput.Instance.transform.position;
            playerWorld.z += 1f;
            var playerScreen = Camera.main.WorldToScreenPoint(playerWorld);

            yield return TweenPosition(playerScreen, 0.3f).WaitForCompletion();

            var pop = Instantiate(_popViewPrefab, transform);
            pop.Init("Используй WASD или стрелочки чтобы перемещаться");
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            PlayerInput.Instance.Enable();

            yield return new WaitUntil(() => PlayerInput.Instance.GetRawMove() != Vector3.zero);

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, 0.3f));

            yield return seq.WaitForCompletion();

            yield return new WaitForSeconds(4f);

            pop.Init("твой скелет автоматически стреляет когда ты стоишь и враг близко");
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => PlayerInput.Instance.Shooter.StartedShooting);

            TutorialCompletedPref_1 = true;

            yield return pop.Disappear().WaitForCompletion();
        }

        public void StartTutorial_2()
        {
            if (TutorialCompletedPref_2) return;

            var width = Screen.width;
            var height = Screen.height;

            var radius = _material.GetFloat("_Radius");

            var x = width / 2;
            var y = height + radius;

            _material.SetVector("_Position", new(x, y, 0f, 0f));
            _material.SetColor("_Color", new(0f, 0f, 0f, 0f));

            _triggerTouched = false;
            TutorialTrigger.OnEnter += TutorialTrigger_OnEnter;

            StartCoroutine(TutorialCoroutine_2());
        }

        private void TutorialTrigger_OnEnter()
        {
            _triggerTouched = true;
        }

        private IEnumerator TutorialCoroutine_2()
        {
            yield return new WaitForSeconds(0.5f);

            var pop = Instantiate(_popViewPrefab, transform);
            pop.Init("на этом уровне есть сундук! найди его!");
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => _triggerTouched);

            PlayerInput.Instance.Disable();

            yield return pop.Disappear().WaitForCompletion();

            yield return TweenAlpha(0.9f, 0.3f).WaitForCompletion();

            var chestWorld = FindFirstObjectByType<Chest>().transform.position;
            chestWorld.z += 1.5f;
            var chestScreen = Camera.main.WorldToScreenPoint(chestWorld);

            yield return TweenPosition(chestScreen, 0.3f).WaitForCompletion();

            pop.Init("сундук здесь! открой его!");
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitForSeconds(2f);

            PlayerInput.Instance.Enable();

            TutorialCompletedPref_2 = true;

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, 0.3f));

            yield return seq.WaitForCompletion();
        }

        public void StartTutorial_3()
        {
            if (TutorialCompletedPref_3) return;

            var width = Screen.width;
            var height = Screen.height;

            var radius = _material.GetFloat("_Radius");

            var x = width / 2;
            var y = height + radius;

            _material.SetVector("_Position", new(x, y, 0f, 0f));
            _material.SetColor("_Color", new(0f, 0f, 0f, 0f));

            _triggerTouched = false;
            TutorialTrigger.OnEnter += TutorialTrigger_OnEnter;

            StartCoroutine(TutorialCoroutine_3());
        }

        private IEnumerator TutorialCoroutine_3()
        {
            PlayerInput.Instance.Disable();

            yield return TweenAlpha(0.9f, 0.3f).WaitForCompletion();

            var boosterScreen = FindFirstObjectByType<BoosterView>().transform.position;
            boosterScreen.y = Screen.height - boosterScreen.y;

            yield return TweenPosition(boosterScreen, 0.3f).WaitForCompletion();

            var pop = Instantiate(_popViewPrefab, transform);
            pop.Init("Ты разблокировал бустер! Он умножает весь твой урон пока активен! Нажми на него чтобы активировать!");
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => BoosterData.AnyBoosterRunning());

            PlayerInput.Instance.Enable();

            TutorialCompletedPref_3 = true;

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, 0.3f));

            yield return seq.WaitForCompletion();
        }

        public void StartTutorial_4()
        {
            if (TutorialCompletedPref_4) return;

            var width = Screen.width;
            var height = Screen.height;

            var radius = _material.GetFloat("_Radius");

            var x = width / 2;
            var y = height + radius;

            _material.SetVector("_Position", new(x, y, 0f, 0f));
            _material.SetColor("_Color", new(0f, 0f, 0f, 0f));

            _triggerTouched = false;
            TutorialTrigger.OnEnter += TutorialTrigger_OnEnter;

            StartCoroutine(TutorialCoroutine_4());
        }

        private IEnumerator TutorialCoroutine_4()
        {
            PlayerInput.Instance.Disable();

            yield return TweenAlpha(0.9f, 0.3f).WaitForCompletion();

            var views = FindObjectsByType<BoosterView>(FindObjectsSortMode.None);
            var view = views.First(x => x.Data.name == "HasteBooster");

            var boosterScreen = view.transform.position;
            boosterScreen.y = Screen.height - boosterScreen.y;

            yield return TweenPosition(boosterScreen, 0.3f).WaitForCompletion();

            var pop = Instantiate(_popViewPrefab, transform);
            pop.Init("Ты разблокировал новый бустер! Он сильно увеличивает скорость! Нажми на него чтобы активировать!");
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => BoosterData.AnyBoosterRunning());

            PlayerInput.Instance.Enable();

            TutorialCompletedPref_4 = true;

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, 0.3f));

            yield return seq.WaitForCompletion();
        }

        public void StartTutorial_5()
        {
            if (TutorialCompletedPref_5) return;

            var width = Screen.width;
            var height = Screen.height;

            var radius = _material.GetFloat("_Radius");

            var x = width / 2;
            var y = height + radius;

            _material.SetVector("_Position", new(x, y, 0f, 0f));
            _material.SetColor("_Color", new(0f, 0f, 0f, 0f));

            _triggerTouched = false;
            TutorialTrigger.OnEnter += TutorialTrigger_OnEnter;

            StartCoroutine(TutorialCoroutine_5());
        }

        private IEnumerator TutorialCoroutine_5()
        {
            PlayerInput.Instance.Disable();

            yield return TweenAlpha(0.9f, 0.3f).WaitForCompletion();

            var views = FindObjectsByType<BoosterView>(FindObjectsSortMode.None);
            var view = views.First(x => x.Data.name == "ArmorBooster");

            var boosterScreen = view.transform.position;
            boosterScreen.y = Screen.height - boosterScreen.y;

            yield return TweenPosition(boosterScreen, 0.3f).WaitForCompletion();

            var pop = Instantiate(_popViewPrefab, transform);
            pop.Init("Ты разблокировал новый бустер! Он не дает тебе получить урон! Нажми на него чтобы активировать!");
            yield return pop.Appear(_defaultPopPos.position).WaitForCompletion();

            yield return new WaitUntil(() => BoosterData.AnyBoosterRunning());

            PlayerInput.Instance.Enable();

            TutorialCompletedPref_5 = true;

            var seq = DOTween.Sequence();
            seq.Append(pop.Disappear());
            seq.Join(TweenAlpha(0f, 0.3f));

            yield return seq.WaitForCompletion();
        }

        private Tween TweenPosition(Vector2 to, float duration)
        {
            return DOTween.To(() => (Vector2)_material.GetVector("_Position"),
                x => _material.SetVector("_Position", (Vector4)x),
                to, duration);
        }

        private Tween TweenAlpha(float to, float duration)
        {
            return DOTween.To(() => _material.GetColor("_Color").a,
                x => _material.SetColor("_Color", new(0f, 0f, 0f, x)),
                to, duration);
        }
    } 
}
