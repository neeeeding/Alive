using System;
using System.Collections.Generic;
using _02Script.DoTweenUI.Warring;
using _02Script.GameEvent;
using _02Script.Inventory.Item;
using _02Script.Obj.Obj;
using _02Script.Player;
using _02Script.UI.Dialog.Dialog;
using TMPro;
using UnityEngine;

namespace _02Script.Farming
{
    //나중에 타일맵에서 그리는 걸로
    public class Field : OpenDoUI
    {
        public static Action<ItemDataSO, int> OnUseSeed;
        public static Action<ItemDataSO, int> OnGetViand;

        [Header("Setting")]
        [SerializeField] private int canFarmCount;
        [Header("Need")]
        [SerializeField] private TextMeshProUGUI curTemperatureText;
        
        [SerializeField] private OneFarming seedsPrefab;
        
        private List<OneFarming>  _seeds = new List<OneFarming>();

        private bool _isField;
        private Vector2 _clickPos;
        private int _curFarmCount;

        private TemperatureType _curTemperatureType;

        #region EnDiAw
        protected override void Awake()
        {
            base.Awake();
            NewSeeds(canFarmCount);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameEventManger.OnFarmTemperature += SetTemperature;
            PlayerInput.OnMousePos += SavePos;
            SeedsCard.OnClickCard += Plant;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameEventManger.OnFarmTemperature -= SetTemperature;
            PlayerInput.OnMousePos -= SavePos;
            SeedsCard.OnClickCard -= Plant;
        }
        #endregion

        public void SetTemperature(TemperatureType t)
        {
            _curTemperatureType = t;
            
            string temperatureType = "";
            foreach (TemperatureType type in Enum.GetValues(typeof(TemperatureType)))
            {
                if(type == TemperatureType.none) continue;
                if ((_curTemperatureType & type) != 0)
                {
                    if (temperatureType != "") temperatureType += ", ";
                    temperatureType += ChatSetting.Name(type);
                }
            }
            curTemperatureText.text = temperatureType;
        }

        //리스트 안에 넣기 (재활용)
        public void ListSeeds(OneFarming seeds)
        {
            _seeds.Add(seeds);
            _curFarmCount--;
            OnGetViand?.Invoke(seeds.GetSO().viand, 1);
            seeds.gameObject.SetActive(false);
        }
        
        //심기
        private void Plant(SeedsSO so)
        {
            if ((so.temperatureType & _curTemperatureType) == 0)
            {
                WarringManager.Warring.ShowWarring("해당 씨앗을 심기에는 적당한 온도가 아닙니다.");
                return;
            }
            
            if (_seeds.Count <= 0)
            {
                NewSeeds(1);
            }

            OneFarming newSeeds = _seeds[0];
            
            newSeeds.transform.position = _clickPos;
            newSeeds.SetSO(so,this);
            newSeeds.gameObject.SetActive(true);
            
            _curFarmCount++;
            OnUseSeed?.Invoke(so.seeds, 1);
            
            _seeds.Remove(_seeds[0]);
        }

        //새 씨앗
        private void NewSeeds(int n)
        {
            for (int i = 0; i < n; i++)
            {
                OneFarming newSeeds = Instantiate(seedsPrefab);
                newSeeds.gameObject.SetActive(false);
                newSeeds.transform.SetParent(gameObject.transform);
            
                _seeds.Add(newSeeds);
                
            }
        }

        //심을 위치 저장
        private void SavePos(Vector2 pos)
        {
            _clickPos = pos;
        }
        
        //씨앗들 보여주기 (윈도우)
        public void ClickField()
        {
            if (_curFarmCount >= canFarmCount)
            {
                WarringManager.Warring.ShowWarring("밭에 너무 많은 농작물을 심었습니다.\n수확 후 다시 시도해주세요.",2);
                return;
            }

            ClickObj();
        }
    }

    [System.Serializable]
    public class OneFarmingData
    {
        public Vector2 pos;
        public float gage;
    }
}