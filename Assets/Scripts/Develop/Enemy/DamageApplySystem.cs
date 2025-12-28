using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(PlayerBulletHitSystem))]
public partial struct DamageApplySystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
        var entityManager = state.EntityManager;
        foreach (var (damage, health, entity)
                 in SystemAPI
                     .Query<RefRO<DamageEvent>, RefRW<HealthRef>>()
                     .WithEntityAccess())
        {
            Entity healthEntityRef = health.ValueRO.HealthEntity;
           
            // 参照先の HealthEntity が存在しない場合は無視
            if (!entityManager.Exists(healthEntityRef))
            {
                continue;
            }

            HealthEntity healthEntity = entityManager.GetComponentData<HealthEntity>(healthEntityRef);
            int first = healthEntity.Value;
            healthEntity.Value -= damage.ValueRO.Value;
            entityManager.SetComponentData(healthEntityRef, healthEntity);
            if (healthEntity.Value <= 0)
            {
                ecb.AddComponent(entity, new DeadEvent());
            }

            Debug.Log($"{first} {healthEntity.Value} {damage.ValueRO.Value}");

            ecb.RemoveComponent<DamageEvent>(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}