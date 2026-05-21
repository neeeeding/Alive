using System;
using UnityEngine;

namespace _02Script.Tutorial.House
{
    public class HouseTutorial : MonoBehaviour
    {
        [SerializeField] private GameObject tutorial;
        [SerializeField] private GameObject guide;
        
        
        private void Awake()
        {
            tutorial.SetActive(true); //처음부터 보여줘야 하니까
        }
    }
}