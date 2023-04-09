using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace INSSUnityFrame
{
    /// <summary>
    /// ����ģʽ����
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

