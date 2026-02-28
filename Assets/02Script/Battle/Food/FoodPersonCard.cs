using System;
using _02Script.Obj.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.Food
{
    public class FoodPersonCard : MonoBehaviour
    {
        public static Action<EntityName> OnPerson;

        [SerializeField] private Color baseColor = Color.green;
        [SerializeField] private Color changeColor = Color.orange;
        [SerializeField]private EntityName entityName;
        [SerializeField]private Image _myImage;

        #region EnDiAw
        private void OnEnable()
        {
            FoodPersonCard.OnPerson += ChangeColor;
            ChangeColor(EntityName.None);
        }
        private void OnDisable()
        {
            FoodPersonCard.OnPerson -= ChangeColor;
            ChangeColor(EntityName.None);
        }
        private void Awake()
        {
            _myImage = gameObject.GetComponent<Image>();
            ChangeColor(EntityName.None);
        }
        #endregion

        public void ClickCard()
        {
            OnPerson?.Invoke(entityName);
        }

        private void ChangeColor(EntityName name)
        {
            _myImage.color = name != entityName ? baseColor : changeColor;
        }
    }
}