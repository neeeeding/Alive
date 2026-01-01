using System;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.UI.Map
{
    public class MapActiveBtn : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite notActiveSprite;
        [Header("Need")]
        [SerializeField] private Image activeBtn;
        [SerializeField] private GameObject parentObject;

        private string activeSave = "MapActiveSave";
        private bool isActive;

        private void Awake()
        {
            isActive = PlayerPrefs.GetInt(activeSave) == 0;
            SetActive();
        }

        public void SetActive()
        {
            isActive = !isActive;
            
            activeBtn.sprite = isActive ? activeSprite : notActiveSprite;
            PlayerPrefs.SetInt(activeSave, isActive ? 1 : 0);
            
            foreach (Transform child in parentObject.transform)
            {
                child.gameObject.SetActive(isActive);
            }
        }
    }
}