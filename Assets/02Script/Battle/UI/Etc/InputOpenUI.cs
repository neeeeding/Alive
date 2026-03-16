using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Battle.UI.Etc
{
    public class InputOpenUI : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<string,GameObject> window;
        
        [SerializeField] private SkillBtn rBtn;
        [SerializeField] private SkillBtn fBtn;
        
        [SerializeField] private BattleInput input;
        private void OnEnable()
        {
            input.OnInventoryInput += KeyboardInput;
            input.OnSkillInput += Skill;
            input.OnWeaponInput += KeyboardInput;
            input.OnFoodInput += KeyboardInput;

            foreach (KeyValuePair<string, GameObject> value in window)
            {
                value.Value.SetActive(false);
            }
        }

        private void OnDisable()
        {
            input.OnInventoryInput -= KeyboardInput;
            input.OnSkillInput -= Skill;
            input.OnWeaponInput -= KeyboardInput;
            input.OnFoodInput -= KeyboardInput;
        }

        private void KeyboardInput(string value)
        {
            window[value].SetActive(!window[value].activeSelf);
        }

        private void Skill(string value)
        {
            if (value == "R")
            {
                rBtn.UseSkill();
            }
            else
            {
                fBtn.UseSkill();
            }
        }
    }
}