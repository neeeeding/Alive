using System;
using _02Script.Obj.Entity;
using _02Script.UI.Etc;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.UI.Job
{
    public class SelectCharacterCard : WindowMove
    {
        public static Action OnMouseUp;
        
        public static SelectCharacterCard curSelectCharacter;
        
        [SerializeField] private EntityName character;
        [SerializeField] private Transform dragTransform;
        [SerializeField] private Transform baseTransform;
        
        public EntityName Character{get => character;}
        public SelectCharacterType Select{get => _select;}
        
        private Image _image;
        private SelectCharacterType _select;
        private Transform _parent;

        private void OnEnable()
        {
            _select = SelectCharacterType.None;
            _image = moveObj.GetComponent<Image>();
        }

        public void SetJob(SelectCharacterType job,Transform parent)
        {
            _select = job;
            _parent = parent;
            if (_parent == null)
            {
                moveObj.transform.SetParent(baseTransform);
            }
        }

        public override void MouseClick()
        {
            moveObj.transform.SetParent(dragTransform);
            base.MouseClick();
            _image.raycastTarget = false;
            curSelectCharacter = this;
        }

        public override void MouseCancel()
        {
            OnMouseUp?.Invoke();
            base.MouseCancel();
            _image.raycastTarget = true;
            curSelectCharacter = null;

            if (_parent != null)
            {
                moveObj.transform.SetParent(_parent);
            }
            else
            {
                moveObj.transform.SetParent(baseTransform);
            }
        }
    }

    public enum SelectCharacterType
    {
        None,
        Battle,
        Collect
    }
}