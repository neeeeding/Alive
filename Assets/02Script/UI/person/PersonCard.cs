using _02Script.Etc;
using _02Script.Manager;
using _02Script.SaveData;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Entity;
using _02Script.UI.Save;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace _02Script.UI.person
{
    public class PersonCard : MonoBehaviour
    {
        [SerializeField] private DialogEntitySO dialogEntity; //대상 정보

        [SerializeField] private TextMeshProUGUI characterName; //대상 이름
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI valueText; //호감도 표시 텍스트
        [SerializeField] private Slider valueSlider; //호감도 슬라이더
        [SerializeField] private TMP_InputField memo; //메모
        private int loveValue; //호감도 (수)

        private PlayerStatSC path; //스탯 (저장 공간)

        private void Awake()
        {
            characterName.text = EnumToString.Name(dialogEntity.EntityName);
            //characterImage.sprite = character.characterImage;
            HouseManager.OnStart += LoadData;
        }

        private void OnEnable()
        {
            LoadCard.OnLoad += LoadData;
            if (HouseManager.Instance.isStart)
            {
                path = HouseManager.Instance.PlayerStat;
                if(memo != null)
                    memo.text = path.characterLastText[dialogEntity.EntityName][DialogType.Memo];
                LoadData(); // 로드를 위해
            }
        }

        public void Click()
        {
        }

        public void InputText()
        {
            string value = memo.text;
            path.characterLastText[dialogEntity.EntityName][DialogType.Memo] = value; //메모 저장
        }

        private void LoveUp(int value)
        {
            loveValue += value;
            valueText.text = $"{loveValue} / 100 ";
            valueSlider.value = loveValue;

            path.characterLastText[dialogEntity.EntityName][DialogType.Love] = loveValue.ToString(); //호감도 저장

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
                    path.characterLastText[dialogEntity.EntityName][DialogType.Love] = loveValue.ToString(); //호감도 저장
                }
                else
                {
                    int.TryParse(path.characterLastText[dialogEntity.EntityName][DialogType.Love],
                        out loveValue); //호감도 저장
                    LoveUp(0);
                }
            }
        }

        private void OnDisable()
        {
            HouseManager.OnStart -= LoadData;
            LoadCard.OnLoad += LoadData;
        }
    }
}
