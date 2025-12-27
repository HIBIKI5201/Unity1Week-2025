using UnityEngine;
using UnityEngine.Playables;

public class DebugLogPlayableAsset : PlayableAsset
{
    [SerializeField]
    private string _logMessage = "Debug Log from Playable Asset";

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        Playable playable = ScriptPlayable<DebugLogPlayableBehaviour>.Create(graph);
        DebugLogPlayableBehaviour behaviour = ((ScriptPlayable<DebugLogPlayableBehaviour>)playable).GetBehaviour();
        behaviour.logMessage = _logMessage;
        return playable;
    }
}
