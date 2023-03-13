using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class SubtitleClip : PlayableAsset
{
    public string SubtitleText;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SubtitleBehavior>.Create(graph);
        SubtitleBehavior subtitleBehavior = playable.GetBehaviour();
        subtitleBehavior._Subtitle = SubtitleText;
        return playable;
    }
    

}
