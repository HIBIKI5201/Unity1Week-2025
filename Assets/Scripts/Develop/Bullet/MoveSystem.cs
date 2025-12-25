using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct MoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        MoveJob job = new MoveJob()
        {
            DeltaTime = SystemAPI.Time.DeltaTime
        };
        state.Dependency = job.ScheduleParallel(state.Dependency);
    }

    public partial struct MoveJob : IJobEntity
    {
        public float DeltaTime;

        public void Execute(in MoveEntity move, ref LocalTransform tr)
        {
            tr.Position = tr.Position + move.Velocity * DeltaTime;
        }
    }
}
