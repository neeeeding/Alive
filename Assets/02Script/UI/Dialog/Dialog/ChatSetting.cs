using System.Collections.Generic;
using _02Script.Battle;
using _02Script.Etc;
using _02Script.GoHouse.Etc;
using _02Script.Manager;
using _02Script.Obj.Entity;
using _02Script.UI.Dialog.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            characterName.text = EnumToString.Name(current.EntityName);
            //characterImage.sprite = character.characterImage;
            if (current.EntityName != EntityName.lie)
            {
                characterLoveGauge.gameObject.SetActive(true);
                characterLoveText.gameObject.SetActive(true);

                int.TryParse(SaveManagerCheck.GetCurScenePlayerStat().characterLastText[current.EntityName][DialogType.Love], out int love);
                
                characterLoveGauge.value = love;

                characterLoveText.text = love.ToString();
            }
            else
            {
                characterLoveGauge.gameObject.SetActive(false);
                characterLoveText.gameObject.SetActive(false);
            }
        }
        
        public DialogEntitySO PlayerSelect(DialogEntitySO[] allCharacter,
            Dictionary<string,string> dialog) //말하는 이에 따른 so(이름) 바꾸기
        {
            foreach(DialogEntitySO so in allCharacter)
            {
                if(so.EntityName.ToString().ToLower() == dialog[DialogType.Player.ToString()].ToLower())
                {
                    return so;
                }
            }

            return null;
        }
    }
}