using System;
using UnityEngine;
using DG.Tweening;

namespace _02Script.DotweenUI.Popup
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private float delay = 1f;
        private void OnEnable()
        {
            gameObject.transform.localScale = Vector3.one * 0.2f;
            gameObject.transform.DOScale(Vector3.one, delay).SetEase(Ease.OutElastic).SetUpdate(true);
        }

        private void OnDisable()
        {
            gameObject.transform.localScale = Vector3.one * 0.2f;
        }
    }
}