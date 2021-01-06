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
        public bool useReplaceMaterial;
        public SpawnMaterial[] materialParams = {new SpawnMaterial(),};
    }
}
