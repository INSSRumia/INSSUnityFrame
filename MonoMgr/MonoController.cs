using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace INSSUnityFrame
{
    public class MonoController : MonoBehaviour
    {
        private event Action UpdateAction;

        public void AddUpdateListener(Action action)
        {
            UpdateAction += action;
        }

        public void RemoveUpdateListener(Action action)
        {
            UpdateAction -= action;
        }

        public void DontDestoryOnLoad(GameObject obj)
        {
            DontDestoryOnLoad(obj);
        }

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {
            if(UpdateAction != null)
                UpdateAction.Invoke();
        }
    }

}

