using System.IO;
using _02Script.GamePlayer.Manager;
using _02Script.UI.Save;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace _02Script.Manager
{
    public class SceneChange : MonoBehaviour
    {
        private bool isSceneChange; //true : 씬 이동 가능 / false : 씬 이동 불가

        private void Awake()
        {
            //LoadScene();
            LoadCard.OnLoad += LoadScene;
        }

        private void OnDisable()
        {
            LoadCard.OnLoad -= LoadScene;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == HousePlayerManager.curPlayer.gameObject)
            {
                isSceneChange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (HousePlayerManager.curPlayer.gameObject != null &&collision.gameObject == HousePlayerManager.curPlayer.gameObject)
            {
                isSceneChange = false;
            }
        }

        private void LoadScene() //씬 로드
        {
            // string scene = HouseManager.Instance.PlayerStat.sceneName;
            // //print($"cur : {scene} / save {GameManager.Instance.PlayerStat.sceneName}");
            //
            // if (scene != HouseManager.Instance.curScene) //현재 씬과 이동해야하는 씬이 안 맞다면
            // {
            //     isSceneChange = true;
            //     SceneBtn(scene);
            // }
        }

        public void SceneBtn(int i) //해당 씬으로
        {
            if (isSceneChange)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                HouseManager.Instance.PlayerStat.sceneName = sceneName;
                SceneManager.LoadScene(i);
            }
        }

        public void SceneBtn(string name) //해당 씬으로
        {
            if (isSceneChange)
            {
                HouseManager.Instance.PlayerStat.sceneName = name;
                SceneManager.LoadScene(name);
            }
        }

        public void QuitBtn() //끝내기
        {
            Application.Quit();
        }
    }
}
