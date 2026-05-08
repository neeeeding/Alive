using System.Collections.Generic;
using _02Script.Etc;
using TMPro;
using UnityEngine;

namespace _02Script.InGameDebug
{
    public class DebugPrint : Singleton<DebugPrint>
    {
        [SerializeField] private TextMeshProUGUI debugText;
        [SerializeField] private TextMeshProUGUI allDebugText;
        private string _allDebugText;

        public void PrintInGameDebug (string text, MonoBehaviour script )
        {
            debugText.text = $"{script} : [{text}]";
            _allDebugText += $"{script.name} : [{text}]///";
            allDebugText.text = _allDebugText;
        }
    }
}