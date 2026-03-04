using _02Script.Battle.Entity;
using _02Script.Battle.Monster;
using _02Script.Etc;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _02Script.Battle
{
    public class GameEnd : MonoBehaviour
    {
        [SerializeField] private GameObject successWindow;
        [SerializeField] private GameObject failWindow;
        [SerializeField] private Image bloodWindow;
        
        private readonly string _goHouse = "GoHouse";
        private readonly string _failScene = "AM_House";

        private void OnEnable()
        {
            MonsterManager.OnSuccess += Success;
            BattleCharacter.OnDie += Fail;
            BattleCharacter.OnBlood += Blood;
            
            successWindow.SetActive(false);
            failWindow.SetActive(false);
            Blood(false);
        }

        private void OnDisable()
        {
            MonsterManager.OnSuccess -= Success;
            BattleCharacter.OnDie -= Fail;
            BattleCharacter.OnBlood -= Blood;
        }

        private async void Success()
        {
            successWindow.SetActive(true);
            await AsyncTime.WaitSeconds(2);
            SceneManager.LoadScene(_goHouse);
        }

        private async void Fail()
        {
            failWindow.SetActive(true);
            Time.timeScale = 0;
            await AsyncTime.WaitSeconds(1, true);
            SceneManager.LoadScene(_failScene);
        }

        private void Blood(bool isBlood)
        {
            float fade = isBlood ? 0.2f : 0f;
            bloodWindow.DOFade(fade, 1);
        }
    }
}