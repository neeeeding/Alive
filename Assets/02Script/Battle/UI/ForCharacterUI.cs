using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.UI
{
    public class ForCharacterUI : MonoBehaviour
    {
        [SerializeField] private CharacterHpUI inGameHpUI;
        [SerializeField] private SkillBtn skillDelayUI;
        [SerializeField] private Image weaponImage;
        [SerializeField] private TextMeshProUGUI skillAttackText;
        
        public void ChangeWeapon(WeaponItemDataSO so)
        {
            weaponImage.sprite = so.itemImage;
        }

        public void CurSkill(float curSkillDelay, float skillAttackDelay)
        {
            skillDelayUI.SkillDelay(curSkillDelay,skillAttackDelay);
        }

        public void SetHpUI(float curHp, int maxHp,EntityName entity = EntityName.None)
        {
            if(entity != EntityName.None)
                inGameHpUI.SetCharacter(entity);
            inGameHpUI.UpdateHp(curHp, maxHp);
        }
    }
}