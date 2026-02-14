using System.Linq;
using _02Script.GoHouse.SO;
using _02Script.GoHouse.Stage;
using UnityEditor;
using UnityEngine;

namespace _02Script.Editor
{
    [CustomEditor(typeof(StageSO))]
    public class StageSOInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("스테이지 에디터 열기", GUILayout.Height(28)))
                {
                    StageSO stage = (StageSO)target;
                    StageSOEditorWindow.OpenWith(stage);
                }
            }
            EditorGUILayout.HelpBox("스테이지를 에디터에서 수정 가능합니다.", MessageType.Info);

            if (GUILayout.Button("빈 부분을 none로 채우기"))
            {
                StageSO stage = (StageSO)target;
                ConvertNullToAir(stage);
            }
        }

        private void ConvertNullToAir(StageSO stage)
        {
            string[] guids = AssetDatabase.FindAssets("t:GridDataSO");
            BlockSO noneBlock = guids
                .Select(g => AssetDatabase.LoadAssetAtPath<BlockSO>(AssetDatabase.GUIDToAssetPath(g)))
                .FirstOrDefault(a => a != null && a.blockType == BlockType.None);

            if (noneBlock == null)
            {
                Debug.LogError("빈 블럭에 대해 스크립트를 수정하세요.");
                return;
            }

            Undo.RecordObject(stage, "Convert Null To none");
            bool changed = false;
            foreach (Row row in stage.stageBlocks)
            {
                for (int i = 0; i < row.columns.Count; i++)
                {
                    if (row.columns[i] == null)
                    {
                        row.columns[i] = noneBlock;
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                EditorUtility.SetDirty(stage);
                AssetDatabase.SaveAssets();
                Debug.Log($"{stage.name}: 빈 부분을 none로 채웠습니다!");
            }
        }
    }
}