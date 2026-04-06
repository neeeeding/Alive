using System;
using _02Script.Manager;
using UnityEngine;

namespace _02Script.Farming
{
    public class FarmLoad : MonoBehaviour
    {
        [SerializeField] private GameObject farm;

        private void OnEnable()
        {
            HouseManager.OnStart += Load;
    
            if(HouseManager.Instance.isStart)
            {
                Load();
            }
        }

        private void OnDisable()
        {
            HouseManager.OnStart -= Load;
        }

        private void Load()
        {
            farm.SetActive(true);
            farm.SetActive(false);
        }
    }
}