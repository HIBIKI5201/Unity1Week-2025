using Unity.Entities;
using UnityEngine;

public class PlayerAttacker
{
    public PlayerAttacker(EntityManager em, PlayerConfig config)
    {
        _em = em;
        _config = config;
    }

    public void OnAttack(in BulletContext ctx)
    {
        //todo:後で銃のクールタイム追加
        _em.Shoot(ctx);
    }

    public void OnUpdate()
    {
        //todo:自動で撃つ弾を増やす
    }

    private EntityManager _em;
    private PlayerConfig _config;
}
