using System;
using _02Script.Etc;
using UnityEngine;

namespace _02Script.Farming
{
    public class Viand : MonoBehaviour
    {
        [SerializeField] private EffectShow farmEffect;
        private SpriteRenderer mySpriteRenderer;
        private OneFarming myP;

        private void OnEnable()
        {
            farmEffect.ShowEffect();
        }

        private void OnDisable()
        {
            farmEffect.HideEffect();
        }

        public void ClickViand() //농작물 얻기
        {
            myP.ListSeeds();
            gameObject.SetActive(false);
        }
        
        public void SetSO(SeedsSO seedsSO, OneFarming f)
        {
            myP = f;
            if(mySpriteRenderer == null)
                mySpriteRenderer = GetComponent<SpriteRenderer>();

            mySpriteRenderer.sprite = seedsSO.viand.itemImage;
        }
    }
}