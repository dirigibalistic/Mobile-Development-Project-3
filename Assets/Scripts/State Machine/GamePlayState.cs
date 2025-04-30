using System;
using UnityEngine;

public class GamePlayState : State
{
    private GameFSM _stateMachine;
    private GameController _controller;

    public GamePlayState(GameFSM stateMachine, GameController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
        _controller.HUDController.ShowGameHUD();
        _controller.Input.TouchStarted += BoardTouch;
        _controller.BoardController.OnRoundWon += WinRound;
        _controller.PlayerData.OnPlayerDeath += LoseRound;
        _controller.AudioPlayer.PlayCombatMusic();

        Debug.Log("Entered state: GAMEPLAY");
    }

    public override void Exit()
    {
        base.Exit();
        _controller.Input.TouchStarted -= BoardTouch;
        _controller.BoardController.OnRoundWon -= WinRound;
        _controller.PlayerData.OnPlayerDeath -= LoseRound;
        _controller.AudioPlayer.StopMusic();
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Tick()
    {
        base.Tick();
        _controller.BoardController.BoardTick();
    }

    private void BoardTouch(Vector2 position)
    {
        _controller.BoardController.HandleTouch(position);
    }

    private void WinRound()
    {
        _stateMachine.ChangeState(_stateMachine.WonState);
    }

    private void LoseRound()
    {
        _stateMachine.ChangeState(_stateMachine.LostState);
    }
}
