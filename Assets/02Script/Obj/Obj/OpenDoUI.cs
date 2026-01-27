using _02Script.DoTweenUI.Warring;
using _02Script.Farming;
using _02Script.GameEvent;
using _02Script.Player;
using UnityEngine;

namespace _02Script.Obj.Obj
{
    public class OpenDoUI : MonoBehaviour
    {
        [SerializeField] protected DoUIType doUiType;
        [SerializeField] protected GameObject doUi;
        protected bool isPlayer;
        protected bool isEvent; //밤이나 이벤트로 인지
        protected bool isShow;

        protected string addWarring;

        #region EnDiAw

        protected virtual void OnEnable()
        {
            addWarring = "지금은 ";
            GameEventManger.OnLockUI += DontUi;
            GameEventManger.OnFarmTemperature += EndEvent;
        }

        protected virtual void OnDisable()
        {
            GameEventManger.OnLockUI -= DontUi;
            GameEventManger.OnFarmTemperature -= EndEvent;
        }
        protected virtual void Awake()
        {
            isPlayer = false;
            isEvent = false;
            isShow = false;
            ClickObj();
        }

        #endregion

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == PlayerManager.curPlayer.gameObject)
            {
                isPlayer = true;
            }
        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == PlayerManager.curPlayer.gameObject)
            {
                isPlayer = false;
            }
        }

        protected void DontUi(DoUIType eventType)
        {
            isEvent = doUiType == eventType;
            
            addWarring = "지금은 ";
            if (eventType == DoUIType.all &&
                doUiType != DoUIType.none)
            {
                addWarring = "밤에는 ";
                isEvent = true;
            }
            
            addWarring += doUiType switch
            {
                DoUIType.farm => "밭을 사용하실 수 없습니다.",
                DoUIType.cook => "요리하실 수 없습니다.",
                DoUIType.produce => "제작하실 수 없습니다.",
                _=> "사용하실 수 없습니다.",
            };
        }

        protected void EndEvent(TemperatureType _)
        {
            isEvent = false;
        }

        public virtual  void ClickObj()
        {
            if (doUi.activeSelf)
            {
                doUi.SetActive(false);
            }
            else
            {
                Show();
            }
        }

        private void Show()
        {
            doUi.SetActive(!isEvent && isPlayer);

            if (!isPlayer)
            {
                WarringManager.Warring.ShowWarring("너무 멀리 있습니다.");
                return;
            }
            
            if (isEvent)
            {
                WarringManager.Warring.ShowWarring(addWarring);
            }
        }
    }

    public enum DoUIType
    {
        none = 0,
        farm,
        cook,
        produce,
        all=99,
    }
}