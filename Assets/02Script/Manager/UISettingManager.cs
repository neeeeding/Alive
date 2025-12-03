using _02Script.Obj.Character;
using _02Script.Player;
using _02Script.UI.Chat;
using _02Script.UI.Likeability;
using _02Script.Etc;
using UnityEngine;


namespace _02Script.Manager
{
    public class UISettingManager : Singleton<UISettingManager>
    {
        [Header("Need")] [SerializeField] private GameObject[] inGame; //인 게임 필요 요소 (ex : 돈, 설정...)
        [SerializeField] private Dialog chat; //채팅
        [SerializeField] private GameObject coin; //코인 상점

        [SerializeField] private GameObject backBtn; //인 게임으로

        [Space(10f)] [Header("Need(setting)")] [SerializeField]
        private GameObject allSetting; //설정들 다 모아둔 어미

        [SerializeField] private GameObject setting; //설정
        [SerializeField] private GameObject profile; //프로필
        [SerializeField] private GameObject likeabilityGuide; //캐릭터들
        [SerializeField] private GameObject likeItem; //선호 아이템
        [SerializeField] private LikeItemManager likeItemManager; //선호 아이템 매니저
        [SerializeField] private GameObject map; //지도
        [SerializeField] private GameObject save; //저장

        [Space(10)] [Header("Select")] [SerializeField]
        private GameObject obj; //상점(혹은 교시 고르기)

        private bool isChat; // !isInGame
        private bool isObj;
        private bool isCoin;
        private bool isProfile;
        private bool isLikeItem;
        private bool isLikeabilityGuide;
        private bool isMap;
        private bool isSetting;
        private bool isSave;

        private void Awake()
        {
            isChat = false;
            InGame();
        }

        private void OnEnable()
        {
            Character.OnChat += Chat;
        }

        private void OnDisable()
        {
            Character.OnChat -= Chat;
        }

        private void Update()
        {
            if ((isProfile || isSetting || isLikeabilityGuide || isLikeItem || isMap || isSetting || isSave || isCoin ||
                 isObj || isChat) && PlayerMobileInput.Instance.CheckCanInput())
            {
                PlayerMobileInput.Instance.NoInput();
            }
        }

        public void InGame() //게임으로
        {
            GameManager.Instance.PlayerStat.isChat = false;
            AllHide();
            SettingAll();
            Time.timeScale = 1f;
            PlayerMobileInput.Instance.CanInput();
        }

        public void CloseChat() //채팅 닫기
        {
            AllHide();
            isChat = false;
            GameManager.Instance.PlayerStat.isChat = false;
            SettingAll();
            InGame();
            //chat.DialogSetting(null,null);
        }

        public void Coin() //코인 상점
        {
            AllHide();
            isCoin = true;
            SettingAll();
        }

        public void Obj() //상점 혹은 학교 수업 선택
        {
            AllHide();
            isObj = true;
            SettingAll();
        }

        public void Chat(CharacterSO so, Character character) //채팅
        {
            AllHide();
            isChat = true;
            SettingAll();
            chat.DialogSetting(so, character);
            GameManager.Instance.PlayerStat.isChat = true;
        }

        public void Profile() //프로필
        {
            AllHide();
            isProfile = true;
            SettingAll();
        }

        public void LiKeItme(CharacterSO so) //선호 아이템
        {
            AllHide();
            isLikeItem = true;
            isLikeabilityGuide = true;
            likeItemManager.Setting(so);
            SettingAll();
        }

        public void LikeabilityGuide() //호감도
        {
            AllHide();
            isLikeabilityGuide = true;
            SettingAll();
        }

        public void Map() //지도
        {
            AllHide();
            isMap = true;
            SettingAll();
        }

        public void Setting() // 세팅
        {
            AllHide();
            isSetting = true;
            SettingAll();
        }

        public void Save() //저장
        {
            AllHide();
            isSave = true;
            SettingAll();
        }

        private void SettingAll() //세팅들
        {
            profile.SetActive(isProfile);
            likeItem.SetActive(isLikeItem);
            likeabilityGuide.SetActive(isLikeabilityGuide);
            map.SetActive(isMap);
            setting.SetActive(isSetting);
            save.SetActive(isSave);

            coin.SetActive(isCoin);
            chat.gameObject.SetActive(isChat);
            foreach (GameObject obj in inGame)
            {
                obj.SetActive(!isChat);
            }

            obj.SetActive(isObj);

            bool all = isProfile || isSetting || isLikeabilityGuide || isLikeItem || isMap || isSetting || isSave;
            allSetting.SetActive(all);
            backBtn.SetActive(all || isCoin || isObj);

            Time.timeScale = 0f;
        }

        private void AllHide() //전부 숨기기
        {
            isCoin = false;
            //isChat = false;
            isObj = false;
            isProfile = false;
            isLikeItem = false;
            isLikeabilityGuide = false;
            isMap = false;
            isSetting = false;
            isSave = false;
        }
    }
}
