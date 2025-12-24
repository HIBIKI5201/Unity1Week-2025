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

    class moveBaker : Baker<MoveAuthoring>
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
        }
    }
}
