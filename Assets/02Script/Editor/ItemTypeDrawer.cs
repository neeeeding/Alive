using System.Collections.Generic;
using System.Linq;
using _02Script.Inventory.Item;
using UnityEditor;
using UnityEngine;

namespace _02Script.Editor
{
    [CustomPropertyDrawer(typeof(ItemType))]
    public class ItemTypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty category = property.serializedObject.FindProperty("category");

            if (category == null)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            int cat = category.enumValueIndex;

            int min = cat * 1000;
            int max = min + 999;

            List<ItemType> filtered = new List<ItemType>();

            foreach (ItemType type in System.Enum.GetValues(typeof(ItemType)))
            {
                int v = (int)type;
                if (v >= min && v <= max)
                    filtered.Add(type);
            }

            string[] names = filtered.Select(x => x.ToString()).ToArray();
            int[] values = filtered.Select(x => (int)x).ToArray();

            int index = System.Array.IndexOf(values, property.intValue);
            if (index < 0) index = 0;

            index = EditorGUI.Popup(position, label.text, index, names);

            property.intValue = values[index];
        }
    }
}