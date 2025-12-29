using Unity.Entities;
using UnityEngine;

public class BulletEnemyPrefabBufferAuthring : MonoBehaviour
{
    [SerializeField]
    private BulletAuthoring[] bulletPrefabs;
    class Baker : Baker<BulletEnemyPrefabBufferAuthring>
    {
        public override void Bake(BulletEnemyPrefabBufferAuthring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            // バッファを追加
            var buffer = AddBuffer<BulletEnemyPrefabElement>(entity);

            // GameObject プレハブから Entity プレハブを取得して追加
            foreach (var prefab in authoring.bulletPrefabs)
            {
                // Entity プレハブを取得
                var prefabEntity = GetEntity(prefab, TransformUsageFlags.Dynamic);
                Debug.Log($"{prefab.name}を追加");
                
                // バッファに追加
                buffer.Add(new BulletEnemyPrefabElement
                {
                    Prefab = prefabEntity,
                    Id = prefab.EnemyId
                });
                Debug.Log(buffer);
            }
        }
    }
}
public struct BulletEnemyPrefabElement : IBufferElementData
{
    public Entity Prefab;
    public int Id;
}