using Unity.Entities;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    public PlayerAttacker(EntityManager em, PlayerConfig config)
    {
        _em = em;
        _config = config;
    }
    public void OnAttack(int index, Vector3 pos,Vector3 forward)
    {
        //todo:後で銃のクールタイム追加
        _em.Shoot(index, pos, forward);
    }

    public void OnUpdate()
    {
        //todo:自動で撃つ弾を増やす
    }

    private EntityManager _em;
    private PlayerConfig _config;
}
