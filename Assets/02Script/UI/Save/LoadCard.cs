using UnityEngine;
using System.IO;
using TMPro;
using System;
using _02Script.Manager;
using _02Script.SaveData;
using Newtonsoft.Json;

namespace _02Script.UI.Save
{
public class LoadCard : MonoBehaviour
{
    public static Action OnLoad;
    public static Action<LoadCard> OnDelete;

    private string fileName;
    private string last;
    private string date;

    [SerializeField] private TextMeshProUGUI fileNameText;
    [SerializeField] private TextMeshProUGUI lastText;
    [SerializeField] private TextMeshProUGUI dateText;

    private void Awake()
    {
        if (fileName != "")
        {
            CardSetting();
        }
    }

    public void AwakeLoadSave(string name, PlayerStatSC saveStat) //전거 다시 로드하기
    {
        fileName = name;
        Setting(saveStat);
    }

    public void ClickSave(string name) //파일 이름까지 작성한 상태에서 완료 누를 때
    {
        fileName = name;

        Setting(HouseManager.Instance.PlayerStat);

        string data = JsonConvert.SerializeObject(HouseManager.Instance.PlayerStat);

        File.WriteAllText($"{HouseManager.GameSaveFilePath}/{fileName}", data);
    }

    public void ClickLoad() //불러오기 누를 때
    {
        string data = File.ReadAllText($"{HouseManager.GameSaveFilePath}/{fileName}");
        PlayerStatSC stat = JsonConvert.DeserializeObject<PlayerStatSC>(data);
        HouseManager.Instance.PlayerStat = stat;
        OnLoad?.Invoke();
        HouseManager.CoinText?.Invoke();
    }

    private void CardSetting() //카드 세팅
    {
        fileNameText.text = fileName;
        lastText.text = last;
        dateText.text = date;
    }

    private void Setting(PlayerStatSC saveStat ) //첨에 세팅
    {
        PlayerStatSC stat = saveStat;

        last = stat.lastText;

        bool pm = stat.hour >= 12 && stat.hour != 24;

        date = $"{stat.month} / {stat.day}\n{(pm ? "오후" : "오전")} {(pm ? stat.hour - 12 : stat.hour)} : {stat.minute}";

        CardSetting();
    }

    public void ClcikDeleteBtn()
    {
        OnDelete?.Invoke(this);
    }

    public void DeleteMe() //파일 삭제
    {
        string[] load = File.ReadAllLines($"{HouseManager.GameSaveFilePath}/saveName");
        File.Delete($"{HouseManager  .GameSaveFilePath}/saveName");
        
        for(int i = 0; i < load.Length; i++)
        {
            if(load[i] != fileName)
            {
                string name = load[i];
                File.AppendAllText($"{HouseManager.GameSaveFilePath}/SaveName", $"{name}\n");
            }
        }

        File.Delete($"{HouseManager.GameSaveFilePath}/{fileName}");

        Destroy(gameObject);
    }
}
}
