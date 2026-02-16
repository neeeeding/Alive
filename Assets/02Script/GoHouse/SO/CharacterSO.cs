using System;
using _02Script.UI.Dialog.Entity;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "CharacterSO", menuName = "SO/GoHouse/Block/CharacterSO")]
    public class CharacterSO : BlockActionSO
    {
        public static Action<DialogEntitySO, int, int> OnChat;
        
        [SerializeField] private DialogEntitySO dialogEntitySo; //나는 누구인가
        [SerializeField] private int chapter; //챕터
        [SerializeField] private int finalNum; //번호
        public override void DoBlockAction()
        {
            OnChat?.Invoke(dialogEntitySo,chapter,finalNum);
        }
    }
}