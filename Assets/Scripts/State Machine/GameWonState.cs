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
        //show win menu, save high score
        if(_controller.PlayerData.CurrentRound > SaveManager.Instance.ActiveSaveData.HighestLevel)
        {
            SaveManager.Instance.ActiveSaveData.HighestLevel = _controller.PlayerData.CurrentRound;
            SaveManager.Instance.Save();
        }
        _controller.HUDController.ShowWinMenu();
        _controller.PlayerData.OnNextRound += StartNextRound;

        Debug.Log("Entered state: WIN");
    }

    public override void Exit()
    {
        base.Exit();
        _controller.PlayerData.OnNextRound -= StartNextRound;
    }

    public override void FixedTick()
    {
        base.FixedTick();
    }

    public override void Tick()
    {
        base.Tick();
    }

    private void StartNextRound()
    {
        _stateMachine.ChangeState(_stateMachine.SetupState);
    }
}
