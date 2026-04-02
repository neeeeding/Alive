using _02Script.Obj.Obj;
using UnityEngine;

namespace _02Script.GameEvent
{
    public class EventLimitWarring : MonoBehaviour
    {
        [SerializeField] protected DoUIType myProduceType;
        
        public (bool,string)  CantUse(DoUIType type) //제작 기능을 사용할 수 있는지 유무
        {
            bool isCan = myProduceType != type;
            
            string warringText = "지금은 ";
            if (type == DoUIType.all &&
                myProduceType != DoUIType.none)
            {
                warringText = "밤에는 ";
                isCan = false;
            }
            
            warringText += myProduceType switch
            {
                DoUIType.farm => "밭을 사용하실 수 없습니다.",
                DoUIType.cook => "요리하실 수 없습니다.",
                DoUIType.produce => "제작하실 수 없습니다.",
                _=> "사용하실 수 없습니다.",
            };
            
            return (isCan, warringText);
        }
    }
}