using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace INSSUnityFrame
{
    /// <summary>
    /// ����ص�����
    /// </summary>
    public class PoolData
    {
        /// <summary>
        /// ���ж����ڳ����еĸ�����
        /// </summary>
        public GameObject objParent;
        /// <summary>
        /// �洢���������
        /// </summary>
        public Stack<GameObject> poolStack;

        /// <summary>
        /// ��������ز��ڳ���������һ�����������
        /// �������صĶ��󽫳�Ϊ����ص�������
        /// </summary>
        /// <param name="name">����ص�����</param>
        /// <param name="pool">���ж���صĸ�����</param>
        public PoolData(string name, GameObject pool)
        {
            objParent = new GameObject(name);
            poolStack = new Stack<GameObject>();
            objParent.transform.SetParent(pool.transform);
        }
    }


    public class PoolMgr : Singleton<PoolMgr>
    {
        /// <summary>
        /// ����صĸ����壬���ж���ض������ڸ�������
        /// </summary>
        GameObject pool;

        public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

        /// <summary>
        /// �Ӷ������ȡ��������������޶������Resources�м�����Դ
        /// </summary>
        /// <param name="path">������Resources�е�·��</param>
        /// <returns></returns>
        public void GetObjFromResourcesAsync(string path, Action<GameObject> action)
        {
        
            //������ڶ�Ӧ�Ķ�����Ҷ�����ж�����������0
            if(poolDic.ContainsKey(path) && poolDic[path].poolStack.Count > 0)
            {
                //������ӳ���ȡ��
                GameObject obj = poolDic[path].poolStack.Pop();
                obj.transform.SetParent(null);
                //�������
                obj.SetActive(true); 
            }
            else
            {
                ResMgr.Instance.ResourcesLoadAsync<GameObject>(path, (obj) => 
                {
                    obj.name = path; 
                    action(obj);
                });
            }
        }

        /// <summary>
        /// �Ӷ������ȡ��������������޶�����newһ������
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public GameObject GetObjByNew(string name)
        {
            GameObject obj;
            //������ڶ�Ӧ�Ķ�����Ҷ�����ж�����������0
            if (poolDic.ContainsKey(name) && poolDic[name].poolStack.Count > 0)
            {
                //������ӳ���ȡ��
                obj = poolDic[name].poolStack.Pop();
                obj.transform.SetParent(null);
                //�������
                obj.SetActive(true);
            }
            else
            {
                obj = new GameObject();
            }
            return obj;
        }

        /// <summary>
        /// �������������
        /// </summary>
        /// <param name="path">������Resources�е�·��</param>
        /// <param name="obj">Ҫ����Ķ���</param>
        public void PushObj(string path, GameObject obj)
        {
            //�ж϶���صĸ������Ƿ����
            pool = pool ?? new GameObject("Pool");

            //�������ڶ�Ӧ�Ķ���أ����ȴ���һ�������
            if (!poolDic.ContainsKey(path))
                poolDic.Add(path, new PoolData(path, pool));

            poolDic[path].poolStack.Push(obj);
            obj.transform.SetParent(poolDic[path].objParent.transform);
            obj.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">������Resources�е�·��</param>
        /// <param name="obj">Ҫ����Ķ���</param>
        /// <param name="time">�ӳٵ�ʱ��</param>
        public void PushObj(string path, GameObject obj, float time)
        {
            MonoMgr.Instance.StartCoroutine(PushObjCoroutine(path, obj, time));
        }

        IEnumerator PushObjCoroutine(string path, GameObject obj, float time)
        {
            yield return new WaitForSeconds(time);
            PushObj(path, obj);
        }
        /// <summary>
        /// ��ն���أ��ڳ����л���ʹ��
        /// </summary>
        public void Clear()
        {
            poolDic.Clear();
            pool = null;
        }
    }

}

