using UnityEngine;

namespace _02Script.Inventory.Inventory
{
    public class InventoryUI :MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] private InventoryInput input;
        [SerializeField] private GameObject inventory;

        #region EnDi
        private void OnEnable()
        {
            input.OnIBtn += Inventory;
            inventory.SetActive(false);
        }
        private void OnDisable()
        {
            input.OnIBtn -= Inventory;
        }
        #endregion

        public void Inventory()
        {
            inventory.SetActive(!inventory.activeSelf);
        }
    }
}