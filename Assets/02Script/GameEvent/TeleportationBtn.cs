using _02Script.Obj.Obj;
using TMPro;
using UnityEngine;

namespace _02Script.GameEvent
{
    public class TeleportationBtn : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        
        private ObjTeleportationPos _teleportationPos;
        private EventAlarm _parent;

        private void OnDisable()
        {
            Hide();
        }

        private void Hide()
        {
            _parent.AddBtnList(this);
        }

        public void ClickTeleportationBtn()
        {
            //선택 중인 캐릭터가 위치로 이동.
            _teleportationPos.getPos();
        }
        
        public void Setting(ObjTeleportationPos pos, string name, EventAlarm parent)
        {
            _parent = parent;
            nameText.text = name + "에게";
            _teleportationPos = pos;
        }
    }
}