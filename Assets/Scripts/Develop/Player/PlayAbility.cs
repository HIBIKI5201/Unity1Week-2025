using UnityEngine;

public class PlayAbility<T> : MonoBehaviour where T : IAbility, new()
{
    private T _ability;

    private void Awake()
    {
        _ability = new T();
        _ability.Apply(gameObject);
    }
}
