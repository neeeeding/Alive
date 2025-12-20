using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace _02Script.Etc
{
    public class AsyncTime : MonoBehaviour
    {
        public static async Task WaitSeconds(float seconds, CancellationToken token)
        {
            int ms = Mathf.RoundToInt(seconds * 1000);
            await Task.Delay(ms, token);
        }
        public static async Task WaitSeconds(float seconds)
        {
            int ms = Mathf.RoundToInt(seconds * 1000);
            await Task.Delay(ms);
        }
    }
}