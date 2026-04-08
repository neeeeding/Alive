using System;
using _02Script.Etc;
using _02Script.Inventory.Item;
using TMPro;
using UnityEngine;

namespace _02Script.Farming
{
    public class SeedsCard : ItemCard
    {
        public static Action<SeedsSO> OnClickCard;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI temperatureTypeText;
        
        [SerializeField] private SeedsSO mySO;

        public void ClickCard()
        {
            OnClickCard?.Invoke(mySO);
        }

        public void NewCard(SeedsSO seedsSO, ItemData seedsData)
        {
            mySO = seedsSO;
            itemData = seedsData;

            cardImage.sprite = mySO.seeds.itemImage;
            nameText.text = mySO.seeds.itemName;
            
            //온도 속성
            string temperatureType = "";
            foreach (TemperatureType type in Enum.GetValues(typeof(TemperatureType)))
            {
                if(type == TemperatureType.none) continue;
                if ((seedsSO.temperatureType & type) != 0)
                {
                    if (temperatureType != "") temperatureType += ", ";
                    temperatureType += EnumToString.Name(type);
                }
            }
            temperatureTypeText.text = temperatureType;
        }
    }
}