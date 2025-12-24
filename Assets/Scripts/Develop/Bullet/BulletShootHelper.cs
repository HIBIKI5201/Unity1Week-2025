using Unity.Entities;
using Unity.Mathematics;

public static class BulletShootHelper
{
    public static void Shoot(this EntityManager em, int index, float3 pos, float3 forward)
    {
        var request = em.CreateEntity();
        em.AddComponentData(request,
            new BulletSpawnRequest(index, pos, forward));
    }
}
