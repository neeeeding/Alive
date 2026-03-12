using System.Collections.Generic;
using _02Script.Battle.Monster;
using _02Script.GoHouse.Stage;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Battle.Stage
{
    [CreateAssetMenu(fileName = "BattleStageSO", menuName = "SO/Battle/BattleStageSO")]
    public class BattleStageSO : ScriptableObject
    {
        public string stageName;
        public int stageNum;

        public Vector2 canCollectTime; //채집 가능 시간
        [Header("PlayerPos")]
        public Vector3 cPlayerOnePos;
        public Vector3 cPlayerTwoPos;
        public Vector3 bPlayerOnePos;
        public Vector3 bPlayerTwoPos;
        
        [Header("CamPos")]
        public Vector3 cCamPos;
        public Vector3 bCamPos;
        public Vector3 miniCamPos;
        
        public Vector2 cCamLimitOffset;
        public Vector2 cCamLimitSize;
        public Vector2 bCamLimitOffset;
        public Vector2 bCamLimitSize;
        public Vector3 miniCamSize;
        
        [Header("Monster")]
        public List<MonsterSO> monster = new List<MonsterSO>();
        public List<float> mTime = new List<float>();
        public List<Vector3> mPos = new List<Vector3>();
        
        [Header("Item")]
        public List<ItemDataSO> itme = new List<ItemDataSO>();
        public List<int> iCount = new List<int>();
        public List<Vector3> iPos = new List<Vector3>();

        [Header("GoHouse")]
        public GoHouseStageSO goHouse; //집가기

        public void SetMonster(List<MonsterSO> m, List<float> t, List<Vector3> p)
        {
            monster = m;
            mTime = t;
            mPos = p;
        }

        public void SetItem(List<ItemDataSO> m, List<int> c, List<Vector3> p)
        {
            itme = m;
            iCount = c;
            iPos = p;
        }

        public void SetPos(Vector3 cOne, Vector3 cTwo, Vector3 bOne, Vector3 bTwo, Vector3 cCam, Vector3 bCam, Vector3 miniCam,
            Vector2 cCamOffset, Vector2 cCamSize, Vector2 bCamOffset, Vector2 bCamSize, Vector3 miniSize)
        {
            cPlayerOnePos = cOne;
            cPlayerTwoPos = cTwo;
            bPlayerOnePos = bOne;
            bPlayerTwoPos = bTwo;
            cCamPos = cCam;
            bCamPos = bCam;
            miniCamPos = miniCam;
            
            cCamLimitOffset = cCamOffset;
            cCamLimitSize = cCamSize;
            bCamLimitOffset = bCamOffset;
            bCamLimitSize = bCamSize;
            miniCamSize = miniSize;
        }
    }
}