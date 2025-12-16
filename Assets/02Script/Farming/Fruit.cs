using System.Threading.Tasks;
using UnityEngine;

namespace _02Script.Farming
{
    //나중에 아껴 쓰기 & 저장하기
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private SeedsSO mySO;
        private SpriteRenderer mySpriteRenderer;
        private OneFarming myP;

        public void ClickFruit()
        {
            gameObject.SetActive(false);
            myP.ListSeeds();
            //얻기
        }
        
        public void SetSO(SeedsSO seedsSO, OneFarming f)
        {
            mySO = seedsSO;
            myP = f;
        }
        
        private void OnEnable()
        {
            if(mySpriteRenderer == null)
                mySpriteRenderer = GetComponent<SpriteRenderer>();

            mySpriteRenderer.sprite = mySO.fruit.itemImage;
        }
    }
}