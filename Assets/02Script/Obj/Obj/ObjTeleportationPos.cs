using UnityEngine;

namespace _02Script.Obj.Obj
{
    public class ObjTeleportationPos : MonoBehaviour
    {
        [SerializeField] private Transform pos;

        public Vector2 getPos()
        {
            return pos.position;
        }
    }
}