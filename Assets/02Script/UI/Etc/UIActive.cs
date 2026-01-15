using UnityEngine;

namespace _02Script.UI.Etc
{
    public class UIActive : MonoBehaviour
    {
        [SerializeField] private GameObject ui;

        public void Show()
        {
            ui.SetActive(true);
        }
        public void Hide()
        {
            ui.SetActive(false);
        }
    }
}