using UnityEngine;

public interface IActiveAbility
{
    bool CanActivate { get; }
    bool IsActive { get; }
    void Activate();
    void Tick(float dt);
    void Deactivate();
}
