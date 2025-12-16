using UnityEngine;

namespace _02Script.Farming
{
    public class Seeds : MonoBehaviour
    {
        private SpriteRenderer mySpriteRenderer;
        
        [SerializeField] private SeedsSO mySO;

        public void SetSO(SeedsSO seedsSO)
        {
            mySO = seedsSO;
            
            if(mySpriteRenderer == null)
                mySpriteRenderer = GetComponent<SpriteRenderer>();
            
            mySpriteRenderer.sprite = mySO.seeds.itemImage;
        }
    }
}