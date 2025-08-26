using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    public class MSG_CameraEdgePlacer : MonoBehaviour
    {
        [SerializeField] Transform _scoreTriggerBox;

        private void Start()
        {
            _scoreTriggerBox.gameObject.SetActive(false);
            Debug.Log("오브젝트 비활성화");
        }

        public void PlaceBox()
        {
            Vector2 rightEdge = Camera.main.ViewportToWorldPoint(new Vector2(1f, 0.5f));
            _scoreTriggerBox.transform.position = rightEdge;
            StartCoroutine(WaitForTurnRight());
        }

        // 오른쪽으로 다 돌지 않은 상태에서 TriggerBox에 먼저 OnTriggerEnter하는 경우가 있어 미리 집계되는 경우가 있어 기다림
        private IEnumerator WaitForTurnRight()
        {
            yield return new WaitForSeconds(0.5f);
            _scoreTriggerBox.gameObject.SetActive(true);
        }
    }
}
