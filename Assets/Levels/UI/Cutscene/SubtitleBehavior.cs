using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;

public class SubtitleBehavior : PlayableBehaviour
{
    public string _Subtitle;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TMP_Text tmp = playerData as TMP_Text;
        tmp.text = _Subtitle;

    }

}
