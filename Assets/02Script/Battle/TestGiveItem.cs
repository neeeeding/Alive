using _02Script.Inventory.Etc;

namespace _02Script.Battle
{
    public class TestGiveItem : StartGiveItem
    {
        private void OnEnable()
        {
            BattleSaveManager.OnStart += Set;
        }

        private void OnDisable()
        {
            BattleSaveManager.OnStart -= Set;
        }
    }
}