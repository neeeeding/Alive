using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace _02Script.UI.Dialog.Dialog
{
    //인벤토리가 있어야 함.
    public class DialogItem : MonoBehaviour
    {
        // public bool HaveItem(int i,
        //     List<Dictionary<string,string>> dialog,
        //     SerializedDictionary<string, ItemDataSO> items,
        //     InventoryManager manager) //아이템
        // {
        //     if (dialog[i][DialogType.Item.ToString()] == "") //아이템 있는지
        //         return true;
        //
        //     string[] c = dialog[i][DialogType.ItemCount.ToString()].Split('~');
        //     int[] count = new int[c.Length];
        //     for (int j = 0; j < c.Length; j++)
        //     {
        //         count[j] = int.Parse(c[j]);
        //     }
        //     string[] itemNames = dialog[i][DialogType.Item.ToString()].Split('~');
        //
        //     for (int j = 0; j < itemNames.Length; j++)
        //     {
        //         if (!items.ContainsKey(itemNames[j])) return false;
        //         ItemDataSO myItem = items[itemNames[j]];
        //
        //         if (!items.ContainsKey(itemNames[j])) //아이템이 없음
        //         {
        //             print(itemNames[j]);
        //             return false;
        //         }
        //         
        //         if (count[j] > 0)
        //         {
        //             manager.AddItem(myItem, count[j]);
        //         }
        //         else
        //         {
        //             count[j] *= -1;
        //             manager.UseItem(myItem, count[j]);
        //         }
        //     }
        //     return true;
        // }
    }
}