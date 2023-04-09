using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace INSSUnityFrame
{ 
    public interface IEventInfo { }

    public class EventInfo<T> : IEventInfo
    {
        public Action<T> actions;

        public EventInfo(Action<T> action)
        {
            actions +=action;
        }   
    }

    public class EventInfo : IEventInfo
    {
        public Action actions;

        public EventInfo(Action action)
        {
            actions += action;
        }
    }

    /// <summary>
    /// 事件中心管理器
    /// </summary>
    public class EventCenterMgr : Singleton<EventCenterMgr>
    {
        //public Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();
        public Dictionary<int, IEventInfo> eventDic = new Dictionary<int, IEventInfo>();

        /// <summary>
        /// 添加有参事件监听
        /// </summary>
        /// <typeparam name="T">事件委托类型</typeparam>
        /// <typeparam name="K">传递的参数类型</typeparam>
        /// <param name="eventEnum">具体的枚举</param>
        /// <param name="action">委托</param>
        public void AddEventListener<T, K>(T eventEnum, Action<K> action) where T : struct, Enum
        {
            int eventHashCode = (eventEnum.DisplayName() + eventEnum.DisplayName()).GetHashCode();
            if (!eventDic.ContainsKey(eventHashCode))
                eventDic.Add(eventHashCode, new EventInfo<K>(action));
            else
                (eventDic[eventHashCode] as EventInfo<K>).actions += action;
        }


        ///// <summary>
        ///// 添加有参事件监听
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="eventName"></param>
        ///// <param name="action"></param>
        //public void AddEventListener<T>(string eventName, Action<T> action)
        //{
        //    if(!eventDic.ContainsKey(eventName))
        //        eventDic.Add(eventName, new EventInfo<T>(action));
        //    else
        //        (eventDic[eventName] as EventInfo<T>).actions += action;
        //}

        /// <summary>
        /// 添加无参事件监听
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventEnum">具体的枚举</param>
        /// <param name="action">事件</param>
        public void AddEventListener<T>(T eventEnum, Action action) where T : struct,Enum
        {
            int eventHashCode = (eventEnum.DisplayName() + eventEnum.DisplayName()).GetHashCode();
            if (!eventDic.ContainsKey(eventHashCode))
                eventDic.Add(eventHashCode, new EventInfo(action));
            else
                (eventDic[eventHashCode] as EventInfo).actions += action;
        }

        /// <summary>
        /// 移除有参事件
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <typeparam name="K">传递的参数类型</typeparam>
        /// <param name="eventEnum">具体的枚举</param>
        /// <param name="action">委托</param>
        public void RemoveEventListener<T, K>(T eventEnum, Action<K> action) where T : struct, Enum
        {
            int eventHashCode = (eventEnum.DisplayName() + eventEnum.DisplayName()).GetHashCode();
            if (eventDic.ContainsKey(eventHashCode))
            {
                (eventDic[eventHashCode] as EventInfo<K>).actions -= action;
                //当委托中没有回调函数时，将该委托从字典中移除
                if ((eventDic[eventHashCode] as EventInfo<K>).actions.GetInvocationList().Length == 0)
                    eventDic.Remove(eventHashCode);
            }
        }

        /// <summary>
        /// 移除无参事件
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventEnum">具体的枚举</param>
        /// <param name="action">事件</param>
        public void RemoveEventListener<T>(T eventEnum, Action action) where T : struct, Enum
        {
            int eventHashCode = (eventEnum.DisplayName() + eventEnum.DisplayName()).GetHashCode();
            if (eventDic.ContainsKey(eventHashCode))
            {
                (eventDic[eventHashCode] as EventInfo).actions -= action;
                //当委托中没有回调函数时，将该委托从字典中移除
                if ((eventDic[eventHashCode] as EventInfo).actions.GetInvocationList().Length == 0)
                    eventDic.Remove(eventHashCode);
            }
        }

        /// <summary>
        /// 触发有参事件
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <typeparam name="K">传递的参数类型</typeparam>
        /// <param name="eventEnum">具体的枚举</param>
        /// <param name="info">参数</param>
        public void EventTrigger<T, K>(T eventEnum, K info) where T : struct, Enum
        {
            int eventHashCode = (eventEnum.DisplayName() + eventEnum.DisplayName()).GetHashCode();
            if (eventDic.ContainsKey(eventHashCode))
            {
                (eventDic[eventHashCode] as EventInfo<K>).actions(info);
            }
        }

        /// <summary>
        /// 触发无参事件
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventEnum">具体的枚举</param>
        public void EventTrigger<T>(T eventEnum) where T : struct, Enum
        {
            int eventHashCode = (eventEnum.DisplayName() + eventEnum.DisplayName()).GetHashCode();
            if (eventDic.ContainsKey(eventHashCode))
            {
                (eventDic[eventHashCode] as EventInfo).actions();
            }
        }

        /// <summary>
        /// 移除所有的事件
        /// </summary>
        public void Clear()
        {
            eventDic.Clear(); 
        }
    }

}


