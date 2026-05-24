using System.Collections.Generic;
using UnityEngine;

namespace _02Script.Tutorial.Battle
{
    public class BattleTutorial : Tutorial
    {
        private void Awake()
        {
            tutorialDetail = new List<(string text,bool isStop)>()//true가 멈춤
            {
                ("전투를 하기 위해선 누가 채집을 하고 누가 싸울 건지\n역할을 분배해야 해요!",true),
                ("캐릭터를 클릭한 상태에서 드래그 하면 돼요!",false),
                ("위에선 채집, 아래에선 전투를 해요.",true),
                ("채집은 우측에 정해진 시간 동안에만 진행 할 수 있어요.",true),
                ("위의 미니맵을 통해 캐릭터와 아이템들의 위치를 확인 할 수 있어요.",true),
                ("캐릭터와 아이템을 클릭해 채집을 해봐요!",false),
                ("좋아요, 이렇게 매칭하여 채집 할 수 있어요.\n채집한 아이템은 인벤토리에서 확인 할 수 있어요.",false),
                ("인벤토리가 가득 차면 채집을 진행할 수 없어요.\n'버리기'를 통해 정리 할 수 있어요.",false),
            };
        }
    }
}