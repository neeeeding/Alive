using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Farming
{
    public class SeedsCard : MonoBehaviour
    {
        public static Action<SeedsSO> OnClickCard;
        
        [SerializeField] private SeedsSO mySO;

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;

        public void ClickCard()
        {
            OnClickCard?.Invoke(mySO);
        }
        
        public void SetSO(SeedsSO seedsSO)
        {
            mySO = seedsSO;

            image.sprite = mySO.seeds.itemImage;
            text.text = mySO.seeds.itemName;
        }
    }
}