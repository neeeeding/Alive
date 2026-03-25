using UnityEngine;

namespace _02Script.Produce.Weapon
{
    public class ProduceWindowActive : MonoBehaviour
    {
        [SerializeField] private RectTransform[] inventory;
        [SerializeField] private GameObject selectWindow;
        [SerializeField] private GameObject checkWindow;
        [SerializeField] private GameObject makeWindow;
        [SerializeField] private GameObject resultWindow;

        private void OnEnable()
        {
            Select();
        }

        public void Select()
        {
            foreach (RectTransform i in inventory)
            {
                i.anchoredPosition = Vector2.zero;
            }
            selectWindow.SetActive(true);
            checkWindow.SetActive(false);
            makeWindow.SetActive(false);
            resultWindow.SetActive(false);
        }
        
        public void Check()
        {
            selectWindow.SetActive(false);
            checkWindow.SetActive(true);
        }

        public void AgainSelectMake()
        {
            foreach (RectTransform i in inventory)
            {
                i.anchoredPosition = Vector2.zero;
            }
            SelectItemCard.curSelectItem = null;
            selectWindow.SetActive(true);
            checkWindow.SetActive(false);
        }

        public void MakeBtn()
        {
            checkWindow.SetActive(false);
            makeWindow.SetActive(true);
        }

        public void ResultBtn()
        {
            checkWindow.SetActive(false);
            makeWindow.SetActive(false);
            resultWindow.SetActive(true);
        }
    }
}