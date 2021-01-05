using UnityEngine;

namespace InstantSpawner
{
    [RequireComponent(typeof(Renderer))]
    public class SpawnMaterialEditor : MonoBehaviour
    {
        public Texture2D texture;
        public int materialIndex;

        private void Start()
        {
            var renderer = GetComponent<Renderer>();
            renderer.materials[materialIndex].SetTexture("_MainTex", texture);
        }

        private void OnDestroy()
        {
            // 複製されたマテリアルをすべて破棄する
            foreach (var material in GetComponent<Renderer>().materials)
            {
                Destroy(material);
            }
        }
    }
}