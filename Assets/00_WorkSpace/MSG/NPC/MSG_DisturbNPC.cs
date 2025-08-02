using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_DisturbNPC : MSG_NPCBase
    {
        private void Start()
        {
            MSG_NPCProvider.RegisterDisturb(this, _collider);
        }

        private void Update()
        {
            _moveController.Tick();
        }

        private void OnDestroy()
        {
            MSG_NPCProvider.UnregisterDisturb(this, _collider);
        }
    }
}
