using UnityEngine;
using UnityEditor;

namespace InstantSpawner.Util
{
    public static class EditorUtil
    {
        public static void DrawLine(int preSpace = 6, int postSpace = 6)
        {
            GUILayout.Space(preSpace);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            GUILayout.Space(postSpace);
        }

        public static float GetPropertyHeight(SerializedProperty property = null)
        {
            var height = property == null
                ? EditorGUIUtility.singleLineHeight
                : EditorGUI.GetPropertyHeight(property, true);

            return height + EditorGUIUtility.standardVerticalSpacing;
        }

        public static bool FoldoutField(ref Rect rect, SerializedProperty property, string label, string propertyName)
        {
            var prop = property.FindPropertyRelative(propertyName);
            prop.isExpanded = EditorGUI.Foldout(rect, prop.isExpanded, GUIContent.none);
            EditorGUI.PropertyField(rect, prop, new GUIContent(label));
            rect.y += GetPropertyHeight(prop);

            return prop.isExpanded;
        }

        public static void Field(ref Rect rect, SerializedProperty property, string label, string propertyName)
        {
            var prop = property.FindPropertyRelative(propertyName);
            EditorGUI.PropertyField(rect, prop, new GUIContent(label), true);
            rect.y += GetPropertyHeight(prop);
        }

        public static bool CheckBoxField(ref Rect rect, SerializedProperty property, string label, string propertyName)
        {
            var prop = property.FindPropertyRelative(propertyName);
            EditorGUI.PropertyField(rect, prop, new GUIContent(label), true);
            rect.y += GetPropertyHeight(prop);
            return prop.boolValue;
        }

        public static void Label(ref Rect rect, string label)
        {
            EditorGUI.LabelField(rect, new GUIContent(label));
            rect.y += GetPropertyHeight();
        }
    }
}
