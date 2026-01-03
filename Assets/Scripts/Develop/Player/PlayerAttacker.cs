using SymphonyFrameWork.System;
using Unity.Entities;

public class PlayerAttacker
{
    public PlayerAttacker(EntityManager em, PlayerConfig config)
    {
        _em = em;
        _config = config;
        _audioManager = ServiceLocator.GetInstance<AudioManager>();
    }

    public void OnAttack(in BulletContext ctx)
    {
        //todo:後で銃のクールタイム追加
        _em.Shoot(ctx);
        _audioManager?.PlaySE("Shoot");
    }

    public void OnUpdate()
    {
        //todo:自動で撃つ弾を増やす
    }

    private EntityManager _em;
    private PlayerConfig _config;
    private AudioManager _audioManager;
}
