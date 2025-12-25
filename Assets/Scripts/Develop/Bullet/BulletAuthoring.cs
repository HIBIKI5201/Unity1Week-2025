using Unity.Entities;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    public Vector3 Direction;
    public float Speed;
    public float Radius;
    public BulletType BulletType;

    class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BulletEntity
            {
                Radius = authoring.Radius,
            });
            AddComponent(entity, new MoveEntity()
            {
                Velocity = authoring.Direction * authoring.Speed
            });

            // 弾の種類に応じてタグを付与
            switch (authoring.BulletType)
            {
                case BulletType.Player:
                    AddComponent<PlayerBullet>(entity);
                    break;

                case BulletType.Enemy:
                    AddComponent<EnemyBullet>(entity);
                    break;
            }
        }
    }
}
