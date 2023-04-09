using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INSSUnityFrame
{
    public class MonoMgr : Singleton<MonoMgr>
    {
        private MonoController controller;
        private GameObject controllerObj;

        public MonoMgr()
        {
            controllerObj = new GameObject("MonoController");
            controller = controllerObj.AddComponent<MonoController>();
        }


        public void AddUpdateListener(Action action)
        {
            controller.AddUpdateListener(action);
        }

        public void RemoveUpdateListener(Action action)
        {
            controller.RemoveUpdateListener(action);
        }

        public void DontDestoryOnLoad(GameObject obj)
        {
            controller.DontDestoryOnLoad(obj);
        }

        public Coroutine StartCoroutine(string methodName)
        {
            return controller.StartCoroutine(methodName);
        }

        public Coroutine StartCoroutine(string methodName, object value)
        {
            return controller.StartCoroutine(methodName);
        }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return controller.StartCoroutine(routine);
        }

        public Coroutine StartCoroutine_Auto(IEnumerator routine)
        {
            return controller.StartCoroutine(routine);
        }

        public void StopCoroutine(IEnumerator routine)
        {
            controller.StopCoroutine(routine);
        }

        public void StopCoroutine(Coroutine routine)
        {
            controller.StopCoroutine(routine);
        }

        public void StopCoroutine(string methodName)
        {
            controller.StopCoroutine(methodName);
        }

        public void StopAllCoroutines()
        {
            controller.StopAllCoroutines();
        }
    }

}


