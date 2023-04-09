using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INSSUnityFrame
{
    public class ResMgr : Singleton<ResMgr>
    {

        /// <summary>
        /// Resources��Դͬ������
        /// </summary>
        /// <typeparam name="T">��Դ����</typeparam>
        /// <param name="path">��Դ��Resources�е�·��</param>
        public T ResourcesLoad<T>(string path) where T : UnityEngine.Object
        {
            T res = Resources.Load<T>(path);
            if (res is GameObject)
                //�����ص���ԴΪGameObjectʱ��ʵ�����󷵻�
                return GameObject.Instantiate(res);
            else//TextAsset AudioClip
                return res;
        }

        /// <summary>
        /// Resources��Դ�첽����
        /// </summary>
        /// <typeparam name="T">��Դ����</typeparam>
        /// <param name="path">��Դ��Resources�е�·��</param>
        /// <param name="action">��Դ���غ�ִ�е�ί��</param>
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

