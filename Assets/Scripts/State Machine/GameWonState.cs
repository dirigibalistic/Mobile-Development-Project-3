using UnityEngine;

public class GameWonState : State
{
    private GameFSM _stateMachine;
    private GameController _controller;

    public GameWonState(GameFSM stateMachine, GameController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
        _controller.HUDController.ShowWinMenu();
        Debug.Log("Entered state: WIN");
        //show win menu
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Tick()
    {
        base.Tick();
    }
}
