using System.Collections.Generic;
using _02Script.Battle.Entity;
using _02Script.Battle.Food;
using _02Script.Collect.Item;
using _02Script.GamePlayer.State;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02Script.Tutorial.Battle
{
    public class BattleTutorial : Tutorial
    {
        [SerializeField] private List<GameObject> next = new List<GameObject>();

        private void OnEnable()
        {
            CollectItem.OnGetItem += Next;
            BattleCharacter.OnChangeWeapon += Next;
            FoodCheck.OnFood += Next;
            BattleCharacter.OnAction += Next;
        }

        private void OnDisable()
        {
            CollectItem.OnGetItem -= Next;
            BattleCharacter.OnChangeWeapon -= Next;
            FoodCheck.OnFood -= Next;
            BattleCharacter.OnAction -= Next;
        }


        #region ActionNext
        private void Next(ItemDataSO arg1, int arg2, EntityName arg3)
        {
            if(_curCount != 6) return;
            Next();
        }

        private void Next(WeaponItemDataSO obj)
        {
            if(_curCount != 11) return;
            Next();
        }
        private void Next(EntityName arg1, FoodInventoryCard arg2)
        {
            if(_curCount != 16) return;
            Next();
        }

        private void Next(PlayerStateType obj)
        {
            if(obj != PlayerStateType.Skill || _curCount != 14) return;
            Next();
        }
        #endregion

        private void Awake()
        {
            _curCount = 0;
            tutorialDetail = new List<(string text,bool isStop)>()//true가 멈춤
            {
                ("전투를 하기 위해선 누가 채집을 하고 누가 싸울 건지\n역할을 분배해야 해요!",true),//0
                ("캐릭터를 클릭한 상태에서 드래그 하면 돼요!",false),
                ("위에선 채집, 아래에선 전투를 해요.",true),
                ("원하는 위치에 마우스를 가져다 둔 상태에서 마우스 휠이나 WASD를 통해\n화면 사이즈나 위치를 조정할 수 있어요.",true),
                ("채집은 우측에 정해진 시간 동안에만 진행 할 수 있어요.",true),
                ("위의 미니맵을 통해 캐릭터와 아이템들의 위치를 확인 할 수 있어요.",true),
                ("캐릭터와 아이템을 클릭해 채집을 해봐요!",false),
                ("좋아요, 이렇게 매칭하여 채집 할 수 있어요.\n채집한 아이템은 인벤토리에서 확인 할 수 있어요.",false),
                ("인벤토리가 가득 차면 채집을 진행할 수 없어요.\n아이템을 클릭 후'버리기'를 통해 정리 할 수 있어요.",false),
                ("전투도 배워볼까요?",true),
                ("마우스를 캐릭터에게 가져다 놓으면 해당 캐릭터의 스탯 창을 볼 수 있어요.",true), //10
                ("전투에서는 무기와 갑옷을 지정해줄 수 있어요.\nZ나 X를 누르거나 직접 버튼을 눌러서 무기를 변경해봐요.",false),
                ("갑옷도 같은 방법임으로 지금은 넘어갈게요.",true),
                ("무기가 지정된 상태에서는 기본으로 자동 평타를 진행해요.",true),
                ("다만 스킬은 직접 조작해야 해요.\nR이나 F를 누르거나 직접 버튼을 눌러서 스킬을 사용해봐요.",false),
                ("합성에서는 집에서와 똑같이 합성을 진행할 수 있어요.",true),
                ("섭취를 통해 전투중 음식을 먹어 능력치를 올릴 수 있어요\nC를 누르거나 직접 버튼을 눌러서 섭취해봐요.",false),
                ("참고로 피가 한 명이라도 20미만이면 블러드 스크린이 뜨고\n0이하면 전투에 실해파게 되요.",true),
                ("전투에 실패하면 게임이 리셋되니 주의하세요!!",true),
                ("충분히 익히신거 같으니\n이제, 집가기 미니게임을 하러 갑시다!",true), //19
            };
            ChangeText();
        }

        public override void Next()
        {
            base.Next();
            if (next[_curCount] != null)
            {
                next[_curCount].SetActive(true);
            }
            if (_curCount > 1 && next[_curCount-1] != null)
            {
                next[_curCount - 1].SetActive(false);
            }
        }

        private void GoHouseScene()
        {
            SceneManager.LoadScene("Tutorial_GoHouse");
        }
    }
}