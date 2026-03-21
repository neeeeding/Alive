using _02Script.Battle.Buff;
using _02Script.Battle.Monster;
using _02Script.Obj.Entity;
using UnityEngine;

namespace _02Script.Battle.UI.Explanation
{
    public class MonsterExplanationUI : EntityExplanationUI
    {
        [SerializeField] private BuffUI[] baseBuff;
        [SerializeField] private BuffUI[] skillBuff;
        
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
            for (int i = 0; i < baseBuff.Length; i++)
            {
                baseBuff[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < skillBuff.Length; i++)
            {
                skillBuff[i].gameObject.SetActive(false);
            }
            
            if(mSO == null) return;
            
            SetHp(_hp,mSO.maxHp);

            if (mSO.eBuff.Count > 0)
            {
                explanationText.text = $"시작시 ";
                foreach (BuffSO buff in mSO.eBuff)
                {
                    explanationText.text += $"[{buff.buffName}]";
                }
                explanationText.text = $" 을/를 시전합니다.\n" +
                                       $"{mSO.baseAttackDelay} 초 마다 {mSO.baseAttack} 데미지를 줍니다.";

                for (int i = 0; i < mSO.eBuff.Count; i++)
                {
                    baseBuff[i].gameObject.SetActive(true);
                    baseBuff[i].BuffSet(mSO.eBuff[i],null,EntityName.None,true);
                }
            }

            BossMonsterSet(mSO);
        }

        private void BossMonsterSet(MonsterSO mSO)
        {
            
            BossMonsterSO bmSO = mSO as BossMonsterSO;
            for (int i = 0; i < skillBuff.Length; i++)
            {
                skillBuff[i].gameObject.SetActive(false);
            }
            if(bmSO == null) return;
            
            if (bmSO.eSkillBuff != null)
            {
                explanationText.text += $"스킬 사용---\n";
                foreach (BuffSO buff in bmSO.eSkillBuff)
                {
                    explanationText.text += $"[{buff.buffName}]";
                }
                explanationText.text = $" 을/를 시전합니다.\n" +
                                       $"{bmSO.skillAttackDelay} 초 마다 {bmSO.skillAttack} 데미지를 줍니다.";
                
                for (int i = 0; i < bmSO.eSkillBuff.Count; i++)
                {
                    baseBuff[i].gameObject.SetActive(true);
                    skillBuff[i].BuffSet(bmSO.eSkillBuff[i],null,EntityName.None,true);
                }
            }
        }
    }
}