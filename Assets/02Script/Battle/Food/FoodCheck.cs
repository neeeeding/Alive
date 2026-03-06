using System;
using _02Script.Obj.Entity;
using UnityEngine;

namespace _02Script.Battle.Food
{
    public class FoodCheck : MonoBehaviour
    {
        public static Action<EntityName,FoodInventoryCard> OnFood;
        
        private FoodInventoryCard _curFood;
        private EntityName _entityName;
        
        private void OnEnable()
        {
            ResetSelect();
            FoodPersonCard.OnPerson += SetPerson;
            FoodInventoryCard.OnMouseClick += SetFood;
        }
        private void OnDisable()
        {
            FoodPersonCard.OnPerson -= SetPerson;
            FoodInventoryCard.OnMouseClick -= SetFood;
        }

        private void SetFood(FoodInventoryCard card)
        {
            _curFood = card;
            UseFood();
        }

        private void SetPerson(EntityName name)
        {
            _entityName = name;
            UseFood();
        }

        private void UseFood()
        {
            if (_entityName == EntityName.None || _curFood == null) return;
            
            OnFood?.Invoke(_entityName, _curFood);
            ResetSelect();
        }

        private void ResetSelect()
        {
            _entityName = EntityName.None;
            _curFood = null;
        }
    }
}