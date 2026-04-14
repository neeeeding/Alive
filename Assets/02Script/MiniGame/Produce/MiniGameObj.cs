using UnityEngine;

namespace _02Script.MiniGame.Produce
{
    public class MiniGameObj : MonoBehaviour
    {
        protected MiniGameObjSpawn spawn;

        public void SetSpawn(MiniGameObjSpawn s)
        {
            spawn = s;
        }
    }
}