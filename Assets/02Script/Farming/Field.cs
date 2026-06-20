using System;
using System.Collections.Generic;
using System.Linq;
using _02Script.DoTweenUI.Warring;
using _02Script.Etc;
using _02Script.GameEvent;
using _02Script.Inventory.Item;
using _02Script.Obj.Obj;
using _02Script.GamePlayer.Manager;
using _02Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        [SerializeField] private TextMeshProUGUI curCountText;
        [SerializeField] private Transform parent;
        [SerializeField] private Tilemap farmTilemap;
        
        [SerializeField] private OneFarming seedsPrefab;
        
        private List<OneFarming> _seeds = new List<OneFarming>();
        private Dictionary<Vector3Int, OneFarming> _plantedSeeds = new Dictionary<Vector3Int, OneFarming>(); // 추가

        private bool _isField;
        private int _curFarmCount;

        private TemperatureType _curTemperatureType;

        #region EnDiAw
        protected override void Awake()
        {
            CountText();
            base.Awake();
            NewSeeds(canFarmCount);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameEventManager.OnFarmTemperature += SetTemperature;
            SeedsCard.OnClickCard += Plant;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameEventManager.OnFarmTemperature -= SetTemperature;
            SeedsCard.OnClickCard -= Plant;
        }
        #endregion

        protected override void OnTriggerEnter2D(Collider2D collision) //창 보이기
        {
            if (collision.gameObject == HousePlayerManager.curPlayer.gameObject)
            {
                isPlayer = true;
                ClickObj();
            }
        }
        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == HousePlayerManager.curPlayer.gameObject)
            {
                isPlayer = false;
                doUi.SetActive(false);
            }
        }

        public void SetTemperature(TemperatureType t) //온도 설정
        {
            _curTemperatureType = t;
            
            string temperatureType = "";
            foreach (TemperatureType type in Enum.GetValues(typeof(TemperatureType)))
            {
                if(type == TemperatureType.none) continue;
                if ((_curTemperatureType & type) != 0)
                {
                    if (temperatureType != "") temperatureType += ", ";
                    temperatureType += EnumToString.Name(type);
                }
            }
            curTemperatureText.text = temperatureType;
        }

        //리스트 안에 넣기 (재활용)
        public void ListSeeds(OneFarming seeds)
        {
            _seeds.Add(seeds);
            _curFarmCount--;
            CountText();
            OnGetViand?.Invoke(seeds.GetSO().viand, 1);
            seeds.gameObject.SetActive(false);

            //목록에서 서 제거
            Vector3Int key = default;
            bool found = false;
            foreach (var kvp in _plantedSeeds)
            {
                if (kvp.Value == seeds)
                {
                    key = kvp.Key;
                    found = true;
                    break;
                }
            }
            if (found) _plantedSeeds.Remove(key);
        }
        
        //심기
        private void Plant(SeedsSO so)
        {
            if (isEvent)
            {
                WarringManager.Warring.ShowWarring(addWarring);
                return;
            }
            
            if (_curFarmCount >= canFarmCount)
            {
                WarringManager.Warring.ShowWarring("밭에 너무 많은 농작물을 심었습니다.\n수확 후 다시 시도해주세요.",2);
                return;
            }
            if ((so.temperatureType & _curTemperatureType) == 0)
            {
                WarringManager.Warring.ShowWarring("해당 씨앗을 심기에는 적당한 온도가 아닙니다.",2);
                return;
            }

            Vector3Int pos = farmTilemap.WorldToCell(HousePlayerManager.curPlayer.gameObject.transform.position);

            // 추가: 이미 심어져 있는지 확인
            if (_plantedSeeds.ContainsKey(pos))
            {
                WarringManager.Warring.ShowWarring("이미 작물이 심겨져 있습니다. 다른 위치에서 시도해주세요", 2);
                return;
            }

            if (_seeds.Count <= 0)
            {
                NewSeeds(1);
            }

            SeedPlant(so, pos);
        }

        public void LoadFarm(Dictionary<ItemType, SeedsSO> seeds)
        {
            Dictionary<SaveVector2, ItemType> farm = HouseManager.Instance.PlayerStat.farm.ToDictionary(); 

            foreach (KeyValuePair<SaveVector2, ItemType> item in farm.ToArray())
            {
                if(item.Value == ItemType.none) continue;
                SeedPlant(seeds[item.Value], new Vector3Int((int)item.Key.x, (int)item.Key.y, 0));
            }
        }

        private void SeedPlant(SeedsSO so, Vector3Int pos)
        {
            OneFarming newSeeds = _seeds[0];

            TileBase farmTile = farmTilemap.GetTile(pos);
            Vector3 plantPos = farmTilemap.GetCellCenterWorld(pos);

            if (farmTile == null) return;

            newSeeds.transform.position = plantPos;
            newSeeds.SetSO(so, pos, this);
            newSeeds.gameObject.SetActive(true);

            _curFarmCount++;
            CountText();
            OnUseSeed?.Invoke(so.seeds, 1);

            _seeds.Remove(_seeds[0]);

            _plantedSeeds[pos] = newSeeds; // 추가
        }

        //새 씨앗
        private void NewSeeds(int n)
        {
            for (int i = 0; i < n; i++)
            {
                OneFarming newSeeds = Instantiate(seedsPrefab,parent);
                newSeeds.gameObject.SetActive(false);
                newSeeds.transform.SetParent(parent);
            
                _seeds.Add(newSeeds);
                
            }
        }
        
        //카운트 텍스트
        private void CountText()
        {
            curCountText.text = $"{_curFarmCount}/{canFarmCount}";
        }
    }

    [Serializable]
    public class OneFarmingData
    {
        public Vector2 pos;
        public float gage;
    }
}