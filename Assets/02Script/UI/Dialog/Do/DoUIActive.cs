using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Etc;
using _02Script.UI.Etc;

namespace _02Script.UI.Dialog.Do
{
    public class DoUIActive : UIActive, IDialogCanScript
    {
        public void Do(DoScriptType type)
        {
            if(type != DoScriptType.DoUIActive) return;
            Show();
        }
    }
}