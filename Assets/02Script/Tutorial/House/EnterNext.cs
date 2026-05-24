using System;
using UnityEngine;

namespace _02Script.Tutorial.House
{
    public class EnterNext : MonoBehaviour
    {
        public bool isShow;
        public bool isHide;
        public bool isClick;
        [SerializeField] private HouseTutorial tutorial;

        private bool _isGet;

        private void OnEnable()
        {
            _isGet =false;
            if (isShow)
            {
                tutorial.Next();
                Destroy(this);
            }
        }
        private void OnDisable()
        {
            if (isHide)
            {
                tutorial.Next();
                Destroy(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isGet && !isClick && !isShow && other.CompareTag("Player"))
            {
                _isGet = true;
                Next();
            }
        }

        public void Next()
        {
            tutorial.Next();
            gameObject.SetActive(false);
        }
    }
}