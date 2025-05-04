using Unity.VisualScripting;
using UnityEngine;

public class GameSetupState : State
{
    private GameFSM _stateMachine;
    private GameController _controller;

    public GameSetupState(GameFSM stateMachine, GameController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered state: SETUP");

        int currentRound = _controller.PlayerData.CurrentRound;
        int boardLength = Mathf.Clamp(currentRound + 4, 5, 12);

        //boards all square for now, could add more variety later.
        //5 feels like the bare minimum to be playable
        //12 is kind of arbitrary but don't want to zoom out so much it's unreadable

        int spawnPointsNumber = currentRound / 2 + 1;
        int totalEnemies = currentRound * 10;

        Camera.main.transform.localPosition = new Vector3(0, boardLength, 0);
        _controller.BoardController.InitializeBoard(new Vector2Int(boardLength, boardLength), spawnPointsNumber, totalEnemies);
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
        _stateMachine.ChangeState(_stateMachine.PreparationState);
    }
}
