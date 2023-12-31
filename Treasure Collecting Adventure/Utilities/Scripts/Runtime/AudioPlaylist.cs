﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace LupinrangerPatranger
{
    [CreateAssetMenu(fileName = "Audio Playlist", menuName = "Treasure Collecting Adventure/Utilities/Audio Playlist")]
    [System.Serializable]
    public class AudioPlaylist : ScriptableObject
    {
        [SerializeField]
        protected List<AudioClip> m_Clips;

        public AudioClip this[int index]
        {
            get { return this.m_Clips[index]; }
        }

        public int Count
        {
            get { return this.m_Clips.Count; }
        }

    }
}