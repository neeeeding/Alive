using UnityEngine;

namespace JYE._01Script.Inventory.Item.Characteristic
{
    [CreateAssetMenu(fileName = "Characteristic", menuName = "SO/JYE/Item/CharacteristicSO")]
    public class CharacteristicSO : ItemDataSO
    {
        public override bool DoSomething()
        {
            //나중에 능력 실행
            return true;
        }
    }
}