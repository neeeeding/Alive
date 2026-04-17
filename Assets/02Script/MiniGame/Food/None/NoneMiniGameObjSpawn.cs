using System.Collections.Generic;
using _02Script.MiniGame.Food.FryingPan;
using UnityEngine;

namespace _02Script.MiniGame.Food.None
{
    public class NoneMiniGameObjSpawn : MonoBehaviour
    {
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
            if (_poolObj.Count <= 0)
            {
                obj = Instantiate(objPrefab, spawnPos);
                _poolObj.Add(obj);
            }
            obj = _poolObj[0];
            _poolObj.RemoveAt(0);
            CheckRect rect = center;
            if (_obj.Count > 1)
            {
                rect = _obj[_obj.Count - 1];
            }
            _obj.Add(obj);
            obj.Setting(images[_curIndex],rect);
            _curIndex++;
            obj.transform.position = spawnPos.position;
            obj.gameObject.SetActive(true);
        }
    }
}