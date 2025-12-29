public static class AbilityBridge
{
    public static AblityManager Manager;

    public static void OnHit(ref HitContext ctx)
    {
        Manager?.OnHit(ref ctx);
    }
}
