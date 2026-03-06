using System;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using UnityEngine;

namespace _02Script.Collect
{
    public class DeleteBtn : MonoBehaviour
    {
        public static event Action<EntityName,ItemDataSO,float> OnDelete;

        [SerializeField] private EntityName inventoryName;
        [SerializeField] private GameObject btn;

        private EntityName _name;
        private ItemDataSO _so;
        private float _count;
        private void OnEnable()
        {
            Cancel();
            CollectInventoryCard.OnMouseClick += Select;   
        }

        private void OnDisable()
        {
            CollectInventoryCard.OnMouseClick -= Select;
        }

        public void Delete()
        {
            OnDelete?.Invoke(_name,_so, _count);
        }

        private void Select(CollectInventoryCard card,EntityName name, ItemDataSO so, float count)
        {
            if(name != inventoryName) return;
            
            btn.SetActive(true);

            _name = name;
            _so = so;
            _count = count;
        }

        private void Cancel()
        {
            btn.SetActive(false);
        }

        public void SetCharacter(EntityName name)
        {
            inventoryName = name;
        }
    }
}