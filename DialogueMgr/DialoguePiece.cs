using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialougePiece
{

    /// <summary>
    /// 这句话说完之后是否要暂停
    /// </summary>
    public bool isPause;

    /// <summary>
    /// 这句话是否已经结束
    /// </summary>
    [HideInInspector] public bool isDone;

    /// <summary>
    /// 这句话的持续时间
    /// </summary>
    public float waitSecond = 0;

    /// <summary>
    /// 这句话结束后等待的时间
    /// </summary>
    public float waitSecondAfterThisPiece = 1f;

    /// <summary>
    /// 这句话的配音
    /// </summary>
    public AudioClip voice;

    /// <summary>
    /// 这句话的讲述者（当讲述者不是Player时）
    /// </summary>
    public string actor;

    /// <summary>
    /// 对话的内容
    /// </summary>
    public string text;

    /// <summary>
    /// 对话结束后触发的事件
    /// </summary>
    public UnityEvent afterPieceEvent;


}
