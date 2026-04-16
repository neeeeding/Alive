using System;
using System.Collections.Generic;
using UnityEngine;

namespace _02Script.MiniGame.Food.None
{
    public class NoneMiniGameObjSpawn : MonoBehaviour
    {
        [SerializeField] private List<Sprite> images;
        [SerializeField] private Transform spawnPos;
        [SerializeField] private NoneMiniGameMoveObj objPrefab;

        private List<NoneMiniGameMoveObj> _objs = new List<NoneMiniGameMoveObj>();
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
        }

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
            FoodScore.OnEndMiniGame?.Invoke(_score/2);
        }

        private void NewMoveObj()
        {
            NoneMiniGameMoveObj obj;
            if (_objs.Count <= 0)
            {
                obj = Instantiate(objPrefab, spawnPos);
                _objs.Add(obj);
            }
            obj = _objs[0];
            obj.Setting(images[_curIndex]);
            _curIndex++;
            obj.transform.position = spawnPos.position;
            obj.gameObject.SetActive(true);
        }
    }
}