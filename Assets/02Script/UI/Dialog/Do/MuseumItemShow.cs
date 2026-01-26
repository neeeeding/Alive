using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Etc;
using UnityEngine;

namespace _02Script.UI.Dialog.Do
{
    //샘플
    public class MuseumItemShow : MonoBehaviour,IDialogCanScript
    {
        [SerializeField] private GameObject[] items;

        private bool isItemShow;

        private void Awake()
        {
            isItemShow = false;
            foreach (GameObject item in items)
            {
                item.gameObject.SetActive(false);
            }
        }

        public void Do(DoScriptType type)
        {
            isItemShow = true;
            foreach (GameObject item in items)
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}