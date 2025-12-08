using System.Collections.Generic;
using _02Script.Player;
using UnityEngine;

namespace _02Script.Farming
{
    //나중에 타일맵에서 그리는 걸로
    public class Field : MonoBehaviour
    {
        [SerializeField] private GameObject SeedWindow;
        
        [SerializeField] private Seeds seedsPrefab;
        
        private List<Seeds>  seeds = new List<Seeds>();

        private bool isField;
        private Vector2 clickPos;

        public void ListSeeds(Seeds seeds)
        {
            this.seeds.Add(seeds);
            seeds.gameObject.SetActive(false);
        }
        
        private void Plant(SeedsSO so)
        {
            if (seeds.Count <= 0)
            {
                NewSeeds(1);
            }

            Seeds newSeeds = seeds[0];
            
            newSeeds.transform.position = clickPos;
            newSeeds.SetSO(so,this);
            newSeeds.gameObject.SetActive(true);
            
            seeds.Remove(seeds[0]);
        }

        private void NewSeeds(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Seeds newSeeds = Instantiate(seedsPrefab);
                newSeeds.gameObject.SetActive(false);
                newSeeds.transform.SetParent(gameObject.transform);
            
                seeds.Add(newSeeds);
            }
        }

        private void SavePos(Vector2 pos)
        {
            clickPos = pos;
        }
        
        public void ClickField()
        {
            SeedWindow.SetActive(true);
        }

        #region EnDiAw
        private void Awake()
        {
            SeedWindow.SetActive(false);
        }

        private void OnEnable()
        {
            PlayerInput.OnMousePos += SavePos;
            SeedsCard.OnClickCard += Plant;
        }

        private void OnDisable()
        {
            PlayerInput.OnMousePos -= SavePos;
            SeedsCard.OnClickCard -= Plant;
        }
        #endregion
    }
}