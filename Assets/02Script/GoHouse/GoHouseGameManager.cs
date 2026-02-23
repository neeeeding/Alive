using _02Script.Etc;
using _02Script.GoHouse.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02Script.GoHouse
{
    public class GoHouseGameManager : MonoBehaviour
    {
        [SerializeField] private GameObject successWindow; //이 윈도우도 뭐 있기?? (주석)
        [SerializeField] private GameObject battleWindow;
        #region EnDiAw
        private void OnEnable()
        {
            successWindow.SetActive(false);
            battleWindow.SetActive(false);
            HouseSO.OnSuccess += Success;

        }
        private void OnDisable()
        {
            HouseSO.OnSuccess -= Success;   
        }
        #endregion

        #region BlockAction
        private async void Success(string sceneName, BlockActionSO so)
        {
            if (so as BattleSO != null)
                battleWindow.SetActive(true);
            else
                successWindow.SetActive(true);
            await AsyncTime.WaitSeconds(1);
            SceneManager.LoadScene(sceneName);
        }
        #endregion
    }
}