using System.Threading;
using System.Threading.Tasks;
using _02Script.Etc;
using _02Script.Manager;
using _02Script.UI.Save;
using UnityEngine;

namespace _02Script.Player.State
{
    public class HousePlayerMovement: PlayerMovement
    {
        private readonly int[] autoX = { 1, 0, -1, 0 };
        private readonly int[] autoY = { 0, 1, 0, -1 };
        private CancellationTokenSource cts = new(); //시간을 위해
        

        #region EnDiAwStDe

        protected override void Awake()
        {
            base.Awake();
            GameManager.OnStart += StartLoad;
        }

        private void Start()
        {
            _ = AutoMove();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            PlayerInput.OnMousePos += MouseMove;
            PlayerInput.OnMovePos += KeyboardMove;
            LoadCard.OnLoad += Load;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            PlayerInput.OnMousePos -= MouseMove;
            PlayerInput.OnMovePos -= KeyboardMove;
            GameManager.OnStart -= StartLoad;
            LoadCard.OnLoad -= Load;
        }
        
        private void OnDestroy()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
            }
        }
        #endregion

        #region Load
        private void StartLoad()
        {
            Vector2 position = GameManager.Instance.saveData.stat.characterPositions[player.playerName];
            GameManager.Instance.PlayerStat.characterPositions[player.playerName] = position;
            Load();
        }

        private void Load()
        {
            transform.position = GameManager.Instance.PlayerStat.characterPositions[player.playerName];
        }
        #endregion

        #region Move
        private void MouseMove(Vector2 mousePos)
        {
            if(!player.isCurPlayer) return;
            IsMoving = true;
            TargetPos = mousePos;
            MoveStart();
        }
        private void KeyboardMove(Vector2 mousePos)
        {
            if(!player.isCurPlayer) return;
            IsMoving = true;
            TargetPos = (Vector2)transform.position + mousePos.normalized;
            MoveStart();
        }
        private async Task AutoMove()
        {
            while (!cts.IsCancellationRequested)
            {
                if (player.isCurPlayer)
                {
                    await Task.Yield();
                    continue;
                }
                try
                {
                    await AsyncTime.WaitSeconds(Random.Range(0,1.1f), cts.Token, false);
                    
                    int auto = Random.Range(0, autoX.Length);
            
                    IsMoving = true;
                    TargetPos = (Vector2)transform.position + new Vector2(autoX[auto], autoY[auto]);
                    MoveStart();
                }
                catch (TaskCanceledException){break;}
            }
        }

        protected override void Arrive()
        {
            GameManager.Instance.PlayerStat.characterPositions[player.playerName] = transform.position; //위치 저장
            base.Arrive();
        }
        #endregion
    }
}
