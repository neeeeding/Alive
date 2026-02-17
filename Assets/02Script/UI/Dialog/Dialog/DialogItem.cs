using System;
using System.Collections.Generic;
using _02Script.Inventory.Item;
using _02Script.Manager;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.UI.Dialog.Dialog
{
    public class DialogItem : MonoBehaviour
    {
        public static Action<ItemDataSO, int> OnGetItem;
        public void GetOrThrowItem(Dictionary<string,string> dialog,
            SerializedDictionary<ItemType, ItemDataSO> items) //아이템 얻기
        {
            if (dialog[DialogType.GetItem.ToString()] == "") //아이템 있는지
                return;
        
            string[] allCounts = dialog[DialogType.ItemCount.ToString()].Split('~');
            int[] count = new int[allCounts.Length];
            for (int j = 0; j < allCounts.Length; j++)
            {
                count[j] = int.Parse(allCounts[j]);
            }
            string[] itemNames = dialog[DialogType.GetItem.ToString()].Split('~');
        
            for (int j = 0; j < itemNames.Length; j++)
            {
                ItemType type = (ItemType)Enum.Parse(typeof(ItemType), itemNames[j]); //스트링을 enum 값으로
                if (!items.ContainsKey(type)) return;
                ItemDataSO myItem = items[type];
                
                OnGetItem?.Invoke(myItem, count[j]);
            }
        }
        
        public int? IsHoldItem(List<Dictionary<string, string>> dialog) //들고 있는 아이템 있다면 (챕터 번호)
        {
            int value = 0;
            ItemDataSO dataSo = HouseManager.Instance.holdItemData;
            if (dataSo != null)
            {
                for (int i = 0; i < dialog.Count - 1; i++)
                {
                    if (dialog[i][DialogType.Item.ToString()].ToLower() ==
                        dataSo.itemType.ToString().ToLower()) //대화의 아이템 창과 들고 있는 아이템 찾기
                    {
                        value = int.Parse(dialog[i][DialogType.Chapter.ToString()]);
                        
                        OnGetItem?.Invoke(dataSo, 1);//아이템 빼기 (늘 1만큼 사용하기? (주석)
                        return value;
                    }
                } //여기서 못 찾으면 경고 보내야 하나? (주석)
            }
            return null;
        }
    }
}