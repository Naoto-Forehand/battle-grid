using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHelper : MonoBehaviour
{
    [SerializeField]
    private BoardBuilder _builder;

    private void Awake()
    {
        if (_builder != null)
        {
            _builder.BuilderReady += this.TriggerBoardBuild;
        }
    }

    public void TriggerBoardBuild()
    {
        _builder.BuildBoard();
    }
}
