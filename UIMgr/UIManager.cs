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
        /// �洢panels���ֵ�
        /// </summary>
        public Dictionary<string, BasePanel> panelsDic = new Dictionary<string, BasePanel>();

        public UIManager()
        {
            GetCanvas();
        }

        private void GetCanvas()
        {
            //��ȡ�����е�Canvas
            canvasTrans = GameObject.Find("Canvas").transform;
            if (canvasTrans == null)
            {
                canvasTrans = new GameObject().AddComponent<RectTransform>();
                canvasTrans.gameObject.AddComponent<Canvas>();
                canvasTrans.gameObject.AddComponent<CanvasScaler>();
                canvasTrans.AddComponent<GraphicRaycaster>();
                canvasTrans.gameObject.name = "Canvas";
            }
            //�ڻ�����ʱ��ɾ��Canvas
            GameObject.DontDestroyOnLoad(canvasTrans.gameObject);
        }

        /// <summary>
        /// ��ʾ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ShowPanel<T>() where T:BasePanel
        {
            //Ԥ������������Ҫ���������ͬ
            string panelName = typeof(T).Name;
            //�жϳ������Ƿ��Ѿ��и�������
            if (!panelsDic.ContainsKey(panelName))
            {
                //����panel����������������ΪCanvas
                GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/Panel/" + panelName), canvasTrans, false);
                T panel = panelObj.GetComponent<T>();
                //��Panel�����ֵ�
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
        /// �������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isFade">�Ƿ���Ҫ���뵭��</param>
        public void HidePanel<T>( bool isFade ) where T:BasePanel
        {
            BasePanel panel;
            if (panelsDic.TryGetValue(typeof(T).Name,out panel))
            {
                if(isFade)
                {
                    //����ص���������������ɺ�ɾ�����
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
        /// ��ȡ���
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

