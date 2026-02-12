using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "BlockSO", menuName = "SO/GoHouse/BlockSO")]
    public class BlockSO : ScriptableObject
    {
        public BlockType blockType;
        public Sprite blockImage;

        public List<BlockActionSO> actions;

        public void BlockAction()
        {
            foreach (var action in actions)
            {
                action.DoBlockAction();
            }
        }
    }
    public enum BlockType
    {
        [Description("없음")] None = 0,
        [Description("벽")] Wall = 101,
        [Description("사망")] Die = 102,
        
        [Description("보스의 부산물")] BossItem = 201,
        [Description("해당 방향으로 이동")] AutoMove = 202, //이동 횟수 감소 안됨
        [Description("이동 횟수 증가 혹은 감소")] MoveCount = 203,
        [Description("잠긴 방")] LockRoom = 204,
        [Description("열쇠")] Key = 205,
        [Description("2차전")] Battle = 206,
        [Description("손실")] Less = 207, //집가기에서 얻은 것들을 잃음
        [Description("캐릭터")] Character = 208, //대사 (선택지에 따라 + 부산물)
        [Description("능력치 방")] Stat = 209, //힐 포함, 감소 혹은 증가
        [Description("포탈")] Portal = 210,
        
        [Description("집")] House = 900,
        
        //겹침
        [Description("안개")] Fog = 501,
        [Description("붕괴")] Breakdown = 502,
    }
}