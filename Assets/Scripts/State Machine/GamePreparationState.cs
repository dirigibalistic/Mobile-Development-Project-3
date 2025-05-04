using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class GamePreparationState : State
{
    private GameFSM _stateMachine;
    private GameController _controller;

    public GamePreparationState(GameFSM stateMachine, GameController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
        _controller.Input.TouchTapPerformed += BoardTouch;
        _controller.HUDController.OnStartRoundPressed += StartRoundPressed;
        _controller.HUDController.ShowGameHUD();
        _controller.AudioPlayer.PlayPrepMusic();

        Debug.Log("Entered state: PREPARATION");
        //let player place turrets
        //end when they hit "ready" button, move into play state
    }

    public override void Exit()
    {
        base.Exit();
        _controller.Input.TouchTapPerformed -= BoardTouch;
        _controller.HUDController.OnStartRoundPressed -= StartRoundPressed;
        _controller.AudioPlayer.StopMusic();
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Tick()
    {
        base.Tick();
    }

    private void BoardTouch(Vector2 position)
    {
        _controller.BoardController.HandleTouch(position);
    }
    private void StartRoundPressed()
    {
        _stateMachine.ChangeState(_stateMachine.PlayState);
    }
}
