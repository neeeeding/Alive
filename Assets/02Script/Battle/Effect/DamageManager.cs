using System;
using System.Collections.Generic;
using _02Script.Battle.Entity;
using UnityEngine;

namespace _02Script.Battle.Effect
{
    public class DamageManager : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private DamageEffect damageEffect;
        private List<DamageEffect> _damageEffects = new List<DamageEffect>();

        private void OnEnable()
        {
            BattleEntity.OnHit += NewDamage;
        }

        private void OnDisable()
        {
            BattleEntity.OnHit -= NewDamage;
        }

        public void AddDamageEffect(DamageEffect damageEffect)
        {
            _damageEffects.Add(damageEffect);
            damageEffect.gameObject.SetActive(false);
        }

        private void NewDamage(Transform position, int damage, bool isHit/*맞은건지 힐인지*/)
        {
            DamageEffect effect;
            if (_damageEffects.Count <= 0)
            {
                effect = Instantiate(damageEffect,parent);
                effect.gameObject.SetActive(false);
                _damageEffects.Add(effect);
            }
            
            effect = _damageEffects[0];
            effect.Damage(damage,isHit,this);
            effect.transform.SetParent(position);
            effect.gameObject.SetActive(true);
            effect.transform.localPosition = Vector3.zero;
            _damageEffects.RemoveAt(0);
        }
    }
}