using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace INSSUnityFrame
{
    public class PlayerPrefsMgr : Singleton<PlayerPrefsMgr>
    {
        //#region 单例模式
        //static private PlayerPrefsMgr instance = new PlayerPrefsMgr();
        //static public PlayerPrefsMgr Instance 
        //{
        //    get { return instance; }
        //}
        //private PlayerPrefsMgr() { }
        //#endregion

        #region 存储数据
        //存储规则：
        //非自定义类型：keyName_传入数据的类型
        //自定义类型：keyName_自定义类型_字段名_字段类型
        /// <summary>
        /// 数据存储，目前仅支持Int32，float，string，bool，List<>，Dictionary<,>以及包含以上数据类型的类和结构体
        /// </summary>
        /// <param name="data">需要保存的数据</param>
        /// <param name="keyName">该数据的键</param>
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
                //判断data的类型是否是一个具有构造的泛型类型。如果是，则判断该泛型类型的定义是否是List<>
                else if (data.GetType().IsConstructedGenericType && data.GetType().GetGenericTypeDefinition() == typeof(List<>))
                {
                    //由于data是一个object，无法得到List的长度与其中的内容。
                    //且未知List的泛型参数类型，因此先将其转为IList，可以得到数据的长度和使用迭代器访问其中内容
                    IList list = (IList)data;
                    PlayerPrefs.SetInt(saveKeyName+"_ListCount", list.Count);//先储存list的长度
                    for (int i = 0; i < list.Count; i++)
                    {
                        //使用递归将list中的内容一一存储
                        SaveData(list[i], saveKeyName + "[" + i+"]");
                    }
                }
                //同上
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
                    //存储自定义类型
                    SaveCustomType(data, saveKeyName);
                }

            PlayerPrefs.Save();
            }
        /// <summary>
        /// 用于存储自定义类型
        /// </summary>
        /// <param name="data">需要存储的数据</param>
        /// <param name="keyName">该数据的键</param>
        private void SaveCustomType(object data , string keyName)
        {
            string saveKeyName;
            Type dataType = data.GetType();
            //获取自定义类型中的全部字段
            FieldInfo[] fieldInfos = dataType.GetFields();
            for(int i = 0; i < fieldInfos.Length; i++)
            {
                saveKeyName = keyName +"_" + fieldInfos[i].Name;
                //获取data中对应字段的值并存储，类似递归
                SaveData(fieldInfos[i].GetValue(data), saveKeyName);
            }

        }
        #endregion

        #region 读取数据
        /// <summary>
        /// 数据读取
        /// </summary>
        /// <param name="dataType">需要保存的数据的类型</param>
        /// <param name="keyName">存储数据时使用的键</param>
        /// <returns>返回一个object，请使用强制转换将其转换为需要的类型</returns>
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
                    //通过递归获取到存储的数据并加入array中
                    array[i] = LoadData(elementType, loadKeyName + "[" + i + "]");
                }
                data = array;
            }
            //同数据存储
            else if (dataType.IsConstructedGenericType && dataType.GetGenericTypeDefinition() == typeof(List<>))
            {
                int listLength = PlayerPrefs.GetInt(loadKeyName+"_ListCount");
                //通过反射实例化了一个传入的List<>，由于无法创建对应的泛型List变量
                //且使用object无法获得数据长度，无法使用迭代器。因此使用IList存储
                IList list = Activator.CreateInstance(dataType) as IList;
                //获取泛型参数
                Type[] genericArg = dataType.GetGenericArguments();
                for (int i = 0; i < listLength; i++)
                {
                    //通过递归获取到存储的数据并加入List中
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
        /// 存储自定义类型
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
