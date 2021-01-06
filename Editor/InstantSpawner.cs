using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace InstantSpawner
{
    using Model;
    using Util;

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
                var directoryPath = Path.Combine(new [] {"Assets", "TempMaterial"});
                AssetDatabase.DeleteAsset(directoryPath);
                AssetDatabase.CreateFolder("Assets", "TempMaterial");

                var parent = new GameObject(ParentName);
                var settingCount = 0;

                foreach (var setting in spawnParam.spawnSettings)
                {
                    settingCount++;
                    var materialCount = 0;
                    
                    if (setting.prefab == null) continue;

                    var obj = Instantiate(
                        setting.prefab,
                        setting.position,
                        Quaternion.Euler(setting.rotate),
                        parent.transform
                    );

                    if (!setting.useReplaceMaterial) continue;

                    foreach (var materialParam in setting.materialParams)
                    {
                        materialCount++;
                        var debugTargetName = $"\nIn 'SpawnSettings Index': {settingCount - 1}, 'MaterialParams Index': {materialCount - 1}'";

                        if (materialParam.targetObjectName == "")
                        {
                            Debug.Log("'Target Object Name' is not set." + debugTargetName);
                            continue;
                        }

                        var targetObject = obj.transform.Find(materialParam.targetObjectName);
                        if (targetObject == null)
                        {
                            Debug.Log($"'{materialParam.targetObjectName}' is not Find." + debugTargetName);
                            continue;
                        }

                        if (materialParam.texture == null)
                        {
                            Debug.Log("'Texture' is not Find." + debugTargetName);
                            continue;
                        }

                        var renderer = targetObject.GetComponent<Renderer>();
                        if (renderer == null)
                        {
                            Debug.Log($"'Renderer Component' is not Find." + debugTargetName);
                            continue;
                        }

                        var tempMaterial = new Material(renderer.sharedMaterial)
                        {
                            mainTexture = materialParam.texture
                        };
                        var path = Path.Combine(new[] {directoryPath, $"s{settingCount}m{materialCount}.mat"});
                        AssetDatabase.CreateAsset(tempMaterial, path);
                        AssetDatabase.SaveAssets();

                        renderer.sharedMaterial = tempMaterial;
                    }
                }
            };
        }
    }
}