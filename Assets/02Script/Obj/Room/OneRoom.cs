using UnityEngine;

namespace _02Script.Obj.Room
{
    public class OneRoom : MonoBehaviour
    {
        [Header("OneRoom")]
        [SerializeField] protected GameObject room;
        protected bool isDoor;

        protected virtual void Awake()
        {
            isDoor = false;
            room.SetActive(false);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            isDoor = true;
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            isDoor = false;
        }

        public virtual void WantEnterRoom()
        {
            if(!isDoor) return;
        }

        public virtual void WantExitRoom()
        {
            if(isDoor) return;
        }
    }
}