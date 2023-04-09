using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace INSSUnityFrame
{
    public class UIManager : Singleton<UIManager>
    {
        private Transform canvasTrans;
        /// <summary>
        /// 存储panels的字典
        /// </summary>
        public Dictionary<string, BasePanel> panelsDic = new Dictionary<string, BasePanel>();

        public UIManager()
        {
            GetCanvas();
        }

        private void GetCanvas()
        {
            //获取场景中的Canvas
            canvasTrans = GameObject.Find("Canvas").transform;
            if (canvasTrans == null)
            {
                canvasTrans = new GameObject().AddComponent<RectTransform>();
                canvasTrans.gameObject.AddComponent<Canvas>();
                canvasTrans.gameObject.AddComponent<CanvasScaler>();
                canvasTrans.AddComponent<GraphicRaycaster>();
                canvasTrans.gameObject.name = "Canvas";
            }
            //在换场景时不删除Canvas
            GameObject.DontDestroyOnLoad(canvasTrans.gameObject);
        }

        /// <summary>
        /// 显示面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ShowPanel<T>() where T:BasePanel
        {
            //预制体面板的名字要与面板类相同
            string panelName = typeof(T).Name;
            //判断场景中是否已经有该面板存在
            if (!panelsDic.ContainsKey(panelName))
            {
                //生成panel，并将父物体设置为Canvas
                GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/Panel/" + panelName), canvasTrans, false);
                T panel = panelObj.GetComponent<T>();
                //将Panel存入字典
                panelsDic.Add(panelName, panel);
                panel.ShowMe();
                return panel;
            }
            else
            {
                return panelsDic[panelName] as T;
            }
        }

        /// <summary>
        /// 隐藏面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isFade">是否需要淡入淡出</param>
        public void HidePanel<T>( bool isFade ) where T:BasePanel
        {
            BasePanel panel;
            if (panelsDic.TryGetValue(typeof(T).Name,out panel))
            {
                if(isFade)
                {
                    //传入回调函数，当淡出完成后删除面板
                    panel.HideMe(() => GameObject.Destroy(panel.gameObject));
                    panelsDic.Remove(typeof(T).Name);
                }
                else
                {
                    GameObject.Destroy(panel.gameObject);
                    panelsDic.Remove(typeof(T).Name);
                }
            }
        
        }

        /// <summary>
        /// 获取面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetPanel<T>() where T:BasePanel
        {
            string panelName = typeof(T).Name;

            if (panelsDic.ContainsKey(panelName))
                return panelsDic[panelName] as T;

            return null; 
        }
    }
}

