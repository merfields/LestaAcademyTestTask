using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Node Type", menuName ="New Node Type", order = 1)]
public class NodeSO : ScriptableObject
{
    public Sprite nodeSprite;
    public NodeType nodeType;

}

public enum NodeType{
    Yellow,
    Blue,
    Red,
    Blocked,
    Free
}
