using Unity.Entities;

/// <summary>
/// 弾の種類
/// </summary>
public enum BulletType
{
    Player,
    Enemy
}
/// <summary>
/// プレイヤーが発射した弾であることを示すタグ
/// </summary>
public struct PlayerBullet : IComponentData
{
}
/// <summary>
/// 敵が発射した弾であることを示すタグ
/// </summary>
public struct EnemyBullet : IComponentData
{
    public int Id;
}