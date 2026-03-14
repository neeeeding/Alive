using _02Script.GoHouse.Block;
using _02Script.GoHouse.SO;
using UnityEngine;

namespace _02Script.GoHouse.UI
{
    public class SkipBtn : MonoBehaviour
    {
        [SerializeField] private GameObject btn;

        private void OnEnable()
        {
            BlockPlayer.OnReSet += Show;
            DieSO.OnDie += Show;
        }

        private void OnDisable()
        {
            BlockPlayer.OnReSet -= Show;
            DieSO.OnDie -= Show;
        }

        private void Start()
        {
            btn.SetActive(false);
        }

        private void Show()
        {
            btn.SetActive(true);
        }
    }
}