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

        _controller.Input.TouchStarted += OnTouch;

        Debug.Log("Entered state: GAMEPLAY");
        //listen for inputs
        //start spawning enemies
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

        _controller.BoardController.BoardTick();
        //check for win - round timer <= 0

        //check for lose - health <= 0
    }

    private void OnTouch(Vector2 position)
    {
        _controller.BoardController.HandleTouch(position);
    }
}
