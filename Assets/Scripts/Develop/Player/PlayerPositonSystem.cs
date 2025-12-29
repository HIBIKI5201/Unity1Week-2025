using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// プレイヤーの LocalTransform を PlayerPosition に同期する System
/// </summary>
public partial struct PlayerPositionSyncSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        // PlayerPosition が存在する場合のみ System を実行する
        state.RequireForUpdate<PlayerPosition>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (playerPos, transform) in
                 SystemAPI.Query<RefRW<PlayerPosition>, RefRO<LocalTransform>>())
        {
            // プレイヤーの現在位置を ECS の PlayerPosition に反映する
            playerPos.ValueRW.Position = transform.ValueRO.Position;
        }
    }
}