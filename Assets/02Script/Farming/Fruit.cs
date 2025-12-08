using System.Threading.Tasks;
using UnityEngine;

namespace _02Script.Farming
{
    //나중에 아껴 쓰기 & 저장하기
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private SeedsSO mySO;
        private SpriteRenderer mySpriteRenderer;

        public void ClickFruit()
        {
            Destroy(gameObject);
        }
        
        public void SetSO(SeedsSO seedsSO)
        {
            mySO = seedsSO;
        }
        
        private void OnEnable()
        {
            if(mySpriteRenderer == null)
                mySpriteRenderer = GetComponent<SpriteRenderer>();

            mySpriteRenderer.sprite = mySO.fruit.itemImage;
        }
    }
}