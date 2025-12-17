using UnityEngine;

namespace JYE._01Script.Inventory.Inventory
{
    public class InventoryUI :MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] private InventoryInput input;
        [SerializeField] private GameObject inventory;

        #region EnDi
        private void OnEnable()
        {
            input.OnTab += Inventory;
            inventory.SetActive(false);
        }
        private void OnDisable()
        {
            input.OnTab -= Inventory;
        }
        #endregion

        private void Inventory()
        {
            inventory.SetActive(!inventory.activeSelf);
        }
    }
}