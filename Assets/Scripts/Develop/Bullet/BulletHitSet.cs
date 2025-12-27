using Unity.Collections;
using Unity.Entities;

public struct BulletHitSet : IComponentData
{
    public NativeHashSet<Entity> Value;
}
