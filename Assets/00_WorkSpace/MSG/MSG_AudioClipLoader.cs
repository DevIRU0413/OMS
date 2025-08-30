using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_AudioClipLoader : MonoBehaviour
    {
        private void Awake()
        {
            MSG_AudioDict.Init();
        }
    }
}
