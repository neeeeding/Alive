using System;
using System.Collections.Generic;
using _02Script.UI.Dialog.Entity;
using _02Script.UI.Dialog.Etc;
using UnityEngine;
using UnityEngine.Rendering;

namespace _02Script.UI.Dialog.Dialog
{
    
    //쓰려면 수정이 필요
    public class DoScript : MonoBehaviour
    {
        //[SerializeField]private ScriptListAllFinderSO allScript;
        
        [SerializeField]private SerializedDictionary<string, List<IDialogCanScript>> scripts;
    
        private void Awake()
        {
            Organize();
        }
    
        public void DoCheck(string st, DialogEntity entity)
        {
            string[] all = st.Split('~');
            foreach (var doScriptName in all)
            {
                string doScript = doScriptName.ToLower();
                if (!scripts.ContainsKey(doScript)) continue;

                if (doScript == DoScriptType.EndDialog.ToString().ToLower()) //삭제
                {
                    Destroy(entity);
                    continue;
                }
                if (scripts[doScript].Count <= 0) continue;
                
                IDialogCanScript script = scripts[doScript][0];
                if (doScript == DoScriptType.DialogDeleteObj.ToString().ToLower()) //삭제
                {
                    for (int i = 0; i < scripts[doScript].Count; i++)
                    {
                        script = scripts[doScript][i];
                        script.Do(entity);
                        if(script == null) continue;
                    }
                }
                script.Do();
            }
        }
        
        private void Organize()
        {
            // scripts = new SerializedDictionary<string, List<IDialogCanScript>>();
            //
            // List<DialogCanScript> targets = allScript.GetTarget<DialogCanScript>();
            // foreach (DoScriptType type in Enum.GetValues(typeof(DoScriptType)))
            // {
            //     List<IDialogCanScript> script = new List<IDialogCanScript>();
            //     foreach (DialogCanScript target in targets)
            //     {
            //         if(target.GetType().Name.ToLower() != type.ToString().ToLower()) continue;
            //         
            //         script.Add(target.GetComponent<IDialogCanScript>());
            //     }
            //     scripts.Add(type.ToString().ToLower(),script);
            // }
        }
    }
    
    public enum DoScriptType
    {
        EndDialog, //대화 종료
        DeleteGameObject, //삭제
        DialogDeleteObj, //대화의 삭제
        MuseumItemShow, //박물관
        ShopUI, //상점
    }
}