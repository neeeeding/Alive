using UnityEngine;
using UnityEngine.UI;

namespace _02Script.GoHouse.Stage
{
    public class StageScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform player;
        [SerializeField] private RectTransform stageBoard;
        [SerializeField] private GridLayoutGroup blockGroup;

        private readonly float _baseBlockSize = 172;
        private readonly int _space = 10;
        private readonly int _baseBlockCount = 5;
        private readonly Vector2 _maxBoardSize = new Vector2(1400,900);

        private float blockSize;
        private Vector2 boardSize;

        public void SetScreenSize(float width, float height)
        {
            blockSize = _baseBlockSize;
            if (width > _baseBlockCount || height > _baseBlockCount) //블록 수가 많음
            {
                if (width > height) //가로 수가 더 많을 때
                {
                    blockSize = _maxBoardSize.x - ((width-1) * _space);
                    blockSize /= width;
                }
                else //세로 수가 더 많을 때
                {
                    blockSize = _maxBoardSize.y - ((height-1) * _space);
                    blockSize /= height;
                }
                blockGroup.cellSize = Vector2.one * blockSize;
                player.sizeDelta = Vector2.one * blockSize;
            }
            boardSize = new Vector2(width, height) * blockSize;
            boardSize += new Vector2(width -1, height -1) * _space;
            stageBoard.sizeDelta = boardSize;
        }
    }
}