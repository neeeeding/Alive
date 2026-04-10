using System;
using System.Collections.Generic;
using _02Script.Battle.Entity;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _02Script.UI.Person
{
    public class PersonWeaponInventory : MonoBehaviour
    {
        [SerializeField] private GameObject window;
        [SerializeField] private BattleEntitySO[] entities;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private GameObject inventoryParent;
        [SerializeField] private Transform allParent;
        [SerializeField] private ItemCard cardPrefabs;
        
        private Dictionary<EntityName, List<WeaponItemDataSO>> _weapons = new Dictionary<EntityName, List<WeaponItemDataSO>>();
        private Dictionary<EntityName, RectTransform> _parent = new Dictionary<EntityName, RectTransform>();

        #region EnDiAw
        private void OnEnable()
        {
            Close();
            PersonCard.OnOpenWeapon += Setting;
        }

        private void OnDisable()
        {
            PersonCard.OnOpenWeapon -= Setting;
        }

        private void Awake()
        {
            window.SetActive(false);
            DictionaryOrganize();
            InventorySetting();
        }
        #endregion

        private void Setting(EntityName name, Transform btnPos)
        {
            if (window.activeSelf && transform.parent == btnPos)
            {
                Close();
                return;
            }
            
            window.SetActive(true);
            foreach (KeyValuePair<EntityName, RectTransform> p in _parent)
            {
                p.Value.gameObject.SetActive(false);
            }
            _parent[name].gameObject.SetActive(true);
            _parent[name].anchoredPosition = Vector2.zero;
            scrollRect.content = _parent[name];
            
            transform.SetParent(btnPos);
            transform.position = btnPos.position + (Vector3.up * 300);
        }

        private void Close()
        {
            window.SetActive(false);
        }

        private void InventorySetting()
        {
            foreach (KeyValuePair<EntityName, List<WeaponItemDataSO>> weapon in _weapons)
            {
                GameObject parent = Instantiate(inventoryParent, allParent);
                _parent.Add(weapon.Key, parent.GetComponent<RectTransform>());
                foreach (WeaponItemDataSO w in weapon.Value)
                {
                    ItemCard cardObj = Instantiate(cardPrefabs,_parent[weapon.Key].transform);
                    ItemData data = new ItemData();
                    data.NewItem(w);
                    cardObj.NewCard(data,0,0,null);
                }
                
                _parent[weapon.Key].gameObject.SetActive(false);
            }
        }

        private void DictionaryOrganize()
        {
            foreach (BattleEntitySO e in entities)
            {
                _weapons.Add(e.EntityName, e.useWeapons);
            }
        }
    }
}