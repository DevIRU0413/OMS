using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


namespace MSG
{
    public class MSG_VolumeChanger : MonoBehaviour
    {
        [SerializeField] private Slider _volumeSlider;

        private void Awake()
        {
            if (_volumeSlider != null)
            {
                _volumeSlider = GetComponent<Slider>();
            }
        }

        private void OnEnable()
        {
            _volumeSlider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            if (_volumeSlider)
            {
                _volumeSlider.onValueChanged.RemoveListener(OnValueChanged);
            }
        }

        public void OnValueChanged(float value)
        {
            YSJ_AudioManager.Instance.SetBgmVolume(value);
            YSJ_AudioManager.Instance.SetSfxVolume(value);
        }
    }
}
