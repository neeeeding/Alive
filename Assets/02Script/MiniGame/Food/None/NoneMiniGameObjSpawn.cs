using System;
using System.Collections.Generic;
using _02Script.MiniGame.Food.FryingPan;
using TMPro;
using UnityEngine;

namespace _02Script.MiniGame.Food.None
{
    public class NoneMiniGameObjSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject miniGame;
        [SerializeField] private List<Sprite> images;
        [SerializeField] private Transform spawnPos;
        [SerializeField] private CheckRect center;
        [SerializeField] private NoneMiniGameMoveObj objPrefab;

        private List<NoneMiniGameMoveObj> _poolObj = new List<NoneMiniGameMoveObj>();
        private List<NoneMiniGameMoveObj> _obj = new List<NoneMiniGameMoveObj>();
        private int _curIndex;
        private int _score;

        private void OnEnable()
        {
            _curIndex = 0;
            _score = 0;
            NewMoveObj();
            NoneMiniGameMoveObj.OnFall += GetScore;
        }

        private void OnDisable()
        {
            NoneMiniGameMoveObj.OnFall -= GetScore;
            foreach (var obj in _obj.ToArray())
            {
                _poolObj.Add(obj);
                _obj.Remove(obj);
            }
        }

        //실패 했는지 여부 받고 점수 얻기
        private void GetScore(bool isGet)
        {
            if(isGet) _score++;

            if (images.Count - 1 <= _curIndex)
            {
                CheckScore();
                return;
            }
            NewMoveObj();
        }

        private void CheckScore()
        {
            miniGame.SetActive(false);
            FoodScore.OnEndMiniGame?.Invoke(Math.Max(1,5 - _score));
        }

        private void NewMoveObj() //새 오브젝트 생성
        {
            NoneMiniGameMoveObj obj;
            if (_poolObj.Count <= 0)
            {
                obj = Instantiate(objPrefab, spawnPos);
                _poolObj.Add(obj);
            }
            obj = _poolObj[0];
            _poolObj.RemoveAt(0);

            _obj.Add(obj);
            obj.Setting(images[_curIndex],center, (_curIndex - _score));
            _curIndex++;
            obj.transform.position = spawnPos.position;
            obj.gameObject.SetActive(true);
        }
    }
}