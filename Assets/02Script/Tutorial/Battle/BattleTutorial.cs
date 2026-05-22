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
            };
        }
    }
}