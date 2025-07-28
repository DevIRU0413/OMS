using UnityEngine;

namespace Core.UnityUtil
{
    public abstract class SimpleSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        [SerializeField, HideInInspector] private bool m_isDontDestroyOnLoad = true;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<T>();
                    if (m_instance == null)
                    {
#if UNITY_EDITOR
                        Debug.LogWarning($"[SimpleSingleton] {typeof(T)} 인스턴스가 없어 에디터에서 자동 생성됨.");
#endif
                        GameObject go = new GameObject($"@{typeof(T)}");
                        m_instance = go.AddComponent<T>();

                        if ((m_instance as SimpleSingleton<T>).IsDontDestroyOnLoad)
                            DontDestroyOnLoad(go);
                    }
                }
                return m_instance;
            }
        }

        protected virtual bool IsDontDestroyOnLoad => m_isDontDestroyOnLoad;

        protected virtual void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this as T;
                if (IsDontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
            }
            else if (m_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (m_instance == this)
            {
                m_instance = null;
            }
        }
    }
}
