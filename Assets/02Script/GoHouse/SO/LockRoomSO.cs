using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "LockRoomSO", menuName = "SO/GoHouse/Block/LockRoomSO")]
    public class LockRoomSO : BlockActionSO
    {
        public int roomNum;
        public override void DoBlockAction()
        {
        }

        public bool KeyCheck(int key)
        {
            return key == roomNum;
        }
    }
}