using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using System;
using _02Script.Manager;
using _02Script.Obj.Character;

namespace _02Script.UI.Chat
{
    public class ChatSetting : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI characterName; //이름
        [SerializeField] private Image characterImage; //사진
        [SerializeField] private Slider characterLoveGauge; //러브 게이지
        [SerializeField] private TextMeshProUGUI characterLoveText; //러브 게이지

        public void CurrentCharacter(CharacterSO current) //첫 세팅
        {
            characterName.text = Name(current.characterName);
            //characterImage.sprite = character.characterImage;
            if (current.characterName != CharacterName.me)
            {
                characterLoveGauge.gameObject.SetActive(true);
                characterLoveText.gameObject.SetActive(true);

                int.TryParse(GameManager.Instance.PlayerStat.characterlastText[current.characterName][DialogType.Love],
                    out int love);
                characterLoveGauge.value = love;

                characterLoveText.text =
                    GameManager.Instance.PlayerStat.characterlastText[current.characterName][DialogType.Love];
            }
            else
            {
                characterLoveGauge.gameObject.SetActive(false);
                characterLoveText.gameObject.SetActive(false);
            }
        }

        public static string Name<T>(T wantName) //enum의 주석? 얻기
        {
            var field = wantName.GetType().GetField(wantName.ToString());
            var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attr?.Description ?? wantName.ToString();
        }
    }
}