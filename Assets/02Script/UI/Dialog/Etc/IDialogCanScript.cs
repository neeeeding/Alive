using System;
using UnityEngine;

namespace _02Script.UI.Dialog.Etc
{
    public interface IDialogCanScript
    {
        public void Do<T>(T t)
        {}
        public void Do();
    }
}