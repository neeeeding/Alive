using System;
using System.Collections.Generic;
using _02Script.Farming;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.Produce;
using _02Script.Produce.Weapon;
using _02Script.Produce.Weapon.Compound;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02Script.Tutorial.House
{
    public class HouseTutorial : Tutorial
    {
        [SerializeField] private Guide guideSc;
        
        [SerializeField] private List<GameObject> next = new List<GameObject>();
        [SerializeField] private List<GameObject> guide = new List<GameObject>();

        private bool _isGetNextCheck;
        
        private void OnEnable()
        {
            _isGetNextCheck = false;
            SeedsCard.OnClickCard += Next;
            ProduceBookCard.OnMouseClick += Next;
            InventoryItemCard.OnMouseCursor += Next;
            CompoundCheck.OnCompound += Next;
        }

        private void OnDisable()
        {
            SeedsCard.OnClickCard -= Next;
            ProduceBookCard.OnMouseClick -= Next;
            InventoryItemCard.OnMouseCursor += Next;
            CompoundCheck.OnCompound -= Next;
        }

        public void TutorialBattle()
        {
            SceneManager.LoadScene("Tutorial_Battle");
        }

        #region ActionNext
        private void Next(SeedsSO obj) //1
        {
            if(_isGetNextCheck || _curCount != 6) return;
            if (obj.seeds.itemType == ItemType.riceSeeds)
            {
                Next();
                _isGetNextCheck = true;
            }
        }
        private void Next(ProduceBookSO obj) //2
        {
            if(!_isGetNextCheck || _curCount != 14) return;
            if (obj.itemType == ItemType.warmRice)
            {
                Next();
                _isGetNextCheck = false;
            }
        }
        private void Next(ItemDataSO obj,int i,int ii,float f) //3
        {
            if(_isGetNextCheck || _curCount != 27) return;
            if(obj == null) return;
            if (obj.category == ItemCategory.food)
            {
                Next();
                _isGetNextCheck = true;
            }
        }

        private void Next(StuffItemDataSO arg1, WeaponArmorSaveData arg2, CompoundSelectWeaponArmorCard arg3) //4
        {
            if(!_isGetNextCheck || _curCount != 33) return;
            Next();
            _isGetNextCheck = false;
        }

        #endregion

        private void Awake()
        {
            _curCount = 0;
            tutorialDetail = new List<(string text,bool isStop)>()//true가 멈춤
            {
                ("현재 있는 곳은 집이에요!\n집에선 농사, 요리, 제작 등을 할 수 있어요.",true), //0
                ("먼저 농사부터 해봅시다!\n화살표를 따라가세요.\n(WASD 혹은 마우스로 이동)",false),
                ("잘 도착했어요! 여기가 밭이에요.\n씨앗을 심고 수확할 수 있어요.",true),
                ("밭을 클릭해보세요!",false),
                ("좋아요, 이제 옆에 생긴 창에서 원하는 씨앗을 고를 수 있어요.",true),
                ("다만, 현재 온습도에 씨앗을 심을 수 있는 온습도가 포함되어 있어야 해요.",true),
                ("현재 온습도가 '온난'이라 [벼의 씨앗]을 심을 수 있어요!",false),
                ("심은 씨앗을 클릭하면 남은 시간을 볼 수 있어요.",false),
                ("시간이 지나 다 자라면 수확할 수 있어요.",false),
                ("좋아요, 이제 농사는 마스터하셨네요!",true),
                ("이제 요리를 하러 갑시다.\n화살표를 따라가세요.",false), //10
                ("앞에 보이는 사물을 클릭해보세요!",false),
                ("여기에서 요리를 할 수 있어요.",true),
                ("아래는 요리 목록이에요.\n할 수 있는 요리는 배경이 초록색을 띠며 맨 앞으로 정렬돼요.",true),
                ("흰 밥을 눌러보세요!",false),
                ("왼쪽은 필요한 재료와 도구를 보여주는\n'요리 조합대'예요.",true),
                ("오른쪽의 '만들기'를 누르면 요리를 만들 수 있어요.\n다만 만들기 위해선 미니게임을 진행해야 해요.",false),
                ("미니게임은 요리마다 달라요.\n'흰 밥'은 쌀에서 이물질을 걸러내는 미니게임이에요.",true),
                ("설명서를 보고 미니게임을 해봐요!",false),
                ("요리는 창고에서 볼 수 있어요.\n화살표를 따라 눌러봐요.",false),
                ("여기는 기타 정보들이 있는 곳으로 사전에선 괴물, 아이템등의 정보를 볼 수 있어요.",true),//20
                ("창고는 현재 가지고 있는 아이템들을 볼 수 있어요.",true),
                ("인물은 현재 캐릭터의 상태등을 볼 수 있고,\n지도에서는 집의 위치를 볼 수 있어요.",true),
                ("설정을 통해 게임을 나가거나 사운드를 조정할 수 있어요.",true),
                ("저장에선 현재 게임 데이터를 저장하거나 이전에 저장한 것을 불러올 수 있어요.",true),
                ("요리를 확인 하기 위해 창고를 눌러봐요!",false),
                ("창고에선 씨앗, 곡물, 음식, 무기, 갑옷, 부품, 기계, 기타로 나눠지고\n요리를 확인하기 위해 '음식'을 눌러보세요!",false),
                ("마우스를 아이템에 가져다 놓으면 우측에서 정보를 볼 수 있어요.\n미니게임 점수가 음식의 등급을 결정해요.",false),
                ("등급이 높을 수록 음식 섭취 확률이 올라가요.",true),
                ("다시 게임으로 돌아가세요.",false),
                ("이번엔 제작을 배워봅시다\n화살표를 따라가세요.",false), //30
                ("앞에 보이는 사물을 클릭해보세요.",false),
                ("여기서 제작과 합성을 할 수 있어요.",true),
                ("재료를 드래그 하고 무기 혹은 갑옷에 드롭하면 제작 할 수 있어요.\n다만 만들기 위해선 미니게임을 진행해야 해요.",false),
                ("'제작'의 미니게임은 늘 같아요.\n적당한 점을 찾아 클릭하면 돼요.",true),
                ("미니게임을 해봐요!\n녹음,적당,식음에 따라 무기의 스킬 쿨타임, 타격, 내구도가 달라져요.",false),
                ("잘했어요. 마찬가지로 인벤토리에서 확인 할 수 있어요.",false),
                ("이번에 합성을 해봐요!\n다시 제작으로 들어와보세요.",false),
                ("옆 버튼을 클릭하면 합성을 할 수 있어요.",false),
                ("좌측에는 무기 및 갑옷이, 우측에는 재료들이 있어요",true),
                ("합성은 미니게임이 필요 없어요.\n70% 확률로 성공하고 30% 확률로 실패하게 돼요.",true), //40
                ("성공하면 내구도가 증가 되거나 버프가 추가되고, 실패하면 내구도가 감소해요.",true),
                ("방금 만든 무기와 사용 가능한 재료 하나를 택하고 '합성'버튼을 눌러주세요.",false),
                ("잘했어요. 합성도 쉽죠?",true),
                ("농사, 요리, 제작... 집에서 할 수 있는 것들을 모두 해보았어요.\n이제 전투와 채집에 대해 알아봅시다.",true),
                ("오후 8시가 되면 전투에 나갈 수 있어요.\n괴물을 클릭해보세요!",false),
            };
            ChangeText();
        }

        public override void Next()
        {
            base.Next();
            if(next[_curCount] != null)
                next[_curCount].SetActive(true);
            guideSc.SetTarget(guide[_curCount]);

            if (_curCount == 44)
            {
                HouseManager.Instance.PlayerStat.hour = 12 + 8;
            }
        }
    }
}