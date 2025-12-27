public class PenetrationAbility : IPassiveAbility
{
    private readonly int _penetrationCount;

    public PenetrationAbility(int penetrationCount)
    {
        _penetrationCount = penetrationCount;
    }

    public void OnShoot(ref BulletContext context)
    {
        // 発射時に貫通回数を設定する（指定値で上書き）。
        if (_penetrationCount > 0)
            context.Penetration = _penetrationCount;
    }

    public void OnHit(ref HitContext context)
    {
        // 貫通は Hit 側で処理されるためここでは特に処理しない
    }
}
