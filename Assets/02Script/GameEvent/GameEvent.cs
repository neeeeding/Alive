using System;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.GameEvent
{
    public class GameEvent : MonoBehaviour
    {
        public static Action<ItemDataSO, int> OnGetItem;

        [SerializeField] protected ItemDataSO[] itemDataSos;

        protected int CurrItem;
        protected bool isLock;

        private void Awake()
        {
            CurrItem = -1;
        }

        private void OnEnable()
        {
            isLock = false;
        }

        public virtual void GetItem()
        {
            if(isLock) return;
            isLock = true;
            CurrItem++;
            OnGetItem?.Invoke(itemDataSos[CurrItem], 1);
        }
    }
}