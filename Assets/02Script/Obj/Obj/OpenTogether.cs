using UnityEngine;

namespace _02Script.Obj.Obj
{
    public class OpenTogether : MonoBehaviour
    {
        [SerializeField] protected GameObject obj;

        private void Awake()
        {
            obj.SetActive(false);
        }

        private void OnEnable()
        {
            obj.SetActive(true);
        }

        private void OnDisable()
        {
            if(obj)
                obj.SetActive(false);
        }
    }
}