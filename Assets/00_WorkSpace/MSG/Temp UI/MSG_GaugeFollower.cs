using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_GaugeFollower : MonoBehaviour
    {
        [SerializeField] private RectTransform _catchGaugeUI;
        [SerializeField] private RectTransform _competeGaugeUI;
        [SerializeField] private RectTransform _canvasRectTransform;
        [SerializeField] private MSG_UIInstaller _uiInstaller;

        private Transform _targetNPC;

        private void Start()
        {
            if (_uiInstaller.UIPresenter != null)
            {
                SetupUIPresenter();
            }
            else
            {
                _uiInstaller.OnPresenterReady += SetupUIPresenter;
            }
        }

        private void Update()
        {
            UpdateGaugePosition();
        }

        private void OnDestroy()
        {
            if (_uiInstaller != null)
            {
                _uiInstaller.OnPresenterReady -= SetupUIPresenter;
            }
            if (_uiInstaller.UIPresenter != null)
            {
                _uiInstaller.UIPresenter.OnTargetChanged -= ChangeTarget;
            }
        }

        private void SetupUIPresenter()
        {
            _uiInstaller.UIPresenter.OnTargetChanged += ChangeTarget;
            ChangeTarget();
        }

        private void ChangeTarget()
        {
            if (_uiInstaller.UIPresenter.CurrentTarget != null)
            {
                _targetNPC = _uiInstaller.UIPresenter.CurrentTarget.gameObject.transform;
            }
        }

        private void UpdateGaugePosition()
        {
            if (_uiInstaller.UIPresenter == null)
            {
                Debug.LogWarning("UIPresenter가 없습니다");
                return;
            }

            if (_targetNPC == null)
            {
                //Debug.LogWarning("타겟 NPC가 없습니다");
                return;
            }

            // NPC의 월드 위치 → 스크린 위치
            Vector3 screenPos = Camera.main.WorldToScreenPoint(_targetNPC.position + new Vector3(0, 1.5f, 0)); // 머리 위쪽으로 보정

            // Canvas의 RectTransform 좌표로 변환
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, screenPos, null, out Vector2 localPoint))
            {
                _competeGaugeUI.anchoredPosition = localPoint;
                _catchGaugeUI.anchoredPosition = localPoint;
            }
        }
    }
}
