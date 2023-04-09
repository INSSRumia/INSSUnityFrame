using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialougePiece
{

    /// <summary>
    /// ��仰˵��֮���Ƿ�Ҫ��ͣ
    /// </summary>
    public bool isPause;

    /// <summary>
    /// ��仰�Ƿ��Ѿ�����
    /// </summary>
    [HideInInspector] public bool isDone;

    /// <summary>
    /// ��仰�ĳ���ʱ��
    /// </summary>
    public float waitSecond = 0;

    /// <summary>
    /// ��仰������ȴ���ʱ��
    /// </summary>
    public float waitSecondAfterThisPiece = 1f;

    /// <summary>
    /// ��仰������
    /// </summary>
    public AudioClip voice;

    /// <summary>
    /// ��仰�Ľ����ߣ��������߲���Playerʱ��
    /// </summary>
    public string actor;

    /// <summary>
    /// �Ի�������
    /// </summary>
    public string text;

    /// <summary>
    /// �Ի������󴥷����¼�
    /// </summary>
    public UnityEvent afterPieceEvent;


}
