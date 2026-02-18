using TMPro;
using UnityEngine;

namespace _02Script.GoHouse.UI
{
    public class MoveCountUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;

        public void CountText(int cur, int max)
        {
            countText.text = cur + "/" + max;
        }
    }
}