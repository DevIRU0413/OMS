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
            _scoreTriggerBox.gameObject.SetActive(true);
        }
    }
}
