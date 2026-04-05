using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _02Script.Battle.Effect
{
    public class DamageEffect : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private Vector3 movePos;

        private DamageManager _manager;
        
        private Vector3 _basePos;
        private Vector3 _targetPos;

        private void Awake()
        {
            _basePos = damageText.transform.position;
            _targetPos = new Vector3(_basePos.x + movePos.x,_basePos.y + movePos.y,0);
        }

        public void Damage(int damage,bool isHit, DamageManager manager)
        {
            damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 1);
            damageText.color = isHit ? Color.red : Color.green;
            
            _manager = manager;
            damageText.text = damage.ToString();
            damageText.transform.localPosition = _basePos;

            damageText.transform.DOLocalMove(_targetPos, 1).SetEase(Ease.OutCirc).SetUpdate(true);

            Vector3 size = Vector3.one * (0.5f * ((damage / 10) + 2));
            damageText.transform.localScale = size;
    
            damageText.DOFade(0, 2).OnComplete(() =>
            {
                _manager.AddDamageEffect(this);
            });
        }
    }
}