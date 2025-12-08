using System;
using _02Script.Etc;
using _02Script.Item;
using UnityEngine;

namespace _02Script.Farming
{
    public class SeedsWindow : MonoBehaviour
    {
        [SerializeField] private GameObject window;
        [SerializeField] private SeedsCard sellCard; //판매 카드
        [SerializeField] private SeedsSO[] allSeedsSOs;

        public void CloseBtn()
        {
            window.SetActive(false);
        }

        private void OnEnable()
        {
            SeedsCard.OnClickCard += (so => CloseBtn());
            if (gameObject.transform.childCount <= 0)
            {
                SetCards();
            }
        }

        private void SetCards()
        {
            foreach (SeedsSO so in allSeedsSOs)
            {
                SeedsCard newCard = Instantiate(sellCard);
                newCard.SetSO(so);
                newCard.gameObject.SetActive(true);
                newCard.transform.SetParent(transform);
            }
        }
    }
}