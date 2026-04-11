using _02Script.Etc;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

namespace _02Script.MiniGame
{
    public class MiniGameCount : MonoBehaviour
    {
        //[SerializeField] private SerializedDictionary<MiniGameType, GameObject> miniGameDict;
        [SerializeField] private TextMeshProUGUI countText;

        private int _curCount;

        private void Count(MiniGameType game)
        {
            Count();
            
            //miniGameDict[game].SetActive(true);
        }

        private async void Count()
        {
            countText.gameObject.SetActive(true);
            for (int i = 3; i > 0; i--)
            {
                countText.text = _curCount.ToString();
                await AsyncTime.WaitSeconds(1);
            }
            countText.gameObject.SetActive(false);
        }
    }

    public enum MiniGameType
    {
        Produce, //제작
        FryingPan, //프라이팬
        Pot, // 냄비
        Oven, //오븐
        RiceCooker, //밥솥
        MicrowaveOven, // 전자레인지
    }
}