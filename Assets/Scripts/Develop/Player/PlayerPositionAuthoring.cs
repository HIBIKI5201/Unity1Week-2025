using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


/// <summary>
/// Player の位置を ECS に公開するための Authoring
/// </summary>
public sealed class PlayerPositionAuthoring : MonoBehaviour
{
    class Baker : Baker<PlayerPositionAuthoring>
    {
        public override void Bake(PlayerPositionAuthoring authoring)
        {
            // Player 用の Entity を生成する
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // 初期位置を ECS に登録する
            AddComponent(entity, new PlayerPosition
            {
                Position = authoring.transform.position
            });
            Debug.Log("PlayerPositionAuthoring.Bake called");
        }
    }
}
public struct PlayerPosition : IComponentData
{
    public float3 Position;
}