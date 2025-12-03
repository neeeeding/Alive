using _02Script.Manager;
using UnityEngine;

namespace _02Script.Obj.Obj
{
    public class ObjEnter : MonoBehaviour
    {
        private bool isObj;

        private void Awake()
        {
            isObj = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isObj = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isObj = false;
            }
        }

        public void ClickStore()
        {
            if(isObj)
            {
                UISettingManager.Instance.Obj();
            }
        }
    }
}
