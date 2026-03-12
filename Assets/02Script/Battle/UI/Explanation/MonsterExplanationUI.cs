using _02Script.Battle.Buff;
using _02Script.Battle.Monster;
using _02Script.Obj.Entity;
using UnityEngine;

namespace _02Script.Battle.UI.Explanation
{
    public class MonsterExplanationUI : EntityExplanationUI
    {
        [SerializeField] private BuffUI baseBuff;
        [SerializeField] private BuffUI skillBuff;
        
        private int _hp;
        protected override void OnEnable()
        {
            explanationUI.gameObject.SetActive(false);
            SetMinMax();
            Monster.Monster.OnExplanation += UIShow;
            MouseExit();
            minY -= 40;
            maxY -= 40;
        }

        protected override void OnDisable()
        {
            Monster.Monster.OnExplanation -= UIShow;
        }
        private void UIShow(EntitySO so, Vector3 cardPos, bool isUI,int hp)
        {
            if (so == null)
            {
                if(_isEnter) return;
                UIHide();
                return;
            }

            _hp = hp;
            UIShow(so,cardPos,isUI);
        }
        protected override void UIShow(EntitySO so, Vector3 cardPos, bool isUI)
        {
            EtcSet(so);
            MonsterSet(so);
            SetPos(cardPos, isUI);
        }

        private void MonsterSet(EntitySO so)
        {
            MonsterSO mSO = so as MonsterSO;
            baseBuff.gameObject.SetActive(false);
            skillBuff.gameObject.SetActive(false);
            if(mSO == null) return;
            
            SetHp(_hp,mSO.maxHp);

            if (mSO.useBuff != null)
            {
                explanationText.text = $"시작시 [{mSO.useBuff.buffName}] 을/를 시전합니다.\n" +
                                       $"{mSO.baseAttackDelay} 초 마다 {mSO.baseAttack} 데미지를 줍니다.";
                baseBuff.gameObject.SetActive(true);
                baseBuff.BuffSet(mSO.useBuff,null,EntityName.None,true);
            }

            BossMonsterSet(mSO);
        }

        private void BossMonsterSet(MonsterSO mSO)
        {
            
            BossMonsterSO bmSO = mSO as BossMonsterSO;
            skillBuff.gameObject.SetActive(false);
            if(bmSO == null) return;
            
            if (bmSO.useSkillBuff != null)
            {
                explanationText.text += $"스킬 사용---\n" +
                                        $"스킬 사용시 [{bmSO.useSkillBuff.buffName}] 을/를 시전합니다.\n" +
                                        $"{bmSO.skillAttackDelay} 초 마다 {bmSO.skillAttack} 데미지를 줍니다.";
                skillBuff.gameObject.SetActive(true);
                skillBuff.BuffSet(bmSO.useSkillBuff,null,EntityName.None,true);
            }
        }
    }
}