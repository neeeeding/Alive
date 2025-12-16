using UnityEngine;

namespace _02Script.Farming
{
    public class Seeds : MonoBehaviour
    {
        private SpriteRenderer mySpriteRenderer;

        public void SetSO(SeedsSO seedsSO)
        {
            if(mySpriteRenderer == null)
                mySpriteRenderer = GetComponent<SpriteRenderer>();
            
            mySpriteRenderer.sprite = seedsSO.seeds.itemImage;
        }
    }
}