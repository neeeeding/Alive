using _02Script.Item;
using _02Script.Obj.Character;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.UI.Likeability
{
    public class LikeItemItem : MonoBehaviour
    {
        [SerializeField] private CharacterSO loveCharacter; //아이템 종류
        [SerializeField] private ItemSO loveItem; //아이템 종류

        private string savePath; //저장 경로
        private Image itemImage; //아이템 이미지
        private int colorCount; //(클릭) 단계

        public void SettingItem(CharacterSO characterSO, ItemSO itemSO) //아이템 세팅 (누구인지, 어떤 아이템인지)
        {
            if (itemImage == null)
            {
                itemImage = GetComponent<Image>();
            }

            loveCharacter = characterSO;
            loveItem = itemSO;
            //itemImage.sprite = loveItem.itemImage;
            savePath = $"{loveCharacter}_{loveItem.itemType}likeItem";
            colorCount = PlayerPrefs.GetInt(savePath) - 1; //더해주니까.
            ChangeColorLove();
        }

        public void ChangeColorLove() //아이템 눌러 색 바꾸기
        {
            colorCount++;
            if (colorCount <= 0 || colorCount > 3)
            {
                colorCount = 1;
            }

            itemImage.color = colorCount switch
            {
                1 => Color.white,
                2 => Color.green,
                3 => Color.red,
                _ => Color.white
            };
            PlayerPrefs.SetInt(savePath, colorCount);
            PlayerPrefs.Save();
        }
    }
}