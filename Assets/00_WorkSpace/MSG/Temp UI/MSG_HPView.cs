using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace MSG
{
    public class MSG_HPView : MonoBehaviour
    {
        [SerializeField] private Image _hpBar;
        [SerializeField] private TMP_Text _hpText;
        private int _maxHP;

        private void Start()
        {
            _maxHP = MSG_PlayerData.MaxHP;
        }

        public void SetHpUI(int value)
        {
            _hpBar.fillAmount = (float)value / _maxHP;
            _hpText.text = $"{value} / {_maxHP}";
        }
    }
}
