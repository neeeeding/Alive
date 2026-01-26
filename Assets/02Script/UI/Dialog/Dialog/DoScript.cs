using System;
using System.Collections.Generic;
using _02Script.UI.Dialog.Entity;
using _02Script.UI.Dialog.Etc;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.UI.Dialog.Dialog
{
    public class DoScript : MonoBehaviour
    {
        [SerializeField]private SerializedDictionary<EntityName, MonoBehaviour[]> want = new SerializedDictionary<EntityName, MonoBehaviour[]>();
        private SerializedDictionary<EntityName, IDialogCanScript[]> scripts = new SerializedDictionary<EntityName, IDialogCanScript[]>();
    
        private void Awake()
        {
            Organize();
        }
    
        public void DoCheck(string st, DialogEntitySO entity)
        {
            string[] all = st.Split('~');
            foreach (string doScriptName in all)
            {
                DoScriptType doScript = (DoScriptType)Enum.Parse(typeof(DoScriptType), doScriptName);
                //if (!scripts[entity.EntityName].doScripts.ContainsKey(doScript)) continue;

                if (doScript == DoScriptType.EndDialog) //삭제
                {
                    Destroy(entity);
                    continue;
                }
                //if (scripts[entity.EntityName].doScripts.Count <= 0) continue;
                if(scripts.Count <= 0) Organize();
                
                if (scripts[entity.EntityName].Length <= 0) continue;
                
                //IDialogCanScript script = scripts[entity.EntityName].doScripts[doScript];

                foreach (IDialogCanScript script in scripts[entity.EntityName])
                {
                    print(script);
                    script.Do(doScript);
                }
            }
        }
        
        //나중에 찾기 자동화 만들면. (주석)
        private void Organize()
        {
            scripts = new SerializedDictionary<EntityName, IDialogCanScript[]>();
            foreach (KeyValuePair<EntityName, MonoBehaviour[]> w in want)
            {
                List<IDialogCanScript> list = new();
                foreach (MonoBehaviour mb in w.Value)
                {
                    if (mb is IDialogCanScript script)
                        list.Add(script);
                }
                scripts.Add(w.Key, list.ToArray());
                //scripts.Add(w.Key, w.Value as IDialogCanScript[]);
            }
            
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

    [Serializable]
    public class HaveDoScript
    {
        public SerializedDictionary<DoScriptType,IDialogCanScript> doScripts;
    }
    
    public enum DoScriptType
    {
        EndDialog = 1, //대화 종료
        DeleteGameObject, //삭제
        DialogDeleteObj, //대화의 삭제
        DoOpenTogether,
        DoUIActive,
        
        IsisEvent = 1000,
        MagentaEvent,
        RaeliaEvent,
    }
}