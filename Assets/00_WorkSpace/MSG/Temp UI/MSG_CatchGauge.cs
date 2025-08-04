using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MSG
{
    public class MSG_CatchGauge : MonoBehaviour
    {
        [SerializeField] private Image _gaugeImage;


        public void SetGauge(float value)
        {
            if (_gaugeImage == null)
            {
                Debug.LogError("이미지가 등록되지 않았습니다.");
                return;
            }

            value = Mathf.Clamp01(value);
            _gaugeImage.fillAmount = value;
        }
    }
}
