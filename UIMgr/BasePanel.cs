using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    /// <summary>
    /// ���뵭�����ٶ�
    /// </summary>
    private float alphaSpeed = 10;
    /// <summary>
    /// �Ƿ���ʾ�ı�ʶ
    /// </summary>
    private bool isShow;
    /// <summary>
    /// ���������ʱ���õ�ί��
    /// </summary>
    private UnityAction hideCallBack;

    /// <summary>
    /// �ⲿ����ShowMe��ʾ���
    /// </summary>
    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// �ⲿ����HideMeɾ�����
    /// </summary>
    /// <param name="callBack"></param>
    public void HideMe(UnityAction callBack)
    {
        isShow = false;
        canvasGroup.alpha = 1;
        hideCallBack += callBack;
    }

    /// <summary>
    /// ���ڳ�ʼ��ί�е�
    /// </summary>
    protected abstract void Init();

    protected virtual void Awake()
    {
        //��ȡCanvasGroup�������ȡ��������Ӹ����
        if(!TryGetComponent<CanvasGroup>(out canvasGroup))
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }
    protected virtual void Start()
    {
        Init();
    }
    protected virtual void Update()
    {
        //����
        if(isShow && canvasGroup.alpha <1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha > 1)
                canvasGroup.alpha = 1;
        }
        //����
        else if(!isShow)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                hideCallBack?.Invoke();
            }
        }
    }
}
