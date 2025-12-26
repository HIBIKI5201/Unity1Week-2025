public static class AbilityBridge
{
    public static AbilityManager Manager;

    public static void OnHit(ref HitContext ctx)
    {
        Manager?.OnHit(ref ctx);
    }
}
