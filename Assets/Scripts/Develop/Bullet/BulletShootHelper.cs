using Unity.Entities;
using Unity.Mathematics;

public static class BulletShootHelper
{
    public static void Shoot(this EntityManager em, int index, float3 pos, float3 forward)
    {
        var request = em.CreateEntity();
        em.AddComponentData(request,
            new BulletSpawnRequest(index, pos, forward, 0));
    }

    /// <summary>
    /// BulletContext に基づいて複数（ctx.Count）の弾発射リクエストを作成。
    /// </summary>
    /// <param name="em">EntityManager のインスタンス</param>
    /// <param name="ctx">弾発射に関するコンテキスト（インデックス、位置、方向、カウントなど）</param>
    public static void Shoot(this EntityManager em, in BulletContext ctx)
    {
        // ctx.Count 回分の発射リクエストを作成する。
        for (int i = 0; i < ctx.Count; i++)
        {
            // エンティティを作成して各リクエストを追加。
            var request = em.CreateEntity();
            em.AddComponentData(request,
                new BulletSpawnRequest(
                    ctx.Index,
                    ctx.Position,
                    ctx.Forward,
                    ctx.Penetration
                ));
        }
    }
}
