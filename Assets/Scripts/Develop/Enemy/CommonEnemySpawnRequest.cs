using Unity.Entities;

/// <summary>
/// 共通HPを持つエネミーを3体スポーンする要求
/// </summary>
public struct CommonEnemySpawnRequest : IComponentData
{
    public int InitialHealth;
}