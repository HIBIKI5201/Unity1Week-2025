using UnityEngine;

public interface IActiveAblity
{
    bool CanActivate { get; }
    bool IsActive { get; }
    void Activate();
    void Tick(float dt);
    void Deactivate();
}
