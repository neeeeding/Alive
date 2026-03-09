using System;
using System.Linq;
using _02Script.Etc;
using _02Script.GoHouse.SO;
using _02Script.GoHouse.Stage;
using UnityEditor;
using UnityEngine;

namespace _02Script.Editor
{
    public class StageSOEditorWindow : EditorWindow
    {
        //스테이지 정보
        private GoHouseStageSO _stage;
        private SerializedObject _serializedStage; //지금 수정 할 스테이지 so (를 감싸는 편집기)
        private SerializedProperty _gridRowsProp; //스테이지 맵의 칸 (스테이지 so의 특정 코드)

        //블록
        private BlockSO[] _allBrushes = Array.Empty<BlockSO>();
        private BlockSO _noneBlockSO;
        private int _selectedBrushIndex = -1; // -1 : 지우개
        private Vector2 _paletteScroll;

        //그리드
        private float _cellSize = 56f;
        private float _cellPadding = 4f;
        private Vector2 _gridScroll;

        #region Open
        [MenuItem("Custom/GoHouse Stage Editor")]
        public static void OpenEmpty()
        {
            StageSOEditorWindow window = GetWindow<StageSOEditorWindow>("GoHouse Stage Editor");
            window.Show();
        }

        //더블 클릭으로 열기
        public static void OpenWith(GoHouseStageSO stage)
        {
            StageSOEditorWindow window = GetWindow<StageSOEditorWindow>("GoHouse Stage Editor");
            window.SetTarget(stage);
            window.Show();
        }
        #endregion

        #region EditorWindow
        private void OnEnable()
        {
            SettingBlockBrush();
            wantsMouseMove = true;
        }
        private void OnFocus() //창이 포커스를 얻을 때
        {
            SettingBlockBrush();
        }
        #endregion

        #region Setting
        private void SetTarget(GoHouseStageSO stage) //수정할 스테이지 정하기
        {
            _stage = stage;
            if (_stage != null)
            {
                _serializedStage = new SerializedObject(_stage);
                _gridRowsProp = _serializedStage.FindProperty("stageBlocks");
                EnsureGridInitialized(); //1 * 1 안전 장치
                return;
            }
            _serializedStage = null;
            _gridRowsProp = null;
        }
        private void SettingBlockBrush() //블럭들 세팅
        {
            string[] guids = AssetDatabase.FindAssets("t:BlockSO"); //해당 타입들을 전부 찾음
            _allBrushes = guids
                .Select(g => AssetDatabase.LoadAssetAtPath<BlockSO>(AssetDatabase.GUIDToAssetPath(g)))
                .Where(a => a != null)
                .OrderBy(a => a.blockType)
                .ToArray();

            _noneBlockSO = _allBrushes.FirstOrDefault(a => a.blockType == BlockType.None);

            //삭제로 인해 인덱스가 초과 될 때
            if (_selectedBrushIndex >= _allBrushes.Length)
                _selectedBrushIndex = -1;
        }
        #endregion

        #region GUI
        private void OnGUI()
        {
            DrawHeader();
            EditorGUILayout.Space(4);

            using (new EditorGUILayout.HorizontalScope())
            {
                DrawPalettePanel();
                DrawGridPanel();
                DrawStageSettingPanel();
            }
        }

