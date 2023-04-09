using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    /// <summary>
    /// 淡入淡出的速度
    /// </summary>
    private float alphaSpeed = 10;
    /// <summary>
    /// 是否显示的标识
    /// </summary>
    private bool isShow;
    /// <summary>
    /// 当隐藏完成时调用的委托
    /// </summary>
    private UnityAction hideCallBack;

    /// <summary>
    /// 外部调用ShowMe显示面板
    /// </summary>
    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// 外部调用HideMe删除面板
    /// </summary>
    /// <param name="callBack"></param>
    public void HideMe(UnityAction callBack)
    {
        isShow = false;
        canvasGroup.alpha = 1;
        hideCallBack += callBack;
    }

    /// <summary>
    /// 用于初始化委托等
    /// </summary>
    protected abstract void Init();

    protected virtual void Awake()
    {
        //获取CanvasGroup组件，获取不到则添加该组件
        if(!TryGetComponent<CanvasGroup>(out canvasGroup))
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }
    protected virtual void Start()
    {
        Init();
    }
    protected virtual void Update()
    {
        //淡入
        if(isShow && canvasGroup.alpha <1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha > 1)
                canvasGroup.alpha = 1;
        }
        //淡出
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
