using _02Script.Battle;
using _02Script.GoHouse.Etc;
using _02Script.Manager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace _02Script.Etc
{
    public class StartAudio :  MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        private void Start()
        {
            float sound = 0;
            if (SceneManager.GetActiveScene().name.Contains("AM"))
            {
                sound = HouseManager.Instance.saveData.sound.mainSound;
            }
            else if (SceneManager.GetActiveScene().name.Contains("PM"))
            {
                sound = BattleSaveManager.Instance.saveData.sound.mainSound;
            }
            else if (SceneManager.GetActiveScene().name.Contains("GO"))
            {
                sound = GoHouseSaveManager.Instance.saveData.sound.mainSound;
            }
            else
            {
                sound = 0.2f;
            }
            audioMixer.SetFloat("Master", Mathf.Log10(sound)*20);
        }
    }
}