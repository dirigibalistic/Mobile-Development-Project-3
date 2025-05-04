using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    private int _startingHealth;
    private int _health;
    public int Health => _health;

    [SerializeField]
    private int _startingMoney;
    private int _money;
    public int Money => _money;

    [SerializeField]
    private int _currentRound = 1;
    public int CurrentRound => _currentRound;

    private GameController _gameController;

    public event Action OnPlayerDeath;
    public event Action OnNextRound;

    private void Awake()
    {
        _gameController = GetComponentInParent<GameController>();
        _money = _startingMoney;
        _health = _startingHealth;
    }

    private void Start()
    {
        _gameController.HUDController.UpdateMoneyText(_money);
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        float healthPercent = (float)_health / (float)_startingHealth;
        _gameController.HUDController.UpdateHealthDisplay(healthPercent);
        if (_health <= 0) Die();
    }

    public bool SpendMoney(int amount)
    {
        if (_money < amount) return false;
        _money -= amount;
        _gameController.HUDController.UpdateMoneyText(_money);
        return true;
    }

    public void GainMoney(int amount)
    {
        _money += amount;
        _gameController.HUDController.UpdateMoneyText(_money);
    }

    private void Die()
    {
        OnPlayerDeath?.Invoke();
    }

    public void NextRound()
    {
        _currentRound++;
        _gameController.HUDController.UpdateRoundText(_currentRound);

        _health = _startingHealth;
        _money = _startingMoney;

        OnNextRound?.Invoke();
    }
}