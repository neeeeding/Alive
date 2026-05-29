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
            if (SceneManager.GetActiveScene().name.Contains(_am))
                return HouseManager.Instance.PlayerStat;
            if (SceneManager.GetActiveScene().name == _mini)
                return GoHouseSaveManager.Instance.PlayerStat;
            if (SceneManager.GetActiveScene().name.Contains(_pm))
                return BattleSaveManager.Instance.PlayerStat;
            
            return null;
        }
    }
}