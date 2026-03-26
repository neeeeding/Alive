using _02Script.Produce.Weapon;
using _02Script.Produce.Weapon.Compound;

namespace _02Script.Inventory.Inventory
{
    public class HouseInventoryManager : LoadInventoryManager
    {
        protected override void OnEnable()
        {
            ProduceResult.OnGetItem += AddItem;
            ProduceResult.OnUseItem += ThrowItem;
            CompoundResult.OnGetItem += AddItem;
            CompoundResult.OnUseItem += ThrowItem;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            ProduceResult.OnGetItem -= AddItem;
            ProduceResult.OnUseItem -= ThrowItem;
            CompoundResult.OnGetItem -= AddItem;
            CompoundResult.OnUseItem -= ThrowItem;
            base.OnDisable();
        }
    }
}