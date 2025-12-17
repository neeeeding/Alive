using UnityEngine;

namespace JYE._01Script.Inventory.Item
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "SO/JYE/Item/ItemDataSO")]
    public class ItemDataSO : ScriptableObject
    {
        public Sprite itemImage;
        
        public string itemName;
        public int maxCount;
        
        /**true : 아이템 / false : 부산물 */
        public bool isItem; 
        
        public ItemType itemType;
        
        /**아이템 설명*/
        public string itemExplanation;
        
        //**특정 개수 만큼 모아야 되는지*/
        public int mustItemCount;
        
        /**특성 알맞는 것으로 변경할 것.*/
        public ItemDataSO haveCharacteristic;

        public virtual bool DoSomething() //보통은 그냥 사용 못하게 (환단만 사용 가능)
        {
            return false;
        }
    }

    /**부산물은 10000*/
    public enum ItemType
    {
        Nothing = 0,
        None = 1, //보통
        Hp = 1000, // 포션? 류
        Pet = 2000, //펫 관련
        Armor = 3000, //무기나 갑옷
        Book = 4000, //책
        
        Hoebinghwan = 9000, //회빙환 관련
        
    }
}