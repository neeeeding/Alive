using UnityEngine;
using UnityEngine.Serialization;

namespace _02Script.Produce.Weapon
{
    public class SelectProduceCard : MonoBehaviour
    {
        [SerializeField] private SelectProduceType produce;

        private bool _isEnter;

        private void OnEnable()
        {
            _isEnter = false;
        }

        public void MouseClick()
        {
            if (_isEnter) return;
            _isEnter = true;
            if (SelectItemCard.curSelectItem != null)
            {
                SelectItemCard.curSelectItem.SetProduce(produce);
            }
        }

        public void MouseCancel()
        {
            if (!_isEnter) return;
            _isEnter = false;
            if (SelectItemCard.curSelectItem != null)
            {
                SelectItemCard.curSelectItem.SetProduce(SelectProduceType.None);
            }
        }
    }
}