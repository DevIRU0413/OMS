using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_PlayerLogic : MonoBehaviour
    {
        #region Fields, Properties, Actions
        [SerializeField] private MSG_PlayerData _playerData;
        [SerializeField] private Animator _animator;
        [SerializeField] private CapsuleCollider2D _capsuleCollider2D;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private LayerMask _disturbNPCLayer;
        [SerializeField] private LayerMask _heartLayer;
        [SerializeField] private LayerMask _breadBoxLayer;
        [SerializeField] private MSG_PlayerSettings _playerSettings;
        [SerializeField] private GameObject _breadBag;

        private bool _isWornBreadBag = false;
        private bool _isFever = false;
        private Coroutine _invincibleCO;
        private bool _isFallen = false;

        public MSG_PlayerData PlayerData => _playerData;
        public SpriteRenderer PlayerSpriteRenderer => _spriteRenderer;
        public bool IsFever => _isFever;
        public bool IsWornBreadBag => _isWornBreadBag;

        public event Action OnPlayerFeverStarted;
        public event Action OnPlayerFeverEnded;
        public event Action OnPlayerDied;
        #endregion

        private void Awake()
        {
            //_playerData.Init();
        }

        private void Start()
        {
            _playerData.Init();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _disturbNPCLayer) != 0)
            {
                MSG_NPCProvider.TryGetDisturb(collision, out MSG_DisturbNPC npc);
                TryFallDown(npc.NPCData.CharAttackDamage);
            }

            // TODO: 이것도 GetComponent 우회를 위해 Provider 처럼 쓰는 것 고려
            if (((1 << collision.gameObject.layer) & _heartLayer) != 0)
            {
                var collectible = collision.GetComponent<MSG_ICollectible>();
                if (collectible != null)
                {
                    collectible.Collect();
                }
            }

            if (((1 << collision.gameObject.layer) & _breadBoxLayer) != 0)
            {
                var collectible = collision.GetComponent<MSG_ICollectible>();
                if (collectible != null)
                {
                    collectible.Collect();
                }
            }
        }

        public void Heal(int amount)
        {
            _playerData.CurrentHP = Mathf.Min(_playerData.CurrentHP + amount, MSG_PlayerData.MaxHP);
            Debug.Log($"치료됨{_playerData.CurrentHP}");
        }

        public void TryWearBreadBag()
        {
            // 빵 봉투 쓰는 로직
            _isWornBreadBag = true;
            _breadBag.SetActive(true);
        }

        private void TryFallDown(float damage)
        {
            // 1. 빵 봉투 착용 중이라면
            if (_isWornBreadBag)
            {
                // 빵 봉투 소모
                _isWornBreadBag = false;
                _breadBag.SetActive(false);

                // 무적 처리
                if (_invincibleCO != null)
                {
                    StopCoroutine(_invincibleCO);
                }
                _invincibleCO = StartCoroutine(InvincibleRoutine());

                return;
            }

            // 2. 이미 넘어진 상태라면 무시
            if (_isFallen) return;

            // 3. 넘어짐 처리
            _isFallen = true;
            TakeDamage(damage);

            if (_invincibleCO != null)
            {
                StopCoroutine(_invincibleCO);
            }
            _invincibleCO = StartCoroutine(InvincibleRoutine());
        }

        private IEnumerator InvincibleRoutine()
        {
            Debug.Log("무적 시간 시작");

            _capsuleCollider2D.enabled = false;
            yield return new WaitForSeconds(_playerSettings.InvincibleTime);
            _capsuleCollider2D.enabled = true;
            _isFallen = false;

            Debug.Log("무적 시간 끝");
        }

        private void TakeDamage(float amount)
        {
            _playerData.CurrentHP -= (int)amount; // 소수점 버려짐, 의도하지 않은거라면 바꿔야 될 듯

            if (_playerData.CurrentHP <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // 죽음 처리
            OnPlayerDied?.Invoke();
            Debug.Log("죽음!");
        }
    }
}
