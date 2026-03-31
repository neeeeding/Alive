using _02Script.Produce.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.MiniGame.Produce
{
    public class ProduceMiniGame : MonoBehaviour
    {
        [SerializeField] private ProduceWindowActive window;
        [SerializeField] private Vector3 cursorOffset;
        [SerializeField] private Image stuffImage;
        [SerializeField] private GameObject mouseCursor;

        private void OnEnable()
        {
            //Setting();
            MiniGameTimer.OnEndMiniGame += StopGame;
        }

        private void OnDisable()
        {
            MiniGameTimer.OnEndMiniGame -= StopGame;
        }

        private void Update()
        {
            FollowCursor();
        }

        private void StopGame()
        {
            window.ResultBtn();
        }

        private void Setting()
        {
            stuffImage.sprite = SelectItemCard.curSelectItem.GetCurProduce(true).itemImage;
        }

        private void FollowCursor() // 나중에 클릭시 움직임 있도록 할 것
        {
            mouseCursor.transform.position = Input.mousePosition + cursorOffset;
        }
    }
}