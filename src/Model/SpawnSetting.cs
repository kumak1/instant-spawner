using System;
using UnityEngine;

namespace InstantSpawner.Model
{
    [Serializable]
    public class SpawnSetting
    {
        public GameObject prefab;
        public Vector3 position;
        public Vector3 rotate;
        public Vector3 scale = Vector3.one;
        public bool useReplaceMaterial;
        public string targetObjectName;
        public int materialIndex;
        public Texture2D texture;
    }
}
