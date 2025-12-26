using Unity.Entities;

/// <summary>
/// 弾の残り貫通数を表すコンポーネント。
/// 生成時に Penetration>0 の場合に付与される。
/// </summary>
public struct BulletPenetration : IComponentData
{
    public int Remaining;
}
