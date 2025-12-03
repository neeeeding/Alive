using _02Script.Item;
using _02Script.Obj.Character;
using UnityEngine;


namespace _02Script.UI.Likeability
{
    public class LikeItemManager : MonoBehaviour
    {
        [Header("Items")] [SerializeField] private ItemSO[] loveItems; //만들어줘야 할 아이템들

        [Space(30f)] [Header("Prefabs Need")] [SerializeField]
        private GameObject Item; //오브젝트

        private CharacterSO character; //현재 캐릭터 정보

        public void Setting(CharacterSO so) //세팅
        {
            character = so;
            SettingItem();
        }

        private void MakeItem() //아이템들 생성
        {
            for (int i = 0; i < loveItems.Length; i++)
            {
                GameObject loveItem = Instantiate(Item, transform);
                loveItem.SetActive(true);
            }
        }

        private void SettingItem() //아이템 세팅
        {
            if (transform.childCount == 0)
            {
                MakeItem();
            }

            for (int i = 0; i < loveItems.Length; i++)
            {
                GameObject loveItem = transform.GetChild(i).gameObject;
                LikeItemItem itemSc = loveItem.GetComponent<LikeItemItem>();
                itemSc.SettingItem(character, loveItems[i]);
            }
        }
    }
}