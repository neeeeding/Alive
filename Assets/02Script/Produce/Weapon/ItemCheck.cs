using _02Script.Battle.Buff;
using _02Script.Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Produce.Weapon
{
    public class ItemCheck : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemType;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemExplanation;
        [SerializeField] private Image buffImage;
        [SerializeField] private TextMeshProUGUI buffName;
        [SerializeField] private TextMeshProUGUI buffExplanation;

        [SerializeField] private ProduceWindowActive window;
        private void OnEnable()
        {
            SelectItemCard.OnMouseUp += CheckProduce;
        }

        private void OnDisable()
        {
            SelectItemCard.OnMouseUp -= CheckProduce;
        }

        private void CheckProduce(SelectProduceType produceType)
        {
            if (SelectItemCard.curSelectItem == null) return;
            
            window.Check();
            Setting();
        }

        private void Setting()
        {
            ItemDataSO item = SelectItemCard.curSelectItem.GetCurProduce(false);

            itemImage.sprite = item.itemImage;
            itemName.text = item.itemName;
            itemExplanation.text = item.itemExplanation;

            BuffSO buff = null;
            if (item is WeaponItemDataSO weapon)
            {
                itemType.text = "무기";
                buff = weapon.skillBuff;
            }
            else if (item is ArmorItemDataSO armor)
            {
                itemType.text = "갑옷";
                buff = armor.skillBuff;
            }
            
            if(buff != null)
                buffImage.sprite = buff.buffImage;
            else
                buffImage.gameObject.SetActive(false);
            buffName.text = buff ? buff.buffName : "";
            buffExplanation.text = buff ? buff.buffExplanation : "";
        }
    }
}