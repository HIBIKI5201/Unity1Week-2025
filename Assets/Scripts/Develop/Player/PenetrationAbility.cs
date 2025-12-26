public class PenetrationAbility : IPassiveAbility
{
    public void OnShoot(ref BulletContext context)
    {
        context.Penetration += 1;
    }

    public void OnHit(ref HitContext context)
    {
        // 貫通なので特に何もしない
    }
}
