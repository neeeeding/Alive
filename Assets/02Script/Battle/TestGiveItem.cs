using _02Script.Battle.UI.Job;
using _02Script.Inventory.Etc;

namespace _02Script.Battle
{
    public class TestGiveItem : StartGiveItem
    {
        private void OnEnable()
        {
            SelectDistribution.OnStart += Set;
        }

        private void OnDisable()
        {
            SelectDistribution.OnStart -= Set;
        }
    }
}