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

            AbilityBridge.OnHit(ref ctx);

            ecb.RemoveComponent<HitEvent>(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}
