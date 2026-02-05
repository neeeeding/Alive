using _02Script.GamePlayer.Manager;
using UnityEngine;

namespace _02Script.Obj.Obj
{
    public class ObjTeleportationPos : MonoBehaviour
    {
        [SerializeField] private Transform pos;

        public void getPos()
        {
            HousePlayerManager.curPlayer.gameObject.transform.position = pos.position;
        }
    }
}