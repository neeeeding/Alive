using System.Collections.Generic;
using UnityEngine;

namespace _02Script.MiniGame.Produce
{
    public class MiniGameObjSpawn : MonoBehaviour
    {
        [SerializeField] protected RectTransform map;
        [SerializeField] protected Transform parent;
        [SerializeField] protected MiniGameObj objPrefab;
        
        protected Vector3 mapMin;
        protected Vector3 mapMax;
        protected float minTime = 0.15f;
        protected float maxTime = 1f;
        protected float curTime;
        protected float setTime;
        
        protected List<MiniGameObj> _spotList = new List<MiniGameObj>();

        public virtual void ObjListAdd(MiniGameObj obj)
        {
            _spotList.Add(obj);
            obj.gameObject.SetActive(false);
        }
        
        protected virtual void Update()
        {
            curTime += Time.deltaTime;
            if (curTime >= setTime)
            {
                NewObj();
                SetTime();
            }
        }
        protected virtual void NewObj()
        {
            MiniGameObj obj;
            if (_spotList.Count <= 0)
            {
                ObjSetting();
            }
            obj = _spotList[0];
            _spotList.RemoveAt(0);
            obj.transform.position = RandomPos();
            obj.gameObject.SetActive(true);
        }

        protected virtual void ObjSetting()
        {
            MiniGameObj obj = Instantiate(objPrefab, parent);
            obj.SetSpawn(this);
            obj.gameObject.SetActive(false);
            _spotList.Add(obj);
        }

        protected virtual void SetTime()
        {
            setTime = Random.Range(minTime, maxTime);
            curTime = 0;
        }

        protected virtual Vector3 RandomPos()
        {
            if (mapMin == null || mapMin == Vector3.zero)
            {
                Rect rect = map.rect;

                Vector3 h = new Vector3(rect.width / 2, rect.height / 2, 0);
                mapMin = new Vector3(map.position.x - h.x, map.position.y - h.y, 0);
                mapMax = new Vector3(map.position.x + h.x, map.position.y + h.y, 0);
            }

            return new Vector3(Random.Range(mapMin.x, mapMax.x), Random.Range(mapMin.y, mapMax.y), 0);
        }
    }
}