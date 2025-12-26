using Unity.Entities;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    public PlayerAttacker(EntityManager em)
    {
        _em = em;
    }
    public void OnAttack(int index, Vector3 pos,Vector3 forward)
    {
        _em.Shoot(index, pos, forward);
    }
    private EntityManager _em;
}
