using System;
using UnityEngine;

namespace _02Script.Inventory.Item
{
    public class GetItem : MonoBehaviour
    {
        public static Action<ItemDataSO> OnGetItem;
    }
}