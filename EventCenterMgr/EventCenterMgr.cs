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
    /// �¼����Ĺ�����
    /// </summary>
    public class EventCenterMgr : Singleton<EventCenterMgr>
    {
        //public Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();
        public Dictionary<int, IEventInfo> eventDic = new Dictionary<int, IEventInfo>();

        /// <summary>
        /// ����в��¼�����
        /// </summary>
        /// <typeparam name="T">�¼�ί������</typeparam>
        /// <typeparam name="K">���ݵĲ�������</typeparam>
        /// <param name="eventEnum">�����ö��</param>
        /// <param name="action">ί��</param>
        public void AddEventListener<T, K>(T eventEnum, Action<K> action) where T : struct, Enum
        {
            int eventHashCode = (eventEnum.DisplayName() + eventEnum.DisplayName()).GetHashCode();
            if (!eventDic.ContainsKey(eventHashCode))
                eventDic.Add(eventHashCode, new EventInfo<K>(action));
            else
                (eventDic[eventHashCode] as EventInfo<K>).actions += action;
        }


        ///// <summary>
        ///// ����в��¼�����
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
        /// ����޲��¼�����
        /// </summary>
        /// <typeparam name="T">�¼�ö������</typeparam>
        /// <param name="eventEnum">�����ö��</param>
        /// <param name="action">�¼�</param>
        public void AddEventListener<T>(T eventEnum, Action action) where T : struct,Enum
        {
            int eventHashCode = (eventEnum.DisplayName() + eventEnum.DisplayName()).GetHashCode();
            if (!eventDic.ContainsKey(eventHashCode))
                eventDic.Add(eventHashCode, new EventInfo(action));
            else
                (eventDic[eventHashCode] as EventInfo).actions += action;
        }

        /// <summary>
        /// �Ƴ��в��¼�
        /// </summary>
        /// <typeparam name="T">�¼�ö������</typeparam>
        /// <typeparam name="K">���ݵĲ�������</typeparam>
        /// <param name="eventEnum">�����ö��</param>
        /// <param name="action">ί��</param>
        public void RemoveEventListener<T, K>(T eventEnum, Action<K> action) where T : struct, Enum
        {
            int eventHashCode = (eventEnum.DisplayName() + eventEnum.DisplayName()).GetHashCode();
            if (eventDic.ContainsKey(eventHashCode))
            {
                (eventDic[eventHashCode] as EventInfo<K>).actions -= action;
                //��ί����û�лص�����ʱ������ί�д��ֵ����Ƴ�
                if ((eventDic[eventHashCode] as EventInfo<K>).actions.GetInvocationList().Length == 0)
                    eventDic.Remove(eventHashCode);
            }
        }

        /// <summary>
        /// �Ƴ��޲��¼�
        /// </summary>
        /// <typeparam name="T">�¼�ö������</typeparam>
        /// <param name="eventEnum">�����ö��</param>
        /// <param name="action">�¼�</param>
        public void RemoveEventListener<T>(T eventEnum, Action action) where T : struct, Enum
        {
            int eventHashCode = (eventEnum.DisplayName() + eventEnum.DisplayName()).GetHashCode();
            if (eventDic.ContainsKey(eventHashCode))
            {
                (eventDic[eventHashCode] as EventInfo).actions -= action;
                //��ί����û�лص�����ʱ������ί�д��ֵ����Ƴ�
                if ((eventDic[eventHashCode] as EventInfo).actions.GetInvocationList().Length == 0)
                    eventDic.Remove(eventHashCode);
            }
        }

        /// <summary>
        /// �����в��¼�
        /// </summary>
        /// <typeparam name="T">�¼�ö������</typeparam>
        /// <typeparam name="K">���ݵĲ�������</typeparam>
        /// <param name="eventEnum">�����ö��</param>
        /// <param name="info">����</param>
        public void EventTrigger<T, K>(T eventEnum, K info) where T : struct, Enum
        {
            int eventHashCode = (eventEnum.DisplayName() + eventEnum.DisplayName()).GetHashCode();
            if (eventDic.ContainsKey(eventHashCode))
            {
                (eventDic[eventHashCode] as EventInfo<K>).actions(info);
            }
        }

        /// <summary>
        /// �����޲��¼�
        /// </summary>
        /// <typeparam name="T">�¼�ö������</typeparam>
        /// <param name="eventEnum">�����ö��</param>
        public void EventTrigger<T>(T eventEnum) where T : struct, Enum
        {
            int eventHashCode = (eventEnum.DisplayName() + eventEnum.DisplayName()).GetHashCode();
            if (eventDic.ContainsKey(eventHashCode))
            {
                (eventDic[eventHashCode] as EventInfo).actions();
            }
        }

        /// <summary>
        /// �Ƴ����е��¼�
        /// </summary>
        public void Clear()
        {
            eventDic.Clear(); 
        }
    }

}


