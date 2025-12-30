using Unity.Entities;

/// <summary>
/// プレイヤー死亡時に全エネミーを破棄する System
/// </summary>
public partial struct EnemyCleanupOnPlayerDeadSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        // プレイヤー死亡イベントが存在しないなら何もしない
        if (!SystemAPI.TryGetSingleton<PlayerDeadEvent>(out _))
        {
            return;
        }

        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (enemy, entity) in
                 SystemAPI.Query<RefRO<EnemyEntity>>().WithEntityAccess())
        {
            ecb.AddComponent(entity, new DeadEvent());
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        // イベントは役目を終えたので削除
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerDeadEvent>();
        state.EntityManager.DestroyEntity(playerEntity);
    }
}
