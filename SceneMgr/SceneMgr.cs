using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace INSSUnityFrame
{
    public class SceneMgr : Singleton<SceneMgr>
    {
        enum SceneEventEnum
        {
            loadSceneProgress
        }

        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="name">场景名称</param>
        /// <param name="action">加载完成后触发的事件</param>
        public void LoadScene(string name, Action action)
        {
            SceneManager.LoadScene(name);
            action();
        }
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="name">场景名称</param>
        /// <param name="action">加载完成后触发的事件</param>
        public void LoadSceneAsync(string name, Action action)
        {
            MonoMgr.Instance.StartCoroutine(LoadSceneCoroutine(name,action));
        }
    
        private IEnumerator LoadSceneCoroutine(string name, Action action)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(name);
            while(!ao.isDone)
            {
                //触发进度条更新事件
                EventCenterMgr.Instance.EventTrigger<SceneEventEnum, float>(SceneEventEnum.loadSceneProgress, ao.progress);
                yield return ao.progress;
            }
            action();
        }
    }

}

