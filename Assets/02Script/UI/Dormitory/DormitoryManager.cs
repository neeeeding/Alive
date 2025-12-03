using _02Script.Manager;
using UnityEngine;

namespace _02Script.UI.Dormitory
{
    public class DormitoryManager : MonoBehaviour
    {
        [Header("Setting")] [SerializeField] private int roleHour = 10; //귀가 해야 하는 시간

        private void Awake()
        {
            CheckClock(); // 늦게 왔는지 (나중에 외박 일 수도 있으니 그것도 확인
        }


        private void CheckClock() //시간 확인
        {
            int hour = GameManager.Instance.PlayerStat.hour;
            int minute = GameManager.Instance.PlayerStat.minute;


            if (hour > roleHour)
            {
                print($"지금 {hour}시가 넘었는데 이제야 오는거야?!");
            }
            else
            {
                print("좋아, 제 시간에 맞춰 왔네.");
            }
        }
    }
}