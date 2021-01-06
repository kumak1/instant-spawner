using InstantSpawner.Util;
using UnityEditor;
using UnityEngine;

namespace InstantSpawner.Model
{
    [CustomPropertyDrawer(typeof(SpawnSetting))]
    public class SpawnSettingDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            if (!EditorUtil.FoldoutField(ref position, property, "Prefab", "prefab")) return;

            EditorGUI.indentLevel++;

            EditorUtil.Label(ref position, "Transform");
            EditorGUI.indentLevel++;
            EditorUtil.Field(ref position, property, "Position", "position");
            EditorUtil.Field(ref position, property, "Rotate", "rotate");
            EditorGUI.indentLevel--;

            if (EditorUtil.CheckBoxField(ref position, property, "Replace Material Texture", "useReplaceMaterial"))
            {
                EditorGUI.indentLevel++;
                EditorUtil.Field(ref position, property, "Material Params", "materialParams");
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var enableSkinName = property.FindPropertyRelative("prefab");
            var height = EditorUtil.GetPropertyHeight(enableSkinName);

            if (!enableSkinName.isExpanded)
                return height;

            height += EditorUtil.GetPropertyHeight();
            height += EditorUtil.GetPropertyHeight(property.FindPropertyRelative("position"));
            height += EditorUtil.GetPropertyHeight(property.FindPropertyRelative("rotate"));

            var useReplaceMaterial = property.FindPropertyRelative("useReplaceMaterial");
            height += EditorUtil.GetPropertyHeight(useReplaceMaterial);

            if (useReplaceMaterial.boolValue)
            {
                height += EditorUtil.GetPropertyHeight(property.FindPropertyRelative("materialParams"));
            }

            return height;
        }
    }
}