using _02Script.Etc;
using _02Script.Obj.Entity;
using TMPro;
using UnityEngine;

namespace _02Script.Battle.Monster
{
    public class BossMonsterHpUI : MonsterHpUI
    {
        [SerializeField] private TextMeshProUGUI nameText;
        
        public void SetCharacter(EntityName characterName)
        {
            nameText.text = EnumToString.Name(characterName);
        }
    }
}