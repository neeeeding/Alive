using System;
using _02Script.Manager;
using _02Script.Player;
using UnityEngine;
using UnityEngine.UI;


namespace _02Script.UI.InGame
{
    public class RunBtn : MonoBehaviour
    {
        public static Action<float> OnMoveSpeed;
        
        [SerializeField] private Image fillImage;

        private bool _isRun;
        private readonly float _canUseRun = 5f;
        private float _curUseRun;

        private void OnEnable()
        {
            _isRun = false;
            _curUseRun = _canUseRun;
            PlayerInput.OnRunClick += IsRun;
        }

        private void OnDisable()
        {
            PlayerInput.OnRunClick -= IsRun;
        }

        private void Update()
        {
            if (_isRun)
            {
                _curUseRun -= Time.deltaTime;
                fillImage.fillAmount = _curUseRun/_canUseRun;
                if (_curUseRun <= 0)
                {
                    Walk();
                }
            }
            else if(_curUseRun < _canUseRun)
            {
                _curUseRun += Time.deltaTime;
                fillImage.fillAmount = _curUseRun/_canUseRun;
            }
        }

        private void IsRun(bool value)
        {
            if (value)
                Run();
            else
                Walk();
        }

        public void Run()
        {
            if (_curUseRun <= 0)
            {
                Walk();
                return;
            }
            _isRun = true;
            OnMoveSpeed?.Invoke(GameManager.Instance.RunSpeed);
        }

        public void Walk()
        {
            _isRun = false;
            OnMoveSpeed?.Invoke(GameManager.Instance.WalkSpeed);
        }
    }
}