        private void DrawHeader()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox)) //묶기 위한
            {
                GoHouseStageSO newStage = (GoHouseStageSO)EditorGUILayout.ObjectField("수정 중인 스테이지 : ", _stage, typeof(GoHouseStageSO), false);
                if (newStage != _stage)
                {
                    SetTarget(newStage);
                }

                if (GUILayout.Button("새로고침", GUILayout.Width(80)))
                {
                    SettingBlockBrush();
                }

                if (GUILayout.Button("새 SO", GUILayout.Width(80)))
                {
                    CreateNewStageSO();
                }

                GUILayout.FlexibleSpace();

                _cellSize = EditorGUILayout.Slider(new GUIContent("셀 크기"), _cellSize, 24f, 96f, GUILayout.Width(250));
            }

            if (_stage == null)
            {
                EditorGUILayout.HelpBox("편집할 StageSO를 선택해주세요.", MessageType.Info);
            }
        }

        private void DrawPalettePanel()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(260)))
            {
                EditorGUILayout.LabelField("브러시 팔레트", EditorStyles.boldLabel);

                //지우개 선택
                bool eraseSelected = _selectedBrushIndex == -1;
                GUIStyle eraseStyle = new GUIStyle(EditorStyles.miniButton);
                eraseStyle.normal.textColor = eraseSelected ? Color.white : EditorStyles.miniButton.normal.textColor;
                Color eraseBg = eraseSelected ? EditorGUIUtility.isProSkin ? new Color(0.2f,0.5f,0.2f) : new Color(0.5f,0.8f,0.5f) : GUI.backgroundColor;
                Color prevBg = GUI.backgroundColor;
                GUI.backgroundColor = eraseBg;
                if (GUILayout.Button("지우개 (none)", eraseStyle))
                {
                    _selectedBrushIndex = -1;
                }
                GUI.backgroundColor = prevBg;

                _paletteScroll = EditorGUILayout.BeginScrollView(_paletteScroll, GUILayout.ExpandHeight(true));
                {
                    foreach ((BlockSO brush, int index) in _allBrushes.Select((b, i) => (b, i)))
                    {
                        using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
                        {
                            bool selected = index == _selectedBrushIndex;
                            if (GUILayout.Toggle(selected, GUIContent.none, GUILayout.Width(20)))
                            {
                                _selectedBrushIndex = index;
                            }

                            Texture2D preview = AssetPreview.GetAssetPreview(brush.blockImage) ?? AssetPreview.GetMiniThumbnail(brush);
                            GUILayout.Label(preview, GUILayout.Width(32), GUILayout.Height(32));
                            using (new EditorGUILayout.VerticalScope())
                            {
                                EditorGUILayout.LabelField($"{brush.name} ({EnumToString.Name(brush.blockType)})", EditorStyles.boldLabel);
                            }
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawGridPanel()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                EditorGUILayout.LabelField("그리드", EditorStyles.boldLabel);

                if (_stage == null || _serializedStage == null)
                {
                    EditorGUILayout.HelpBox("Stage를 선택하면 그리드를 편집할 수 있습니다.", MessageType.None);
                    return;
                }

                _serializedStage.Update();

                DrawGridSizeControls();

                EditorGUILayout.Space(4);
                float gridRectHeight = Mathf.Min(position.height - 220f, 10000f);
                _gridScroll = EditorGUILayout.BeginScrollView(_gridScroll, GUILayout.Height(gridRectHeight));
                {
                    DrawGridCanvas();
                }
                EditorGUILayout.EndScrollView();

                if (GUI.changed)
                {
                    _serializedStage.ApplyModifiedProperties();
                    EditorUtility.SetDirty(_stage);
                }
            }
        }
        private void DrawStageSettingPanel()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(250)))
            {
                EditorGUILayout.LabelField("스테이지 설정", EditorStyles.boldLabel);
                EditorGUILayout.Space(8);

                if (_stage == null || _serializedStage == null)
                {
                    EditorGUILayout.HelpBox("Stage를 선택하세요.", MessageType.None);
                    return;
                }

                _serializedStage.Update();

                SerializedProperty stageNameProp = _serializedStage.FindProperty("stageName");
                EditorGUILayout.PropertyField(stageNameProp, new GUIContent("스테이지 이름"));

                SerializedProperty moveCountProp = _serializedStage.FindProperty("moveCount");
                EditorGUILayout.PropertyField(moveCountProp, new GUIContent("무브 카운트"));

                SerializedProperty playerPosProp = _serializedStage.FindProperty("playerPos");
                EditorGUILayout.LabelField("플레이어 시작 위치");
                EditorGUI.indentLevel++;

                SerializedProperty xProp = playerPosProp.FindPropertyRelative("x");
                SerializedProperty yProp = playerPosProp.FindPropertyRelative("y");

                xProp.floatValue = EditorGUILayout.IntField("X", (int)xProp.floatValue);
                yProp.floatValue = EditorGUILayout.IntField("Y", (int)yProp.floatValue);

                EditorGUI.indentLevel--;

                _serializedStage.ApplyModifiedProperties();
            }
        }
        #endregion

        #region Grid
        private void DrawGridSizeControls()
        {
            // 현재 사이즈 읽기
            int rows = _gridRowsProp != null ? _gridRowsProp.arraySize : 1;
            int cols = GetMaxColumns();

            using (new EditorGUILayout.HorizontalScope())
            {
                int newRows = EditorGUILayout.IntField(rows, GUILayout.Width(60));
                GUILayout.Label("x");
                int newCols = EditorGUILayout.IntField(cols, GUILayout.Width(60));

                // if (GUILayout.Button($"{newRows} * {newCols} 적용", GUILayout.Width(60)))
                // {
                //     ResizeGrid(newRows, newCols);
                // }

                if (GUILayout.Button("모두 비우기", GUILayout.Width(100)))
                {
                    ClearAll();
                }
            }
        }

        private int GetMaxColumns()
        {
            if (_gridRowsProp == null) return 0;
            int max = 0;
            for (int r = 0; r < _gridRowsProp.arraySize; r++)
            {
                SerializedProperty rowProp = _gridRowsProp.GetArrayElementAtIndex(r);
                SerializedProperty colsProp = rowProp.FindPropertyRelative("columns");
                if (colsProp != null) max = Mathf.Max(max, colsProp.arraySize);
            }
            return max;
        }

        private void EnsureGridInitialized()
        {
            if (_gridRowsProp == null) return;
            if (_gridRowsProp.arraySize == 0)
            {
                _serializedStage.Update();
                _gridRowsProp.arraySize = 1;
                SerializedProperty rowProp = _gridRowsProp.GetArrayElementAtIndex(0);
                SerializedProperty colsProp = rowProp.FindPropertyRelative("columns");
                colsProp.arraySize = 1;
                _serializedStage.ApplyModifiedProperties();
            }
        }

        private void ResizeGrid(int newRows, int newCols)
        {
            newRows = Mathf.Max(1, newRows);
            newCols = Mathf.Max(1, newCols);

            Undo.RecordObject(_stage, "Resize Grid"); //언도를 위해 기억하기
            _serializedStage.Update();

            _gridRowsProp.arraySize = newRows;
            for (int r = 0; r < newRows; r++)
            {
                SerializedProperty rowProp = _gridRowsProp.GetArrayElementAtIndex(r);
                SerializedProperty colsProp = rowProp.FindPropertyRelative("columns");
                if (colsProp == null) continue;
                
                int oldSize = colsProp.arraySize;
                colsProp.arraySize = newCols;
                
                if (newCols > oldSize)
                {
                    for (int i = oldSize; i < newCols; i++)
                    {
                        colsProp.GetArrayElementAtIndex(i).objectReferenceValue = _noneBlockSO;
                    }
                }
            }

            _serializedStage.ApplyModifiedProperties();
        }

        private void ClearAll()
        {
            if (_gridRowsProp == null) return;
            Undo.RecordObject(_stage, "Clear Grid");
            _serializedStage.Update();
            for (int r = 0; r < _gridRowsProp.arraySize; r++)
            {
                SerializedProperty rowProp = _gridRowsProp.GetArrayElementAtIndex(r);
                SerializedProperty colsProp = rowProp.FindPropertyRelative("columns");
                for (int c = 0; c < colsProp.arraySize; c++)
                {
                    colsProp.GetArrayElementAtIndex(c).objectReferenceValue = _noneBlockSO;
                }
            }
            _serializedStage.ApplyModifiedProperties();
        }

        private void DrawGridCanvas()
        {
            if (_gridRowsProp == null) return;

            int rows = _gridRowsProp.arraySize;
            int cols = GetMaxColumns();

            GUIStyle rowStyle = new GUIStyle();
            rowStyle.margin = new RectOffset(0, 0, 0, 0);

            for (int r = 0; r < rows; r++)
            {
                using (new EditorGUILayout.HorizontalScope(rowStyle))
                {
                    SerializedProperty rowProp = _gridRowsProp.GetArrayElementAtIndex(r);
                    SerializedProperty colsProp = rowProp.FindPropertyRelative("columns");
                    int rowCols = colsProp != null ? colsProp.arraySize : 0;

                    for (int c = 0; c < cols; c++)
                    {
                        object cellRef = null;
                        if (c < rowCols)
                        {
                            SerializedProperty cellProp = colsProp.GetArrayElementAtIndex(c);
                            cellRef = cellProp.objectReferenceValue;
                        }

                        DrawCell(r, c, (BlockSO)cellRef, colsProp, c);
                    }
                }
                GUILayout.Space(_cellPadding);
            }
        }
        #endregion
        
        private void CreateNewStageSO()
        {
            string folderPath = "Assets/03SO/GoHouse/StageSO";

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogError("폴더가 존재하지 않습니다: " + folderPath);
                return;
            }

            GoHouseStageSO newStage = ScriptableObject.CreateInstance<GoHouseStageSO>();

            string path = AssetDatabase.GenerateUniqueAssetPath(
                folderPath + "/NewStage.asset");

            AssetDatabase.CreateAsset(newStage, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = newStage;

            _stage = newStage;
            _serializedStage = new SerializedObject(newStage);
            SetTarget(newStage);
        }

        #region Draw
        private void DrawCell(int r, int c, BlockSO cell, SerializedProperty colsProp, int cellIndex)
        {
            Rect rect = GUILayoutUtility.GetRect(_cellSize, _cellSize, GUILayout.Width(_cellSize), GUILayout.Height(_cellSize));

            // 배경
            EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f, 1f));

            // 스프라이트 또는 색
            if (cell != null)
            {
                Sprite sprite = cell.blockImage;
                Texture2D tex = null;
                if (sprite != null)
                {
                    tex = AssetPreview.GetAssetPreview(sprite) ?? AssetPreview.GetMiniThumbnail(sprite);
                }

                if (tex != null)
                {
                    float pad = 4f;
                    Rect draw = new Rect(rect.x + pad, rect.y + pad, rect.width - pad * 2, rect.height - pad * 2);
                    GUI.DrawTexture(draw, tex, ScaleMode.ScaleToFit);
                }
                else
                {
                    int a = (int)cell.blockType / 100 + (int)cell.blockType%100;
                    EditorGUI.DrawRect(rect, a * new Color(0.1f, 0.1f, 0.1f, 1f));
                }
            }
            else
            {
                // 빈 칸 표시용 체커 패턴
                DrawChecker(rect);
            }

            // 그리드 라인
            Handles.color = new Color(0, 0, 0, 0.4f);
            Handles.DrawAAPolyLine(2f, new Vector3(rect.xMin, rect.yMin), new Vector3(rect.xMax, rect.yMin));
            Handles.DrawAAPolyLine(2f, new Vector3(rect.xMin, rect.yMin), new Vector3(rect.xMin, rect.yMax));
            Handles.DrawAAPolyLine(2f, new Vector3(rect.xMax, rect.yMin), new Vector3(rect.xMax, rect.yMax));
            Handles.DrawAAPolyLine(2f, new Vector3(rect.xMin, rect.yMax), new Vector3(rect.xMax, rect.yMax));

            // 툴팁
            if (Event.current.type == EventType.Repaint && cell != null)
            {
                GUI.Label(rect, new GUIContent("", $"{EnumToString.Name(cell.blockType)} ({cell.name})"));
            }

            // 입력 처리
            Event e = Event.current;
            if (e.isMouse && rect.Contains(e.mousePosition) && (e.type == EventType.MouseDown || (e.type == EventType.MouseDrag && e.button == 0)))
            {
                if (e.button == 0)
                {
                    // 좌클릭: 페인트
                    PaintCell(colsProp, cellIndex);
                    e.Use();
                }
                else if (e.button == 1)
                {
                    // 우클릭: 지우개
                    EraseCell(colsProp, cellIndex);
                    e.Use();
                }
            }
        }

        private void PaintCell(SerializedProperty colsProp, int index)
        {
            if (colsProp == null || index < 0 || index >= colsProp.arraySize) return;
            Undo.RecordObject(_stage, "Paint Cell");
            _serializedStage.Update();

            BlockSO value = _selectedBrushIndex >= 0 && _selectedBrushIndex < _allBrushes.Length
                ? _allBrushes[_selectedBrushIndex]
                : _noneBlockSO;

            colsProp.GetArrayElementAtIndex(index).objectReferenceValue = value;
            _serializedStage.ApplyModifiedProperties();
        }

        private void EraseCell(SerializedProperty colsProp, int index)
        {
            if (colsProp == null || index < 0 || index >= colsProp.arraySize) return;
            Undo.RecordObject(_stage, "Erase Cell");
            _serializedStage.Update();
            colsProp.GetArrayElementAtIndex(index).objectReferenceValue = _noneBlockSO;
            _serializedStage.ApplyModifiedProperties();
        }

        private void DrawChecker(Rect rect)
        {
            Color c1 = new Color(0.22f, 0.22f, 0.22f, 1f);
            Color c2 = new Color(0.28f, 0.28f, 0.28f, 1f);
            float size = 8f;
            for (float y = rect.yMin; y < rect.yMax; y += size)
            {
                for (float x = rect.xMin; x < rect.xMax; x += size)
                {
                    Rect r = new Rect(x, y, size, size);
                    bool toggle = (((int)((x - rect.xMin) / size) + (int)((y - rect.yMin) / size)) % 2) == 0;
                    EditorGUI.DrawRect(r, toggle ? c1 : c2);
                }
            }
        }
        #endregion
    }
}