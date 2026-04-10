using System;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.UI.Person
{
    public class PersonWeaponCard : ItemCard
    {
        public static Action<PersonWeaponCard,Vector3> OnMouseEnter; //정보, 현재 남은 시간 
        
        public void MouseEnter()
        {
            OnMouseEnter?.Invoke(this,gameObject.transform.position);
        }        
        public void MouseExit()
        {
            OnMouseEnter?.Invoke(null,Vector3.zero);
        }

        public override void UpdateCountUI()
        {
        }
    }
}