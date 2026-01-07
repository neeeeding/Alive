using System;
using _02Script.Inventory.Item;

namespace _02Script.Produce
{
    public class ProduceBookCard : ItemCard
    {
        /**이름 뜨기 더 나아가 관련 책 띄어주기??*/
        public static event Action<ItemDataSO> OnMouseCursor;
        public static event Action<ProduceBookSO> OnMouseClick;
        
        private ProduceBookSO _bookData;

        #region Btn
        public void MouseEnter()
        {
            OnMouseCursor?.Invoke(itemData.ReturnDataSO());
        }        
        public void MouseExit()
        {
            OnMouseCursor?.Invoke(null);
        }
        public void SelectBook()
        {
            if(_bookData == null) return; //책이 아님
            OnMouseClick?.Invoke(_bookData);
        }
        #endregion

        public override void NewCard(ItemData itemData)
        {
            _bookData = null;
            //if (itemData.ReturnDataSO().itemType == ItemType.Book) //책
            {
                _bookData = itemData.ReturnDataSO() as ProduceBookSO;
                //return;
            }
            base.NewCard(itemData); //부산물
        }
    }
}