using UnityEngine;

namespace _02Script.UI.Map
{
    public class DeleteAllChild : MonoBehaviour
    {
        [SerializeField] private GameObject parentObject;
        [SerializeField] private MapMarkInGame map;

        public void AllDelete()
        {
            foreach (Transform child in parentObject.transform)
            {
                Destroy(child.gameObject);

                map.ResetMapMarkNum();
            }
        }
    }
}
