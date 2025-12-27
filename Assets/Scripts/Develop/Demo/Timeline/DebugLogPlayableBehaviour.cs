using UnityEngine;
using UnityEngine.Playables;

public class DebugLogPlayableBehaviour : PlayableBehaviour
{
    public string logMessage;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Debug.Log(logMessage);
    }
}
