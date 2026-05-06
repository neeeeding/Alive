using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02Script.Title
{
    public class StartBtn : MonoBehaviour
    {
        [SerializeField] private GameObject goStartList;
        [SerializeField] private string startScene = "AM_House";

        public void StartGame()
        {
            SceneManager.LoadScene(startScene);
        }

        public void GoStartBtn()
        {
            goStartList.SetActive(!goStartList.activeSelf);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}