using UnityEngine;

namespace InstantSpawner.Model
{
    [CreateAssetMenu(
        fileName = "Spawn_", 
        menuName = "Tools/SpawnParam", 
        order    = 6)]
    public class SpawnParam : ScriptableObject
    {
        public SpawnSetting[] spawnSettings = {new SpawnSetting(),};
    }
}
