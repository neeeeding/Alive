using _02Script.Battle;
using _02Script.GoHouse.Etc;
using _02Script.Manager;
using _02Script.SaveData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02Script.Etc
{
    public class SaveManagerCheck : MonoBehaviour
    {
        private static readonly string _am = "House";
        private static readonly string _pm = "Battle";
        private static readonly string _mini = "GoHouse";
        
        public static PlayerStatSC GetCurScenePlayerStat()
        {
            string sceneName = SceneManager.GetActiveScene().name;

            // [수정] "GoHouse"가 "House"보다 먼저 검사되도록 순서 변경 ("GoHouse"가 "House"에 걸리는 버그 해결)
            if (sceneName.Contains(_mini))
                return GoHouseSaveManager.Instance.PlayerStat;
            if (sceneName.Contains(_pm))
                return BattleSaveManager.Instance.PlayerStat;
            if (sceneName.Contains(_am))
                return HouseManager.Instance.PlayerStat;
            
            return null;
        }
    }
}
