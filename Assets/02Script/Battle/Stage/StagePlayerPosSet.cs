using System;
using UnityEditor;
using UnityEngine;

namespace _02Script.Battle.Stage
{
    public class StagePlayerPosSet : MonoBehaviour
    {
        [Header("Show")]
        [SerializeField] private BattleStageSO curStage;
        
        [Header("PlayerPos")]
        public Transform cPlayerOnePos;
        public Transform cPlayerTwoPos;
        public Transform bPlayerOnePos;
        public Transform bPlayerTwoPos;
        
        [Header("CamPos")]
        public Transform cCamPos;
        public Transform bCamPos;
        public Transform miniCamPos;
        
        public BoxCollider2D cCamLimit;
        public BoxCollider2D bCamLimit;

        private void Awake()
        {
            SetPos();
        }

        private void SetPos()
        {
            curStage.SetPos(cPlayerOnePos.position, cPlayerTwoPos.position, bPlayerOnePos.position, bPlayerTwoPos.position, cCamPos.position,bCamPos.position, miniCamPos.position,
                cCamLimit.offset,cCamLimit.size, bCamLimit.offset,bCamLimit.size,miniCamPos.localScale);
            print("ok SetPos");
#if UNITY_EDITOR
            EditorUtility.SetDirty(curStage);
            AssetDatabase.SaveAssets();
#endif
            gameObject.SetActive(false);
        }
    }
}