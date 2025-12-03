using _02Script.Manager;
using _02Script.Obj.Character;
using _02Script.Player;
using _02Script.UI.Chat;
using _02Script.UI.Save;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace _02Script.UI.Likeability
{
    public class LikeabilityCard : MonoBehaviour
    {
        [SerializeField] private CharacterSO character; //대상 정보

        [SerializeField] private TextMeshProUGUI characterName; //대상 이름
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI valueText; //호감도 표시 텍스트
        [SerializeField] private Slider valueSlider; //호감도 슬라이더
        [SerializeField] private TMP_InputField memo; //메모
        private int loveValue; //호감도 (수)

        private PlayerStatSC path; //스탯 (저장 공간)

        private void Awake()
        {
            characterName.text = ChatSetting.Name(character.characterName);
            //characterImage.sprite = character.characterImage;
            GameManager.OnStart += LoadData;
        }

        private void OnEnable()
        {
            LoadCard.OnLoad += LoadData;
            if (GameManager.Instance.isStart)
            {
                path = GameManager.Instance.PlayerStat;
                memo.text = path.characterlastText[character.characterName][DialogType.Memo];
                LoadData(); // 로드를 위해
            }
        }

        public void Click()
        {
            UISettingManager.Instance.LiKeItme(character);
        }

        public void InputText()
        {
            string value = memo.text;
            path.characterlastText[character.characterName][DialogType.Memo] = value; //메모 저장
        }

        private void LoveUp(int value)
        {
            loveValue += value;
            valueText.text = $"{loveValue} / 100 ";
            valueSlider.value = loveValue;

            path.characterlastText[character.characterName][DialogType.Love] = loveValue.ToString(); //호감도 저장

            SaveMyLoveValue(true);
        }

        private void LoadData()
        {
            SaveMyLoveValue(false);
        }

        private void SaveMyLoveValue(bool set)
        {
            if (path != null)
            {
                if (set)
                {
                    path.characterlastText[character.characterName][DialogType.Love] = loveValue.ToString(); //호감도 저장
                }
                else
                {
                    int.TryParse(path.characterlastText[character.characterName][DialogType.Love],
                        out loveValue); //호감도 저장
                    LoveUp(0);
                }
            }
        }

        private void OnDisable()
        {
            GameManager.OnStart -= LoadData;
            LoadCard.OnLoad += LoadData;
        }
    }
}
