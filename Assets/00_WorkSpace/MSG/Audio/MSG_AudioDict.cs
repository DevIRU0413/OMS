using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public static class MSG_AudioDict
    {
        private static Dictionary<string, AudioClip> _clips = new();

        public static void Init()
        {
            var bgms = Resources.LoadAll<AudioClip>("Audio/BGM");
            foreach (var clip in bgms)
            {
                Debug.Log(clip.name);
                _clips.TryAdd(clip.name, clip);
            }

            var sfxs = Resources.LoadAll<AudioClip>("Audio/SFX");
            foreach (var clip in sfxs)
            {
                Debug.Log(clip.name);
                _clips.TryAdd(clip.name, clip);
            }
        }

        public static AudioClip Get(string key)
        {
            if (_clips == null) Init();
            return _clips.TryGetValue(key, out var clip) ? clip : null;
        }
    }
}
