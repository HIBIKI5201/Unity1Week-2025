using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(DamageApplySystem))]
public partial struct AbilityHitDispatchSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (hit, entity)
            in SystemAPI.Query<RefRO<HitEvent>>().WithEntityAccess())
        {
            var ctx = new HitContext
            {
                Bullet = hit.ValueRO.Bullet,
                Target = hit.ValueRO.Target,
                Position = hit.ValueRO.Position,
                ECB = ecb
            };
            // パッシブに通知
            AbilityBridge.OnHit(ref ctx);

            // 貫通処理 / 弾の破壊判定
            var bullet = hit.ValueRO.Bullet;
            if (bullet != Entity.Null)
            {
                var em = state.EntityManager;
                if (em.HasComponent<BulletPenetration>(bullet))
                {
                    var pen = em.GetComponentData<BulletPenetration>(bullet);
                    pen.Remaining -= 1;
                    if (pen.Remaining <= 0)
                    {
                        ecb.DestroyEntity(bullet);
                    }
                    else
                    {
                        ecb.SetComponent(bullet, pen);
                    }
                }
                else
                {
                    // 貫通コンポーネントがない場合は通常弾として即破壊
                    ecb.DestroyEntity(bullet);
                }
            }

            ecb.RemoveComponent<HitEvent>(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}
