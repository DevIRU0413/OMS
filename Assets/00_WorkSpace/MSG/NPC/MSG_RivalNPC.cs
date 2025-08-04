using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_RivalNPC : MSG_NPCBase
    {
        private void Start()
        {
            MSG_NPCProvider.RegisterRival(this, _collider);
        }

        private void Update()
        {
            _moveController.Tick();
        }

        private void OnDestroy()
        {
            MSG_NPCProvider.UnregisterRival(this, _collider);
        }
    }
}
