using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.UI
{
    public class SkillBtn : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        public void SkillDelay(float curS, float skillDelay)
        {
            if(curS > skillDelay) curS = skillDelay;
        
            fillImage.fillAmount = curS/skillDelay;
        }
    }
}