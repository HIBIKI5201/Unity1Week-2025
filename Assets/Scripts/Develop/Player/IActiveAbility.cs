using UnityEngine;

public interface IActiveAbility
{
    bool CanActivate { get; }
    bool IsActive { get; }
    void Activate(float time);
    void Tick(float dt);
}
