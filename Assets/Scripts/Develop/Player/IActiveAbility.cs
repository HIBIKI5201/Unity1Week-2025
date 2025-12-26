using UnityEngine;

public interface IActiveAbility
{
    bool CanActivate { get; }
    void Activate(float time);
    void Tick(float dt);
}
