using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace _02Script.Etc
{
    public class AsyncTime : MonoBehaviour
    {
        public static async Task WaitSeconds(float seconds, CancellationToken token, bool isZeroTime/*시간 멈출때 안 멈출 건지*/)
        {
            while(!isZeroTime)
            {
                await ZeroTimeWait(seconds);
                return;
            }
            
            int ms = Mathf.RoundToInt(seconds * 1000);
            await Task.Delay(ms, token);
        }
        public static async Task WaitSeconds(float seconds, bool isZeroTime)
        {
            while(!isZeroTime)
            {
                await ZeroTimeWait(seconds);
                return;
            }
            int ms = Mathf.RoundToInt(seconds * 1000);
            await Task.Delay(ms);
        }

        private static async Task ZeroTimeWait(float seconds)
        {
            float curS = 0f;

            while (curS < seconds)
            {
                if (Time.timeScale > 0)
                    curS += Time.deltaTime;

                await Task.Yield();
            }
        }
    }
}