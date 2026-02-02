using _02Script.Obj.Entity;
using UnityEngine;

namespace _02Script.UI.Dialog.Entity
{
    [CreateAssetMenu(fileName = "CharacterSO", menuName = "SO/Entity/Dialog")]
    public class DialogEntitySO : EntitySO
    {
        [Space(20f)]
        [Header("Dialog")]
        public TextAsset[] DialogTextFile;
    }
}