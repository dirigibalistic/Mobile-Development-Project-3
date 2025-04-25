using UnityEngine;

public class GameLostState : State
{
    private GameFSM _stateMachine;
    private GameController _controller;

    public GameLostState(GameFSM stateMachine, GameController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
        //show lose menu, save high score
        if (_controller.BoardController.CurrentRound > SaveManager.Instance.ActiveSaveData.HighestLevel)
        {
            SaveManager.Instance.ActiveSaveData.HighestLevel = _controller.BoardController.CurrentRound;
            SaveManager.Instance.Save();
        }
        _controller.HUDController.ShowLoseMenu();
        Debug.Log("Entered state: LOSE");
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
