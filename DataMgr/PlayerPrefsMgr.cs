using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace INSSUnityFrame
{
    public class PlayerPrefsMgr : Singleton<PlayerPrefsMgr>
    {
        //#region ����ģʽ
        //static private PlayerPrefsMgr instance = new PlayerPrefsMgr();
        //static public PlayerPrefsMgr Instance 
        //{
        //    get { return instance; }
        //}
        //private PlayerPrefsMgr() { }
        //#endregion

        #region �洢����
        //�洢����
        //���Զ������ͣ�keyName_�������ݵ�����
        //�Զ������ͣ�keyName_�Զ�������_�ֶ���_�ֶ�����
        /// <summary>
        /// ���ݴ洢��Ŀǰ��֧��Int32��float��string��bool��List<>��Dictionary<,>�Լ����������������͵���ͽṹ��
        /// </summary>
        /// <param name="data">��Ҫ���������</param>
        /// <param name="keyName">�����ݵļ�</param>
        public void SaveData(object data,string keyName)
            {
                string saveKeyName;
                Type dataType = data.GetType();
                saveKeyName = keyName + "_" + dataType.Name;
                if (data.GetType() == typeof(int) || data.GetType().IsEnum || data.GetType() == typeof(uint) ||
                    data.GetType() == typeof(long) || data.GetType() == typeof(ulong) ||
                    data.GetType() == typeof(short) || data.GetType() == typeof(ushort) ||
                    data.GetType() == typeof(sbyte) || data.GetType() == typeof(byte) || data.GetType() == typeof(char))
                {
                    PlayerPrefs.SetInt(saveKeyName, (int)data);
                }
                else if(data.GetType() == typeof(float) || data.GetType() == typeof(double) || data.GetType() == typeof(decimal))
                {
                    PlayerPrefs.SetFloat(saveKeyName, (float)data);
                }
                else if(data.GetType() == typeof(string))
                {
                    PlayerPrefs.SetString(saveKeyName, (string)data);
                }
                else if(data.GetType() == typeof(bool))
                {
                    PlayerPrefs.SetInt(saveKeyName, (bool)data ? 1 : 0);
                }
                else if(data.GetType().IsArray)
                {
                    IList array = (IList)data;
                    PlayerPrefs.SetInt(saveKeyName + "_ArrayLength", array.Count);
                    for (int i = 0; i < array.Count; i++)
                    {
                        SaveData(array[i],saveKeyName + "[" + i + "]");
                    }
                }
                //�ж�data�������Ƿ���һ�����й���ķ������͡�����ǣ����жϸ÷������͵Ķ����Ƿ���List<>
                else if (data.GetType().IsConstructedGenericType && data.GetType().GetGenericTypeDefinition() == typeof(List<>))
                {
                    //����data��һ��object���޷��õ�List�ĳ��������е����ݡ�
                    //��δ֪List�ķ��Ͳ������ͣ�����Ƚ���תΪIList�����Եõ����ݵĳ��Ⱥ�ʹ�õ�����������������
                    IList list = (IList)data;
                    PlayerPrefs.SetInt(saveKeyName+"_ListCount", list.Count);//�ȴ���list�ĳ���
                    for (int i = 0; i < list.Count; i++)
                    {
                        //ʹ�õݹ齫list�е�����һһ�洢
                        SaveData(list[i], saveKeyName + "[" + i+"]");
                    }
                }
                //ͬ��
                else if (data.GetType().IsConstructedGenericType && data.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    IDictionary dictionary = (IDictionary)data;
                    PlayerPrefs.SetInt(saveKeyName+"_DictionaryCount", dictionary.Count);
                    int index = 0;
                    foreach (var key in dictionary.Keys)
                    {
                        SaveData(key, saveKeyName + "_Key["+index+"]");
                        SaveData(dictionary[key], saveKeyName + "_Value[" + index+"]");
                        index++;
                    }
                }
                else
                {
                    //�洢�Զ�������
                    SaveCustomType(data, saveKeyName);
                }

            PlayerPrefs.Save();
            }
        /// <summary>
        /// ���ڴ洢�Զ�������
        /// </summary>
        /// <param name="data">��Ҫ�洢������</param>
        /// <param name="keyName">�����ݵļ�</param>
        private void SaveCustomType(object data , string keyName)
        {
            string saveKeyName;
            Type dataType = data.GetType();
            //��ȡ�Զ��������е�ȫ���ֶ�
            FieldInfo[] fieldInfos = dataType.GetFields();
            for(int i = 0; i < fieldInfos.Length; i++)
            {
                saveKeyName = keyName +"_" + fieldInfos[i].Name;
                //��ȡdata�ж�Ӧ�ֶε�ֵ���洢�����Ƶݹ�
                SaveData(fieldInfos[i].GetValue(data), saveKeyName);
            }

        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ���ݶ�ȡ
        /// </summary>
        /// <param name="dataType">��Ҫ��������ݵ�����</param>
        /// <param name="keyName">�洢����ʱʹ�õļ�</param>
        /// <returns>����һ��object����ʹ��ǿ��ת������ת��Ϊ��Ҫ������</returns>
        public object LoadData(Type dataType,string keyName)
        {
            object data;
            string loadKeyName;
            loadKeyName = keyName + "_" + dataType.Name;
            if (dataType == typeof(int) || dataType.IsEnum ||dataType == typeof(uint) ||
                dataType == typeof(long) || dataType == typeof(ulong) ||
                dataType == typeof(short) || dataType == typeof(ushort) ||
                dataType == typeof(sbyte) || dataType == typeof(byte) || dataType == typeof(char))
            {
                data = PlayerPrefs.GetInt(loadKeyName);
            }
            else if (dataType == typeof(float) || dataType == typeof(double) || dataType == typeof(decimal))
            {
                data = PlayerPrefs.GetFloat(loadKeyName);
            }
            else if (dataType == typeof(string))
            {
                data = PlayerPrefs.GetString(loadKeyName);
            }
            else if (dataType == typeof(bool))
            {
                data = PlayerPrefs.GetInt(loadKeyName)>0;
            }
            else if (dataType.IsArray)
            {
                int ArrayLength = PlayerPrefs.GetInt(loadKeyName + "_ArrayLength");
                Type elementType = dataType.GetElementType();
                IList array = Array.CreateInstance(elementType,ArrayLength);
                for (int i = 0; i < ArrayLength; i++)
                {
                    //ͨ���ݹ��ȡ���洢�����ݲ�����array��
                    array[i] = LoadData(elementType, loadKeyName + "[" + i + "]");
                }
                data = array;
            }
            //ͬ���ݴ洢
            else if (dataType.IsConstructedGenericType && dataType.GetGenericTypeDefinition() == typeof(List<>))
            {
                int listLength = PlayerPrefs.GetInt(loadKeyName+"_ListCount");
                //ͨ������ʵ������һ�������List<>�������޷�������Ӧ�ķ���List����
                //��ʹ��object�޷�������ݳ��ȣ��޷�ʹ�õ����������ʹ��IList�洢
                IList list = Activator.CreateInstance(dataType) as IList;
                //��ȡ���Ͳ���
                Type[] genericArg = dataType.GetGenericArguments();
                for (int i = 0; i < listLength; i++)
                {
                    //ͨ���ݹ��ȡ���洢�����ݲ�����List��
                    list.Add(LoadData(genericArg[0], loadKeyName + "[" + i + "]"));
                }
                data = list;
            }
            else if (dataType.IsConstructedGenericType && dataType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                int dicLength = PlayerPrefs.GetInt(loadKeyName + "_DictionaryCount");
                IDictionary dictionary = Activator.CreateInstance(dataType) as IDictionary;
                Type[] genericArg = dataType.GetGenericArguments();
                for (int i = 0; i < dicLength; i++)
                {
                    object key = LoadData(genericArg[0], loadKeyName + "_Key[" + i + "]");
                    object value = LoadData(genericArg[1], loadKeyName + "_Value_[" + i + "]");
                    dictionary.Add(key,value);
                }
                data = dictionary;
            }
            else
            {
                data = LoadCustomType(dataType, loadKeyName);
            } 
            return data;
        }

        /// <summary>
        /// �洢�Զ�������
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private object LoadCustomType(Type dataType, string keyName)
        {
            string loadKeyName;
            FieldInfo[] fieldInfos = dataType.GetFields();
            object data = Activator.CreateInstance(dataType);
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                loadKeyName = keyName + "_" + fieldInfos[i].Name;
                fieldInfos[i].SetValue(data, LoadData(fieldInfos[i].FieldType, loadKeyName));
            }
            return data;
        }
        #endregion
    }

}
