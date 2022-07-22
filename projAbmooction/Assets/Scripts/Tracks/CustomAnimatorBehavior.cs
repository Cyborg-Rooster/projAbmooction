using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class CustomAnimatorBehavior : PlayableBehaviour
{
    public string clip;
    bool started;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (!started)
        {
            Animator cowAnimator = playerData as Animator;
            cowAnimator.Play(clip);
            started = true;
        }
        //base.ProcessFrame(playable, info, playerData);
    }
}
