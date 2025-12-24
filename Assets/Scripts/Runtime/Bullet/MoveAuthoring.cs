using UnityEngine;
using Unity.Entities;

public struct Move : IComponentData
{
    public Vector3 Direction;
    public float Speed;
}
public class MoveAuthoring : MonoBehaviour
{
    public Vector3 Direction;
    public float Speed;

    class moveBaker : Baker<MoveAuthoring>
    {
        public override void Bake(MoveAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Move
            {
                Direction = authoring.Direction,
                Speed = authoring.Speed
            });
        }
    }
}
