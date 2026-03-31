using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.MiniGame.Produce
{
    public class ProduceSpotSpawn : MonoBehaviour
    {
        [SerializeField] private RectTransform map;
        [SerializeField] private ProduceScore score;
        [SerializeField] private Transform parent;
        [SerializeField] private ProduceSpot spotPrefab;

        private Vector3 _mapMin;
        private Vector3 _mapMax;
        private readonly float _minTime = 0.15f;
        private readonly float _maxTime = 1f;
        private float _curTime;
        private float _setTime;
        
        private List<ProduceSpot> _spots = new List<ProduceSpot>();

        public void SpotListAdd(ProduceSpot obj)
        {
            _spots.Add(obj);
            obj.gameObject.SetActive(false);
            SetTime();
        }

        private void Update()
        {
            _curTime += Time.deltaTime;
            if (_curTime >= _setTime)
            {
                NewSpot();
                SetTime();
            }
        }

        private void NewSpot()
        {
            ProduceSpot spot;
            if (_spots.Count <= 0)
            {
                spot = Instantiate(spotPrefab, parent);
                spot.SetSpot(this, score);
                spot.gameObject.SetActive(false);
                _spots.Add(spot);
            }
            spot = _spots[0];
            _spots.RemoveAt(0);
            spot.transform.position = RandomPos();
            spot.gameObject.SetActive(true);
        }

        private void SetTime()
        {
            _setTime = Random.Range(_minTime, _maxTime);
            _curTime = 0;
        }

        private Vector3 RandomPos()
        {
            if (_mapMin == null || _mapMin == Vector3.zero)
            {
                Rect rect = map.rect;

                Vector3 h = new Vector3(rect.width / 2, rect.height / 2, 0);
                _mapMin = new Vector3(map.position.x - h.x, map.position.y - h.y, 0);
                _mapMax = new Vector3(map.position.x + h.x, map.position.y + h.y, 0);
            }

            return new Vector3(Random.Range(_mapMin.x, _mapMax.x), Random.Range(_mapMin.y, _mapMax.y), 0);
        }
    }
}