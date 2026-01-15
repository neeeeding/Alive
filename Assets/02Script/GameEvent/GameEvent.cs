using System;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.Obj.Obj;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.GameEvent
{
    public class GameEvent : MonoBehaviour
    {
        public static Action<DoUIType> OnLockUI;

        [SerializeField] protected DoUIType[] lockType;
        [SerializeField] protected ItemDataSO[] itemDataSos;

        protected void Update()
        {
            if (GameManager.Instance.PlayerStat.hour >= 20)
            {
                DoEvent();
            }
        }

        protected void DoEvent()
        {
            OnLockUI?.Invoke(lockType[Random.Range(0, lockType.Length)]);
        }
    }
}