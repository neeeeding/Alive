using System.Collections.Generic;
using _02Script.Battle.Monster;
using _02Script.Collect.Item;
using _02Script.Etc;
using _02Script.GoHouse.Stage;
using _02Script.Inventory.Item;
using UnityEditor;
using UnityEngine;

namespace _02Script.Battle.Stage
{
    [CreateAssetMenu(fileName = "BattleStageSO", menuName = "SO/Battle/BattleStageSO")]
    public class BattleStageSO : ScriptableObject
    {
        public string stageName;
        public int stageNum;

        public Vector2 canCollectTime; //채집 가능 시간
        
        //몬스터 스폰에 대해
        public SaveDictionary<MonsterSO, List<float>> monsterSpawn = new SaveDictionary<MonsterSO, List<float>>();
        public SaveDictionary<MonsterSO, List<Vector3>> monsterPos = new SaveDictionary<MonsterSO, List<Vector3>>();
        
        //아이템
        public SaveDictionary<ItemDataSO, List<int>> itemSpawn =new SaveDictionary<ItemDataSO, List<int>>();
        public SaveDictionary<ItemDataSO, List<Vector3>> itemPos =new SaveDictionary<ItemDataSO, List<Vector3>>();

        [Header("GoHouse")]
        public GoHouseStageSO goHouse; //집가기

        public void SetMonster(Dictionary<MonsterSO, List<float>> m, Dictionary<MonsterSO,List<Vector3>> p)
        {
            monsterSpawn.Clear();
            monsterSpawn.Dictionary(m);
            
            monsterPos.Clear();
            monsterPos.Dictionary(p);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }

        public void SetItem(Dictionary<ItemDataSO, List<int>>i, Dictionary<ItemDataSO, List<Vector3>> p)
        {
            itemSpawn.Clear();
            itemSpawn.Dictionary(i);
            
            itemPos.Clear();
            itemPos.Dictionary(p);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }
    }
}