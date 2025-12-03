using UnityEngine;


namespace _02Script.Etc
{
    public class DontDelete : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
