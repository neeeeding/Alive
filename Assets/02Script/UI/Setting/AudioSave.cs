using _02Script.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.UI.Setting
{
    public class AudioSave : MonoBehaviour
    {
        [SerializeField] private Slider main;
        [SerializeField] private Slider bgm;
        [SerializeField] private Slider effect;

        private void Awake()
        {
            main.value = GameManager.Instance.saveData.sound.mainSound;
            bgm.value = GameManager.Instance.saveData.sound.bgmSound;
            effect.value = GameManager.Instance.saveData.sound.effectSound;
        }

        public void ChangeMain()
        {
            if (GameManager.Instance.isStart)
            {
                GameManager.Instance.saveData.sound.mainSound = main.value;
            }
        }

        public void ChangeBGM()
        {
            if (GameManager.Instance.isStart)
            {
                GameManager.Instance.saveData.sound.bgmSound = bgm.value;
            }
        }

        public void ChangeEffect()
        {
            if (GameManager.Instance.isStart)
            {
                GameManager.Instance.saveData.sound.effectSound = effect.value;
            }
        }
    }
}