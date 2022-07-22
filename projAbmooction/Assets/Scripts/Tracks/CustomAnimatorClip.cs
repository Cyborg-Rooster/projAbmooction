using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class CustomAnimatorClip : PlayableAsset
{
    public string clip;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CustomAnimatorBehavior>.Create(graph);

        CustomAnimatorBehavior customAnimatorBehavior = playable.GetBehaviour();

        customAnimatorBehavior.clip = clip;

        return playable;
    }
}
