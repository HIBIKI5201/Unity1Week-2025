using SymphonyFrameWork.System;
using Unity.Entities;
using UnityEditor.ShaderGraph.Internal;

public class PlayerAttacker
{
    public PlayerAttacker(EntityManager em, PlayerConfig config)
    {
        _em = em;
        _config = config;
        _audioManager = ServiceLocator.GetInstance<AudioManager>();
        _shotCoolTime = _config.ShotCoolTime;
        _shotUpdateCoolTime = _config.ShotUpdateCoolTime;
    }

    public void OnAttack(in BulletContext ctx,float time)
    {
        if (time - _shotTime >= _shotCoolTime)
        {
            _em.Shoot(ctx);
            _audioManager?.PlaySE("Shoot");
            _shotTime = time;
        }
    }

    public void OnUpdate()
    {
        //todo:自動で撃つ弾を増やす
    }

    private EntityManager _em;
    private PlayerConfig _config;
    private AudioManager _audioManager;
    private float _shotCoolTime;
    private float _shotUpdateCoolTime;
    private float _shotTime;
}
