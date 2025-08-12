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
        [SerializeField] private MSG_PlayerSettings _playerSettings;
        [SerializeField] private Animator _animator;
        [SerializeField] private CapsuleCollider2D _capsuleCollider2D;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private LayerMask _disturbNPCLayer;
        [SerializeField] private LayerMask _heartLayer;
        [SerializeField] private LayerMask _breadBoxLayer;
        [SerializeField] private GameObject _breadBag;

        private float _currentHPFloat;  // 경쟁상태에서는 Update에서 체력을 감산하여 정밀 계산용 float 필드
        private bool _isWornBreadBag = false;
        private Coroutine _invincibleCO;
        private bool _isFever = false;
        private float _feverGauge;      // 피버타임 게이지용 필드
        private Coroutine _feverCO;
        private bool _isFallen = false;
        private int _followerCount = 0; // UI에서 별도로 값을 가지고 있어서 안쓸 수도 있음
        private MSG_MapData _currentMap;

        public MSG_PlayerData PlayerData => _playerData;
        public MSG_PlayerSettings PlayerSettings => _playerSettings;
        public SpriteRenderer PlayerSpriteRenderer => _spriteRenderer;
        public bool IsWornBreadBag => _isWornBreadBag;
        public bool IsFever => _isFever;
        public bool IsFallen => _isFallen;
        public float FeverGauge => _feverGauge;
        public MSG_MapData CurrentMap => _currentMap;

        public event Action OnPlayerDamaged;
        public event Action OnPlayerFeverStarted;
        public event Action OnPlayerFeverEnded;
        public event Action OnPlayerDied;

        #endregion


        #region Unity Methods

        private void Awake()
        {
            //_playerData.Init();
        }

        private void Start()
        {
            _playerData.Init();
            _currentHPFloat = _playerData.CurrentHP;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _disturbNPCLayer) != 0)
            {
                MSG_NPCProvider.TryGetDisturb(collision, out MSG_DisturbNPC npc);
                TryFallDown(npc.NPCData.CharAttackDamage);
            }

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

        #endregion


        #region Public Methods

        public void Heal(int amount)
        {
            if (_isFever) return; // 피버타임이라면 힐 금지

            _currentHPFloat = Mathf.Min(_currentHPFloat + amount, MSG_PlayerData.MaxHP); // Float 계산용 내부 체력에도 가산
            _playerData.CurrentHP = Mathf.Min(_playerData.CurrentHP + amount, MSG_PlayerData.MaxHP);

            if (_playerData.CurrentHP == MSG_PlayerData.MaxHP)
            {
                if (_feverCO != null)
                {
                    StopCoroutine(_feverCO);
                    _feverCO = null;
                }
                _feverCO = StartCoroutine(FeverRoutine());
            }
        }

        public void TryWearBreadBag()
        {
            // 빵 봉투 쓰는 로직
            _isWornBreadBag = true;
            _breadBag.SetActive(true);
        }

        private void TryFallDown(float damage)
        {
            // if (_isFever) return; // 피버타임이라면 피격 금지 -> 아님 스턴은 되는데 데미지만 무효

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
            TakeDamageForFallen(damage);

            if (_invincibleCO != null)
            {
                StopCoroutine(_invincibleCO);
            }
            _invincibleCO = StartCoroutine(InvincibleRoutine());
        }

        /// <summary>
        /// 포획 중 체력 감소용
        /// </summary>
        public void TakeDamage(float amount)
        {
            if (_isFever) return; // 피버타임이라면 체력 감소 금지

            _currentHPFloat = Mathf.Clamp(_currentHPFloat - amount, MSG_PlayerData.MinHP, MSG_PlayerData.MaxHP);
            int next = Mathf.CeilToInt(_currentHPFloat); // 올림 계산, 0.1도 1로 보이긴 함

            if (next != _playerData.CurrentHP)
            {
                _playerData.CurrentHP = next;
            }

            if (next <= MSG_PlayerData.MinHP)
            {
                Die();
            }
        }

        /// <summary>
        /// 방해 NPC와 충돌 시 데미지 처리용
        /// </summary>
        public void TakeDamageForFallen(float amount)
        {
            if (_isFever) return; // 피버타임이라면 체력 감소 금지

            _currentHPFloat = Mathf.Clamp(_currentHPFloat - amount, MSG_PlayerData.MinHP, MSG_PlayerData.MaxHP);
            int next = Mathf.CeilToInt(_currentHPFloat); // 올림 계산, 0.1도 1로 보이긴 함

            if (next != _playerData.CurrentHP)
            {
                _playerData.CurrentHP = next;
            }

            OnPlayerDamaged?.Invoke(); // 충돌 시에만 호출

            if (next <= MSG_PlayerData.MinHP)
            {
                Die();
            }
        }

        public void ChangeCurrentMap(MSG_MapData currentMap)
        {
            _currentMap = currentMap;
        }

        public void AddFollower()
        {
            _followerCount++;
            YSJ_GameManager.Instance.AddFollower();
        }

        #endregion


        #region Private Methods

        // Restart가 있고 씬을 재시작해야되면 필요할 듯
        private void InitPlayer()
        {
            if (_feverCO != null)
            {
                StopCoroutine(_feverCO);
                _feverCO = null;
            }

            _isWornBreadBag = false;
            _isFever = false;
            _isFallen = false;
        }

        private IEnumerator InvincibleRoutine()
        {
            Debug.Log("무적 시간 시작");

            _capsuleCollider2D.enabled = false;

            float elapsed = 0;
            bool visible = true;

            while (elapsed < _playerSettings.InvincibleTime)
            {
                // 깜빡임 토글
                visible = !visible;
                Color c = _spriteRenderer.color;
                c.a = visible ? 1f : 0.5f;
                _spriteRenderer.color = c;

                // BlinkInterval만큼 기다리고 시간 증가
                yield return new WaitForSeconds(_playerSettings.BlinkInterval);
                elapsed += _playerSettings.BlinkInterval;
            }

            // 무적 종료 시 원래 상태로 복원
            _spriteRenderer.color = new Color(
                _spriteRenderer.color.r,
                _spriteRenderer.color.g,
                _spriteRenderer.color.b,
                1f
            );

            _capsuleCollider2D.enabled = true;
            _isFallen = false;

            Debug.Log("무적 시간 끝");
        }

        private void Die()
        {
            // 죽음 처리
            OnPlayerDied?.Invoke();
            Debug.Log("죽음!");
        }

        private IEnumerator FeverRoutine()
        {
            _isFever = true;
            StartFeverAnimation();
            float elapsed = 0;
            _feverGauge = 0;

            while (elapsed < _playerSettings.FeverTimeDuration)
            {
                elapsed += Time.deltaTime;
                _feverGauge = (elapsed / _playerSettings.FeverTimeDuration) * 100f; // _feverGauge가 0~100이 될 수 있도록 계산
                yield return null;
            }

            _isFever = false;
            EndFeverAnimation();
            _feverGauge = 100f;
            _playerData.CurrentHP = 60; // 피버타임 끝나고 체력 60
        }

        private void StartFeverAnimation()
        {
            Debug.Log("피버타임 애니메이션 시작");
        }

        private void EndFeverAnimation()
        {
            Debug.Log("피버타임 애니메이션 끝");
        }

        #endregion
    }
}
