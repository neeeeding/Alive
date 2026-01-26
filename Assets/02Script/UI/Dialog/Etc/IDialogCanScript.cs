using _02Script.UI.Dialog.Dialog;

namespace _02Script.UI.Dialog.Etc
{
    public interface IDialogCanScript
    {
        public void Do<T>(T t, DoScriptType type)
        {}
        public void Do(DoScriptType type);
    }
}