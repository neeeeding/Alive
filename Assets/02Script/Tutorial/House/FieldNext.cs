using System;
using UnityEngine;

namespace _02Script.Tutorial.House
{
    public class FieldNext : MonoBehaviour
    {
        [SerializeField] private GameObject field;
        [SerializeField] private HouseTutorial tutorial;

        private bool isFieldCheck;

        private void OnEnable()
        {
            isFieldCheck = false;
        }

        private void Update()
        {
            GaugeCheck();
        }

        private void FieldCheck()
        {
            //필드, 필드, 원, 유아이, 캔버스
            if(!field.transform.GetChild(0).transform.GetChild(0).gameObject.activeSelf)
            {
                tutorial.Next();
                gameObject.SetActive(false);
            }
        }

        private void GaugeCheck()
        {
            if (isFieldCheck)
            {
                FieldCheck();
                return;
            }
            //필드, 필드, 원, 유아이, 캔버스
            if(field.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject.activeSelf)
            {
                tutorial.Next();
                isFieldCheck = true;
            }
        }
    }
}