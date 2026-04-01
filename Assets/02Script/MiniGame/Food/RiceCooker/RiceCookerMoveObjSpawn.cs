using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.MiniGame.Food.RiceCooker
{
    public class RiceCookerMoveObjSpawn : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<RiceCookerObjType, Sprite> objSprites;
        [SerializeField] private RiceCookerScore score;
        [SerializeField] private Transform parent;
        [SerializeField] private RiceCookerMoveObj objPrefab;
        [SerializeField] private float baseY = 400;
        
        private readonly int _maxObjCount = 10; //나와야 하는 벌레, 나뭇가지 합
        private readonly float _minXpos = 0;
        private readonly float _maxXpos = 1920;
        private readonly float _minTime = 0.2f;
        private readonly float _maxTime = 1f;
        private int _curObjCount;
        private float _curTime;
        private float _setTime;
        
        private List<RiceCookerMoveObj> _moveObjList = new List<RiceCookerMoveObj>();

        private void OnEnable()
        {
            Time.timeScale = 1;
            SetTime();
            _curObjCount = 0;
        }

        public void MoveObjListAdd(RiceCookerMoveObj obj)
        {
            if (_moveObjList.Contains(obj)) return;
            
            _moveObjList.Add(obj);
            obj.gameObject.SetActive(false);
            SetTime();
        }

        private void Update()
        {
            _curTime += Time.deltaTime;
            if (_curTime >= _setTime)
            {
                NewMoveObj();
                SetTime();
            }
        }

        private void NewMoveObj()
        {
            RiceCookerMoveObj moveObj;
            if (_moveObjList.Count <= 0)
            {
                moveObj = Instantiate(objPrefab, parent);
                moveObj.SetObj(this, score);

                moveObj.gameObject.SetActive(false);
                _moveObjList.Add(moveObj);
            }
            moveObj = _moveObjList[0];
            RiceCookerObjType type = SetType();
            moveObj.Setting(objSprites[type],type);
            _moveObjList.RemoveAt(0);
            moveObj.transform.position = RandomPos();
            
            moveObj.gameObject.SetActive(true);
        }

        private RiceCookerObjType SetType()
        {
            int r = Random.Range((int)RiceCookerObjType.Rice, (int)RiceCookerObjType.Tree);

            if (r == (int)RiceCookerObjType.Worm || r == (int)RiceCookerObjType.Tree)
            {
                if(_curObjCount >= _maxObjCount)
                    r = Random.Range((int)RiceCookerObjType.Rice, (int)RiceCookerObjType.Bean);
                else
                    _curObjCount++;
            }
            
            return (RiceCookerObjType)r;
        }

        private void SetTime()
        {
            _setTime = Random.Range(_minTime, _maxTime);
            _curTime = 0;
        }

        private Vector3 RandomPos()
        {
            return new Vector3(Random.Range(_minXpos,_maxXpos),baseY, 0);
        }
    }
}