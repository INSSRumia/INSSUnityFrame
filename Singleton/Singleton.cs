using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace INSSUnityFrame
{
    /// <summary>
    /// 单例模式基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : new()
    {
        static T instance;
        public static T Instance 
        { 
            get 
            { 
                return instance ?? (instance = new T()); 
            } 
        }
    }
}

