using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    public class MSG_NPCRestrictorSpawner : MonoBehaviour
    {
        [SerializeField] private MSG_MapData[] _mapDatas;
        [SerializeField] private MSG_NPCRestrictor _restrictorPrefab;
        [SerializeField] private Transform _parent;

        private void Start()
        {
            DeployRestrictors();
        }


        private void DeployRestrictors()
        {
            foreach (var data in _mapDatas)
            {
                Vector2 leftSpawnPos = new(data.LeftNPCEndPoint, data.YPos);
                Vector2 rightSpawnPos = new(data.RightNPCEndPoint, data.YPos);

                MSG_NPCRestrictor left = Instantiate(_restrictorPrefab, leftSpawnPos, Quaternion.identity, _parent);
                left.SetDirection(Direction.LeftUp);

                MSG_NPCRestrictor right = Instantiate(_restrictorPrefab, rightSpawnPos, Quaternion.identity, _parent);
                right.SetDirection(Direction.RightUp);
            }
        }
    }
}
