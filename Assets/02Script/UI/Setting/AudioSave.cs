using _02Script.Manager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _02Script.UI.Setting
{
    public class AudioSave : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider main;
        [SerializeField] private Slider bgm;
        [SerializeField] private Slider effect;

        private void Awake()
        {
            main.value = HouseManager.Instance.saveData.sound.mainSound;
            bgm.value = HouseManager.Instance.saveData.sound.bgmSound;
            effect.value = HouseManager.Instance.saveData.sound.effectSound;
        }

        public void ChangeMain()
        {
            if (HouseManager.Instance.isStart)
            {
                HouseManager.Instance.saveData.sound.mainSound = main.value;
                audioMixer.SetFloat("Master", Mathf.Log10(main.value)*20);
            }
        }

        public void ChangeBGM()
        {
            if (HouseManager.Instance.isStart)
            {
                HouseManager.Instance.saveData.sound.bgmSound = bgm.value;
            }
        }

        public void ChangeEffect()
        {
            if (HouseManager.Instance.isStart)
            {
                HouseManager.Instance.saveData.sound.effectSound = effect.value;
            }
        }
    }
}