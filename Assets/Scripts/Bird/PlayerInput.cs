using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private const string Jump = nameof(Jump);
    private const KeyCode AttackKey = KeyCode.F;

    private bool _jumpRequested;

    public event Action Jumped;
    public event Action Attacked;

    private void Update()
    {
        _jumpRequested = Input.GetButtonDown(Jump);

        if (_jumpRequested)
        {
            Jumped?.Invoke();
        }

        if (Input.GetKeyDown(AttackKey))
        {
            Attacked?.Invoke();
        }
    }
}