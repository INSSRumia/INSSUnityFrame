using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INSSUnityFrame
{
    public class AudioMgr : Singleton<AudioMgr>
    {
        enum AudioEventEnum
        {
            changeVolume
        }

        float mainVolume = 1;
        bool mute = false;
        float bkVolume = 1;
        bool bkMute = false;
        float aeVolume = 1;
        bool aeMute = false;    
    
        AudioSource bkMusicAS;
        List<AudioSource> aeList = new List<AudioSource>();

        public AudioMgr()
        {
            mainVolume = (float)PlayerPrefsMgr.Instance.LoadData(typeof(float), "mainVolume");
            mute = (bool)PlayerPrefsMgr.Instance.LoadData(typeof(bool), "mute");
            bkVolume = (float)PlayerPrefsMgr.Instance.LoadData(typeof(float), "bkVolume");
            bkMute = (bool)PlayerPrefsMgr.Instance.LoadData(typeof(bool), "bkMute");
            aeVolume = (float)PlayerPrefsMgr.Instance.LoadData(typeof(float), "aeVolume");
            aeMute = (bool)PlayerPrefsMgr.Instance.LoadData(typeof(bool), "aeMute");

            EventCenterMgr.Instance.AddEventListener<AudioEventEnum, (float, bool, float, bool, float, bool) >(AudioEventEnum.changeVolume, (v)=>
            {
                mainVolume = v.Item1;
                mute = v.Item2;
                bkVolume = v.Item3;
                bkMute = v.Item4;
                aeVolume = v.Item5;
                aeMute = v.Item6;

                PlayerPrefsMgr.Instance.SaveData(mainVolume, "mainVolme");
                PlayerPrefsMgr.Instance.SaveData(mute, "mute");
                PlayerPrefsMgr.Instance.SaveData(bkVolume, "bkVolume");
                PlayerPrefsMgr.Instance.SaveData(bkMute, "bkMute");
                PlayerPrefsMgr.Instance.SaveData(aeVolume, "aeVolume");
                PlayerPrefsMgr.Instance.SaveData(aeMute, "aeMute");

            });

            bkMusicAS = new GameObject().AddComponent<AudioSource>();
            bkMusicAS.spatialBlend = 0;
            bkMusicAS.gameObject.name = "BKMusicAS";
            MonoMgr.Instance.DontDestoryOnLoad(bkMusicAS.gameObject);
        }


        /// <summary>
        /// ���ű�������
        /// </summary>
        /// <param name="path">����·��</param>
        /// <param name="volume">����</param>
        public void PlayBKMusic(string path, float volume = 1)
        {
            ResMgr.Instance.ResourcesLoadAsync<AudioClip>(path, (AC) =>
            {
                bkMusicAS.volume = (mute || bkMute) ? 0 : volume * bkVolume * mainVolume;
                bkMusicAS.clip = AC;
            });
        }
        /// <summary>
        /// ��ͣ��������
        /// </summary>
        public void PauseBKMusic()
        {
            bkMusicAS.Pause();
        }
        /// <summary>
        /// ֹͣ��������
        /// </summary>
        public void StopBKMusic()
        {
            bkMusicAS.Stop();
        }

        /// <summary>
        /// ������Ч
        /// </summary>
        /// <param name="path">��Ч��Resources�е�·��</param>
        /// <param name="transform">��Ч��Ҫ������λ��</param>
        /// <param name="volume">����</param>
        /// <param name="isLoop">��Ч�Ƿ�ѭ��</param>
        public void PlayAE(string path, Transform transform, float volume = 1, bool isLoop = false)
        {
            GameObject aeObj = PoolMgr.Instance.GetObjByNew("ae_" + path);
            aeObj.transform.parent = transform;
            AudioSource ae;
            if (!aeObj.TryGetComponent<AudioSource>(out ae))
                ae = aeObj.AddComponent<AudioSource>();

            aeList.Add(ae);

            ae.volume = (mute || aeMute) ? 0 : volume * aeVolume * mainVolume;
            ae.loop = isLoop;
            ae.playOnAwake = false;

            ResMgr.Instance.ResourcesLoadAsync<AudioClip>(path, (ac) =>
            {
                ae.clip = ac;
                ae.Play();
                MonoMgr.Instance.AddUpdateListener(()=>
                {
                    if (isLoop)
                        return;
                    if (!ae.isPlaying)
                    {
                        aeList.Remove(ae);
                        //��Ч���������ѹ�ض����
                        PoolMgr.Instance.PushObj("ae_"+path, ae.gameObject);
                    } 
                });
            });
        
        }

        /// <summary>
        /// ֹͣ��Ч
        /// </summary>
        public void StopAE()
        {
            foreach (var ae in aeList)
            {
                ae.Stop();
            }
        }
    }

}

