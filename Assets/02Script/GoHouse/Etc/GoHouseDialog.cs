using System;
using _02Script.GoHouse.SO;
using _02Script.Manager;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Entity;
using UnityEngine;

namespace _02Script.GoHouse.Etc
{
    public class GoHouseDialog : DialogEntity
    {
        public static Action<DialogEntitySO,DialogEntity> OnChat;
        
        [SerializeField] private Dialog chat; //채팅
        
        private void OnEnable()
        {
            CharacterSO.OnChat += GetBossItem;
        }

        private void OnDisable()
        {
            CharacterSO.OnChat -= GetBossItem;
        }

        private void GetBossItem(DialogEntitySO entity, int c, int f)
        {
            chapter = c;
            finalNum = f;
            int baseChapter = (chapter%100) + (int)entity.EntityName;
            int.TryParse(HouseManager.Instance.PlayerStat.characterLastText[entity.EntityName][DialogType.Chapter],out chapter);
            if (chapter <= 0) chapter = baseChapter;
            int.TryParse(HouseManager.Instance.PlayerStat.characterLastText[entity.EntityName][DialogType.Num],out finalNum);
            if (finalNum <= 0) finalNum = 1;
                
            OnChat?.Invoke(entity,this);
            
            chat.DialogSetting(entity, this);
            HouseManager.Instance.PlayerStat.isChat = true;
        }
    }
}