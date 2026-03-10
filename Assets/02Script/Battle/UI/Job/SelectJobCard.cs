using System.Collections.Generic;
using _02Script.Obj.Entity;
using UnityEngine;

namespace _02Script.Battle.UI.Job
{
    public class SelectJobCard : MonoBehaviour
    {
        [SerializeField] private SelectCharacterType job;
        [SerializeField] private int maxCount = 2;
        private List<SelectCharacterCard> _select = new List<SelectCharacterCard>();

        private bool _isEnter;

        private void OnEnable()
        {
            _isEnter = false;
        }
        
        public (SelectCharacterType,List<EntityName>) ReturnCard()
        {
            List<EntityName> names = new List<EntityName>();

            foreach (SelectCharacterCard c in _select)
            {
                names.Add(c.Character);
            }
            
            return (job, names);
        }

        public void MouseClick()
        {
            if(_isEnter) return;
            _isEnter = true;
            if (SelectCharacterCard.curSelectCharacter != null)
            {
                if (_select.Count >= maxCount)
                {
                    _select[0].SetJob(SelectCharacterType.None,null);
                    _select.RemoveAt(0);
                }
                SelectCharacterCard.curSelectCharacter.SetJob(job,transform);
                _select.Add(SelectCharacterCard.curSelectCharacter);
            }
        }

        public void MouseCancel()
        {
            if(!_isEnter) return;
            _isEnter = false;
            if (SelectCharacterCard.curSelectCharacter != null)
            {
                SelectCharacterCard.curSelectCharacter.SetJob(SelectCharacterType.None,null);
                _select.Remove(SelectCharacterCard.curSelectCharacter);
            }
        }
    }
}