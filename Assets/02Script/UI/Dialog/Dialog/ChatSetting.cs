using System;
using System.Collections.Generic;
using System.ComponentModel;
using _02Script.Manager;
using _02Script.UI.Dialog.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _02Script.UI.Dialog.Entity;

namespace _02Script.UI.Dialog.Dialog
{
    public class ChatSetting : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI characterName; //이름
        
        [SerializeField] private Image characterImage; //사진
        [SerializeField] private Slider characterLoveGauge; //러브 게이지
        [SerializeField] private TextMeshProUGUI characterLoveText; //러브 게이지


        public void CurrentCharacter(DialogEntitySO current) //첫 세팅
        {
            characterName.text = Name(current.EntityName);
            //characterImage.sprite = character.characterImage;
            if (current.EntityName != EntityName.lie)
            {
                characterLoveGauge.gameObject.SetActive(true);
                characterLoveText.gameObject.SetActive(true);

                int.TryParse(GameManager.Instance.PlayerStat.characterLastText[current.EntityName][DialogType.Love],
                    out int love);
                characterLoveGauge.value = love;

                characterLoveText.text =
                    GameManager.Instance.PlayerStat.characterLastText[current.EntityName][DialogType.Love];
            }
            else
            {
                characterLoveGauge.gameObject.SetActive(false);
                characterLoveText.gameObject.SetActive(false);
            }
        }

        public static string Name<T>(T wantName)
        {
            var field = wantName.GetType().GetField(wantName.ToString());
            var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attr?.Description??wantName.ToString();
        }
        
        public DialogEntitySO PlayerSelect(int i, DialogEntitySO[] allCharacter,
            List<Dictionary<string,string>> dialog) //말하는 이에 따른 so(이름) 바꾸기
        {
            foreach(DialogEntitySO so in allCharacter)
            {
                if(so.EntityName.ToString().ToLower() == dialog[i][DialogType.Player.ToString()].ToLower())
                {
                    return so;
                }
            }

            return null;
        }
    }
}