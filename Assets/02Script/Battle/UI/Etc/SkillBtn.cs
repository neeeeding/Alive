using _02Script.Battle.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.UI.Etc
{
    public class SkillBtn : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        
        private BattleEntity _battleCharacter;

        public void UseSkill()
        {
            _battleCharacter.UseSkill();
        }
        
        public void SkillDelay(float curS, float skillDelay)
        {
            if(curS > skillDelay) curS = skillDelay;
        
            fillImage.fillAmount = curS/skillDelay;
        } 

        public void SetEntity(BattleEntity battleCharacter)
        {
            _battleCharacter = battleCharacter;
        }
    }
}