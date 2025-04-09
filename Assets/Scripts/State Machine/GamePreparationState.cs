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
        _controller.Input.TouchStarted += OnTouch;
        Debug.Log("Entered state: PREPARATION");
        //let player place turrets
        //end when they hit "ready" button, move into play state
    }

    public override void Exit()
    {
        base.Exit();
        _controller.Input.TouchStarted -= OnTouch;
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Tick()
    {
        base.Tick();
    }

    private void OnTouch(Vector2 position)
    {
        _controller.BoardController.HandleTouch(position);
    }
}
