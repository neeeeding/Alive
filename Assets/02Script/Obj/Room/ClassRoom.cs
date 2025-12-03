using _02Script.Manager;
using _02Script.Obj.Character;
using _02Script.UI.Chat;
using _02Script.UI.School;
using UnityEngine;

namespace _02Script.Obj.Room
{
    public class ClassRoom : OneRoom
    {
        [Header("ClassRoom")]
        [SerializeField] private CountClassTime classManager;
        [SerializeField] private PlayerJob classType;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            WantEnterRoom();
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
            WantExitRoom();
        }

        public override void WantEnterRoom()
        {
            base.WantEnterRoom();
            room.gameObject.SetActive(true);
            classManager.CheckClass(classType);
        }

        public override void WantExitRoom()
        {
            base.WantExitRoom();
            room.gameObject.SetActive(false);
        }
    }
}