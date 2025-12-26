using UnityEngine;

public interface IPassiveAbility
{
    void OnShoot(ref BulletContext context);
    void OnHit(ref HitContext context);
}

