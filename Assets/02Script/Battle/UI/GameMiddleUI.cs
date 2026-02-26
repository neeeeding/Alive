using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02Script.Battle.UI
{
    public class GameMiddleUI : MonoBehaviour
    {
        [FormerlySerializedAs("battle")] [SerializeField] private Camera battleCam;
        [FormerlySerializedAs("collect")] [SerializeField] private Camera collectCam;
        [SerializeField] private RectTransform battleRect;
        [SerializeField] private RectTransform collectRect;
        [SerializeField] private GameObject stopWindow;
        [SerializeField] private TextMeshProUGUI timeText;

        private readonly float _camTopPosY = 0.5f;
        private readonly float _camBottomPosY = 0f;
        private readonly float _rectTopPosY = 540f;
        private readonly float _rectBottomPosY = 0f;
        
        private bool _isCollect; //채집 카메라가 위인지
        private float _curMin = 1; //재기
        private float _curSec = 20;

        private void OnEnable()
        {
            PlayGame();
            _isCollect = true;
        }

        public void ChangeCamera()
        {
            Rect collectCR = collectCam.rect;
            collectCR.y = _isCollect? _camBottomPosY : _camTopPosY;
            collectCam.rect = collectCR;
            
            Vector2 collectRP = collectRect.anchoredPosition;
            collectRP.y = _isCollect? -_rectTopPosY : _rectBottomPosY;
            collectRect.anchoredPosition = collectRP;
                
            Rect battleCR = battleCam.rect;
            battleCR.y = _isCollect? _camTopPosY : _camBottomPosY;
            battleCam.rect = battleCR;
            
            Vector2 battleRP = battleRect.anchoredPosition;
            battleRP.y = _isCollect? _rectTopPosY : _rectBottomPosY;
            battleRect.anchoredPosition = battleRP;
            
            _isCollect = !_isCollect;
        }

        public void StopGame()
        {
            if (Time.timeScale == 0)
            {
                PlayGame();
                return;
            }
            Time.timeScale = 0;
            stopWindow.SetActive(true);
        }
        public void PlayGame()
        {
            Time.timeScale = 1;
            stopWindow.SetActive(false);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        private void TimerUI()
        {
            _curSec -= Time.deltaTime;
            if(_curSec <= 0 && _curMin <= 0) return;
            
            if (_curSec <= 0)
            {
                _curSec = 60;
                _curMin--;
            }
            timeText.text = $"{(int)_curMin} : {(int)_curSec}";
        }

        public void SetTime(float min, float sec)
        {
            _curMin = min;
            _curSec = sec;
        }
        
        private void Update()
        {
            TimerUI();
        }
    }
}