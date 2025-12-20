using UnityEngine;

namespace _02Script.Inventory.Item
{
    //max 넘으면 아이템 더 못 얻기, f로 아이템 얻기(UI 뜨기?)
    public class InGameItem : GetItem
    {
        
        [Header("Setting")]
        [SerializeField] private ItemDataSO itemData;
        [SerializeField] private float radius;
        [SerializeField] private LayerMask player;
        
        private Collider[]  _hits = new Collider[10];

        private void Update()
        {
            int check = Physics.OverlapSphereNonAlloc(transform.position,radius,_hits, player );
            
            if(check <= 0) return;
            
            OnGetItem?.Invoke(itemData);
            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}