using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(PlayerBulletHitSystem))]
public partial struct DamageApplySystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (damage, health, entity)
            in SystemAPI
                .Query<RefRO<DamageEvent>, RefRW<HealthEntity>>()
                .WithEntityAccess())
        {
            int first = health.ValueRW.Value;

            health.ValueRW.Value -= damage.ValueRO.Value;

            if (health.ValueRO.Value <= 0)
            {
                ecb.AddComponent(entity, new DeadEvent());
            }

            Debug.Log($"{first} {health.ValueRW.Value} {damage.ValueRO.Value}");

            ecb.RemoveComponent<DamageEvent>(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}