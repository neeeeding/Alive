using System.Threading.Tasks;
using UnityEngine;

namespace _02Script.Farming
{
    //나중에 아껴 쓰기 & 저장하기
    public class Fruit : MonoBehaviour
    {
        private SpriteRenderer mySpriteRenderer;
        private OneFarming myP;

        public void ClickFruit()
        {
            myP.ListSeeds();
            //얻기
            gameObject.SetActive(false);
        }
        
        public void SetSO(SeedsSO seedsSO, OneFarming f)
        {
            myP = f;
            if(mySpriteRenderer == null)
                mySpriteRenderer = GetComponent<SpriteRenderer>();

            mySpriteRenderer.sprite = seedsSO.fruit.itemImage;
        }
    }
}