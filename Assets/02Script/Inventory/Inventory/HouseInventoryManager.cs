using _02Script.Produce.Weapon;

namespace _02Script.Inventory.Inventory
{
    public class HouseInventoryManager : LoadInventoryManager
    {
        protected override void OnEnable()
        {
            Result.OnGetItem += AddItem;
            Result.OnUseItem += ThrowItem;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            Result.OnGetItem -= AddItem;
            Result.OnUseItem -= ThrowItem;
            base.OnDisable();
        }
    }
}