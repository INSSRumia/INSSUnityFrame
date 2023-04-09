using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INSSUnityFrame
{
    public class ResMgr : Singleton<ResMgr>
    {

        /// <summary>
        /// Resources资源同步加载
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源在Resources中的路径</param>
        public T ResourcesLoad<T>(string path) where T : UnityEngine.Object
        {
            T res = Resources.Load<T>(path);
            if (res is GameObject)
                //当加载的资源为GameObject时，实例化后返回
                return GameObject.Instantiate(res);
            else//TextAsset AudioClip
                return res;
        }

        /// <summary>
        /// Resources资源异步加载
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源在Resources中的路径</param>
        /// <param name="action">资源加载后执行的委托</param>
        /// <returns></returns>
        public void ResourcesLoadAsync<T>(string path, Action<T> action) where T : UnityEngine.Object
        {
            MonoMgr.Instance.StartCoroutine(ResourcesLoadCoroutine(path, action));
        }


        private IEnumerator ResourcesLoadCoroutine<T>(string path, Action<T> action) where T : UnityEngine.Object
        {
            ResourceRequest res = Resources.LoadAsync<T>(path);
            yield return res;

            if (res.asset is GameObject)
                action(GameObject.Instantiate(res.asset) as T);
            else
                action(res.asset as T);
        }
    }
}

