using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct Bullet : IComponentData
{
    public float3 Direction;
    public float Speed;
    public float Radius;
}
public class MoveAuthoring : MonoBehaviour
{
    public Vector3 Direction;
    public float Speed;
    public float Radius;
    public BulletType BulletType;

    class MoveBaker : Baker<MoveAuthoring>
    {
        public override void Bake(MoveAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Bullet
            {
                Direction = authoring.Direction,
                Speed = authoring.Speed,
                Radius =  authoring.Radius,
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
