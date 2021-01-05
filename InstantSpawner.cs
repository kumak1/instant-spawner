using System.Diagnostics;
using InstantSpawner.Util;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace InstantSpawner
{
    using Model;

    public class InstantSpawner : EditorWindow
    {
        private const string ParentName = "RespawnObjects";
        public SpawnParam spawnParam;

        [MenuItem("Tools/Instant Spawner")]
        public static void ShowWindow()
        {
            var editorWindow = GetWindow(typeof(InstantSpawner));
            editorWindow.autoRepaintOnSceneChange = true;
            editorWindow.Show();
        }

#if UNITY_EDITOR
        [Conditional("UNITY_EDITOR")]
        public void DoDestroyImmediateObject(string objectName) =>
            EditorApplication.delayCall += () => DestroyImmediate(GameObject.Find(objectName));
#endif

        public void OnGUI()
        {
            EditorGUILayout.Space();

            spawnParam = EditorGUILayout.ObjectField(
                new GUIContent("Spawn Param"),
                spawnParam,
                typeof(SpawnParam),
                false
            ) as SpawnParam;

            EditorUtil.DrawLine();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();

            if (GUILayout.Button("Respawn", GUILayout.MaxWidth(160), GUILayout.MinHeight(40)))
                Respawn();

            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }

        private void Respawn()
        {
            if (spawnParam == null) return;

            EditorApplication.delayCall += () => DestroyImmediate(GameObject.Find(ParentName));
            EditorApplication.delayCall += () =>
            {
                var parent = new GameObject(ParentName);
                foreach (var setting in spawnParam.spawnSettings)
                {
                    if (setting.prefab == null) continue;

                    var obj = Instantiate(setting.prefab);
                    obj.transform.parent = parent.transform;
                    obj.transform.position = setting.position;
                    obj.transform.rotation = Quaternion.Euler(setting.rotate);
                    obj.transform.localScale = setting.scale;

                    if (!setting.useReplaceMaterial) continue;

                    if (setting.targetObjectName == "")
                    {
                        Debug.Log("'Target Object Name' is not set.");
                        continue;
                    }

                    var targetObject = obj.transform.Find(setting.targetObjectName);
                    if (targetObject == null)
                    {
                        Debug.Log($"'{setting.targetObjectName}' is not Find.");
                        continue;
                    }

                    var renderer = targetObject.GetComponent<Renderer>();
                    if (renderer == null)
                    {
                        Debug.Log($"'Renderer Component' is not Find in '{setting.targetObjectName}'.");
                        continue;
                    }

                    if (setting.texture == null)
                    {
                        Debug.Log($"'Texture' is not Find.");
                        continue;
                    }

                    var component = targetObject.gameObject.AddComponent<SpawnMaterialEditor>();
                    component.texture = setting.texture;
                    component.materialIndex = setting.materialIndex;
                }
            };
        }
    }
}