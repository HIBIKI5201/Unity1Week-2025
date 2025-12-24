using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class BulletAuthoring : MonoBehaviour
{
    public Vector3 Direction;
    public float Speed;
    public float Radius;
    public BulletType BulletType;

    class moveBaker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BulletEntity
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
