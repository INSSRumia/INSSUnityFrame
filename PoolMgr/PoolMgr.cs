using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace INSSUnityFrame
{
    /// <summary>
    /// 对象池的数据
    /// </summary>
    public class PoolData
    {
        /// <summary>
        /// 所有对象在场景中的父物体
        /// </summary>
        public GameObject objParent;
        /// <summary>
        /// 存储对象的容器
        /// </summary>
        public Stack<GameObject> poolStack;

        /// <summary>
        /// 创建对象池并在场景中生成一个对象池物体
        /// 存入对象池的对象将成为对象池的子物体
        /// </summary>
        /// <param name="name">对象池的名字</param>
        /// <param name="pool">所有对象池的根物体</param>
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
        /// 对象池的根物体，所有对象池都将存在该物体下
        /// </summary>
        GameObject pool;

        public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

        /// <summary>
        /// 从对象池中取出对象，若对象池无对象，则从Resources中加载资源
        /// </summary>
        /// <param name="path">对象在Resources中的路径</param>
        /// <returns></returns>
        public void GetObjFromResourcesAsync(string path, Action<GameObject> action)
        {
        
            //如果存在对应的对象池且对象池中对象数量大于0
            if(poolDic.ContainsKey(path) && poolDic[path].poolStack.Count > 0)
            {
                //将对象从池中取出
                GameObject obj = poolDic[path].poolStack.Pop();
                obj.transform.SetParent(null);
                //激活对象
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
        /// 从对象池中取出对象，若对象池无对象则new一个返回
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public GameObject GetObjByNew(string name)
        {
            GameObject obj;
            //如果存在对应的对象池且对象池中对象数量大于0
            if (poolDic.ContainsKey(name) && poolDic[name].poolStack.Count > 0)
            {
                //将对象从池中取出
                obj = poolDic[name].poolStack.Pop();
                obj.transform.SetParent(null);
                //激活对象
                obj.SetActive(true);
            }
            else
            {
                obj = new GameObject();
            }
            return obj;
        }

        /// <summary>
        /// 将对象存入对象池
        /// </summary>
        /// <param name="path">对象在Resources中的路径</param>
        /// <param name="obj">要存入的对象</param>
        public void PushObj(string path, GameObject obj)
        {
            //判断对象池的根物体是否存在
            pool = pool ?? new GameObject("Pool");

            //若不存在对应的对象池，则先创建一个对象池
            if (!poolDic.ContainsKey(path))
                poolDic.Add(path, new PoolData(path, pool));

            poolDic[path].poolStack.Push(obj);
            obj.transform.SetParent(poolDic[path].objParent.transform);
            obj.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">对象在Resources中的路径</param>
        /// <param name="obj">要存入的对象</param>
        /// <param name="time">延迟的时间</param>
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
        /// 清空对象池，在场景切换后使用
        /// </summary>
        public void Clear()
        {
            poolDic.Clear();
            pool = null;
        }
    }

}

