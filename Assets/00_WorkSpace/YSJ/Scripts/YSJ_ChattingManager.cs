using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class YSJ_ChattingManager : YSJ_SimpleSingleton<YSJ_ChattingManager>, IManager
{
    [SerializeField] private bool _isChattingEnabled = true;

    [SerializeField] private float _chattingUpdateMinTime = 1.0f;
    [SerializeField] private float _chattingUpdateMaxTime = 2.0f;

    [SerializeField, Range(1, 30)] private int _maxChattingCount = 10;

    private Queue<string> _chattingMessages;

    public Action<string[]> OnChattingMessageAdded;

    public bool IsChattingEnabled => _isChattingEnabled;
    public bool IsDontDestroy => isDontDestroyOnLoad;
    public int MaxChattingCount => _maxChattingCount;

    #region IManager
    public void Initialize() => SetChattingEnabled();
    public void Cleanup() => ClearChattingMessages();

    public GameObject GetGameObject() => this.gameObject;

    #endregion

    private void SetChattingEnabled()
    {
        if (_isChattingEnabled)
        {
            _chattingMessages = new Queue<string>(_maxChattingCount);
            Debug.Log("Chatting enabled.");
        }
        else
        {
            ClearChattingMessages();
            Debug.Log("Chatting disabled.");
        }
    }
    private void ClearChattingMessages()
    {
        _chattingMessages?.Clear();
    }

    public void AddChattingMessage(string message)
    {
        if (_isChattingEnabled)
        {
            _chattingMessages?.Enqueue(message);
            if (_chattingMessages.Count == _maxChattingCount)
            {
                _chattingMessages?.Dequeue();
                _chattingMessages?.Enqueue(message);
            }

            Debug.Log($"Added chat message: {message}");
            OnChattingMessageAdded?.Invoke(GetChattingMessages());
        }
        else
        {
            Debug.LogWarning("Chatting is disabled or max count reached.");
        }
    }

    public string[] GetChattingMessages()
    {
        if (_isChattingEnabled && _chattingMessages != null)
        {
            var list = _chattingMessages.ToList();
            list.Reverse();
            string[] messages = list.ToArray();
            return messages;
        }
        else
        {
            Debug.LogWarning("Chatting is disabled, cannot retrieve messages.");
            return new string[0];
        }
    }
}
