using _02Script.Obj.Obj;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Etc;

namespace _02Script.UI.Dialog.Do
{
    public class DoOpenTogether : OpenTogether, IDialogCanScript
    {
        public void Do(DoScriptType type)
        {
            if(type != DoScriptType.DoOpenTogether) return;
            obj.SetActive(true);
        }
    }
}