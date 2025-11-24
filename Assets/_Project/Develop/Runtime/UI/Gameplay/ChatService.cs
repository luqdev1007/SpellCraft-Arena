using System;
using System.Collections.Generic;

public class ChatService
{
    private List<string> _history = new();

    public IReadOnlyCollection<string> History => _history;

    public event Action<string> MessageAdded;

    public void AddMessage(string content, string userName)
    {
        if (content.Length == 0)
            return;

        _history.Add($"{userName}: {content}");

        MessageAdded?.Invoke(content);
    }
}