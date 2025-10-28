using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private Character _character;
    
    [SerializeField] private TMP_Text _scoreText;
    
    [SerializeField] private TMP_Text _healthText;
    
    private InputAction moveAction;
    private InputAction jumpAction;

    private int _score;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        
        _character.Died += OnDied;
    }

    private void OnDied()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void FixedUpdate()
    {
        _character.MoveInput = moveAction.ReadValue<Vector2>();
        _character.JumpInput = jumpAction.WasPressedThisFrame();
        
        UpdateHud();
    }
    
    private void UpdateHud()
    {
        _scoreText.text = $"score: {_score}";
        _healthText.text = $"health: {_character.Health}";
    }
    
    public void AddScore(int delta)
    {
        _score += delta;
        UpdateHud();
    }
    
    public void StompedEnemy()
    {
        AddScore(5);
        _character.Jump();
    }
}
