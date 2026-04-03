using _02Script.Battle.Stage;
using _02Script.Manager;
using UnityEngine;

namespace _02Script.Battle
{
    public class BattleGoShow : MonoBehaviour
    {
        [SerializeField] private BattleStageSO[] stage;
        [SerializeField] private SceneChange monster;
        
        private readonly string SceneName = "PM_Battle";
        private readonly string saveStage = "battle_BattleStageSoSave";
        
        private void Update()
        {
            if (HouseManager.Instance.PlayerStat.hour >= 20 && !monster.gameObject.activeSelf)
            {
                monster.gameObject.SetActive(true);
            }
            else if (HouseManager.Instance.PlayerStat.hour == 23 && HouseManager.Instance.PlayerStat.minute > 58)
            {
                SceneBtn();
            }
            else if (HouseManager.Instance.PlayerStat.hour < 20 && monster.gameObject.activeSelf)
            {
                monster.gameObject.SetActive(false);
            }
        }

        public void SceneBtn()
        {
            SetStage();
            monster.SceneBtn(SceneName);
        }

        private void SetStage()
        {
            string json = JsonUtility.ToJson(stage[HouseManager.Instance.PlayerStat.day -1]);
            PlayerPrefs.SetString(saveStage, json);
            PlayerPrefs.Save();
        }
    }
}