using Unity.Entities;

/// <summary>
/// 使用するHP Entityへの参照
/// </summary>
public struct HealthReference : IComponentData
{
    public Entity Target;
}