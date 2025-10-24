using System;
using UnityEngine;

#if YG_PLUGIN_YANDEX_GAME
using YG;
#endif

namespace Faza
{
    [Serializable]
    public class RvSpendAction : SpendAction
    {
        private const string RV_SPRITE_PATH = "Rv";

        public override Sprite Sprite => Resources.Load<Sprite>(RV_SPRITE_PATH);

        public override string Cost => Localization.Get(LocalizationKey.FREE);

        public override bool CanSpend()
        {
            return true;
        }

        public override void Spend(Action onSuccess, Action onFailure)
        {
#if YG_PLUGIN_YANDEX_GAME
            YandexGame.RewardVideoEvent += success;
            YandexGame.ErrorVideoEvent += fail;
            YandexGame.RewVideoShow(0);

            void success(int id)
            {
                YandexGame.RewardVideoEvent -= success;
                YandexGame.ErrorVideoEvent -= fail;
                onSuccess?.Invoke();
            }

            void fail()
            {
                YandexGame.RewardVideoEvent -= success;
                YandexGame.ErrorVideoEvent -= fail;
                onFailure?.Invoke();
            }
#else
            onSuccess?.Invoke();
#endif
        }
    }
}
