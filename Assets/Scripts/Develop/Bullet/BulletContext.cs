using Unity.Mathematics;

public struct BulletContext
{
    public int Index;
    public float3 Position;
    public float3 Forward;

    //追加の弾丸パラメータ
    /// <summary>
    /// 弾の数
    /// </summary>
    public int Count;
    /// <summary>
    /// 貫通の数
    /// </summary>
    public int Penetration;
    public bool Homing;
    public bool Explosive;
    public float ExplosionRadius;
}
