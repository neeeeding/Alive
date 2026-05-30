using System.Collections.Generic;
using _02Script.GoHouse.SO;
using UnityEngine;

namespace _02Script.Tutorial.GoHouse
{
    public class GoHouseTutorial : Tutorial
    {
        [SerializeField] private List<GameObject> next = new List<GameObject>();

        private void OnEnable()
        {
            HouseSO.OnSuccess += ResetGame;
        }

        private void OnDisable()
        {
            HouseSO.OnSuccess -= ResetGame;
        }

        private void ResetGame(string arg1, BlockActionSO arg2)
        {
            base.ResetGame();
        }

        private void Awake()
        {
            _curCount = 0;
            tutorialDetail = new List<(string text,bool isStop)>()//true가 멈춤
            {
                ("전투 후에는 늘 '집 가기 미니 게임'이 있어요.",true),//0
                ("WASD나 버튼 클릭으로 '말'을 움직일 수 있어요.",true),
                ("'집 가기' 에서는 다양한 블럭이 존재해요.",true),
                ("'집' 블럭에 도달하면 게임이 끝나요.",true),
                ("다만, 이동 횟수가 닳기 전에 도착해야 해요.",true),
                ("이동 횟수가 닳거나 죽는 블럭에 닿으면 실패하게 돼요.",true), //5
                ("전투와 다르게 게임이 초기화 되지는 않지만,\n채집에서 얻은 아이템의 5%가 랜덤으로 소멸하게 되어요.",true),
                ("또한 '집 가기 미니 게임'에서 얻은 아이템이나 스탯 또한 사라지게 된답니다.",true),
                ("설명은 끝났어요!\n튜토리얼이니, 최대한 다양한 블럭들을 경험해보세요!!",true),
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
    }
}