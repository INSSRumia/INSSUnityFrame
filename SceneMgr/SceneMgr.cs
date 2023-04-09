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
        /// ͬ�����س���
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="action">������ɺ󴥷����¼�</param>
        public void LoadScene(string name, Action action)
        {
            SceneManager.LoadScene(name);
            action();
        }
        /// <summary>
        /// �첽���س���
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="action">������ɺ󴥷����¼�</param>
        public void LoadSceneAsync(string name, Action action)
        {
            MonoMgr.Instance.StartCoroutine(LoadSceneCoroutine(name,action));
        }
    
        private IEnumerator LoadSceneCoroutine(string name, Action action)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(name);
            while(!ao.isDone)
            {
                //���������������¼�
                EventCenterMgr.Instance.EventTrigger<SceneEventEnum, float>(SceneEventEnum.loadSceneProgress, ao.progress);
                yield return ao.progress;
            }
            action();
        }
    }

}

