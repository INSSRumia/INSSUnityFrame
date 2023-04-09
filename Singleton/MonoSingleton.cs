using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INSSUnityFrame
{
    /// <summary>
    /// 继承了Mono的单例模式基类，继承了该脚本后不需要将手动挂载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance 
        { 
            get
            {
                if (instance is null)
                {
                    //在场景中创建一个物体，名字为脚本名
                    GameObject obj = new GameObject(typeof(T).ToString());
                    obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }
                return instance; 
            }
        }

    }

}

