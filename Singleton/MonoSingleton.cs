using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INSSUnityFrame
{
    /// <summary>
    /// �̳���Mono�ĵ���ģʽ���࣬�̳��˸ýű�����Ҫ���ֶ�����
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
                    //�ڳ����д���һ�����壬����Ϊ�ű���
                    GameObject obj = new GameObject(typeof(T).ToString());
                    obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }
                return instance; 
            }
        }

    }

}

