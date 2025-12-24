using Unity.Entities;
using UnityEngine;

public class BulletPrefabBufferAuthring : MonoBehaviour
{
    [SerializeField]  
    private BulletAuthoring[] bulletPrefabs;
    class Baker : Baker<BulletPrefabBufferAuthring>
    {
        public override void Bake(BulletPrefabBufferAuthring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            // バッファを追加
            var buffer = AddBuffer<BulletPrefabElement>(entity);

            // GameObject プレハブから Entity プレハブを取得して追加
            foreach (var prefab in authoring.bulletPrefabs)
            {
                Debug.Log($"{prefab.name}を追加");
                buffer.Add(new BulletPrefabElement
                {
                    Prefab = GetEntity(prefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}

public struct BulletPrefabElement : IBufferElementData
{
    public Entity Prefab;
}
