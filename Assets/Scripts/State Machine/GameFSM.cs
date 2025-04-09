using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(GameController))]
public class GameFSM : StateMachineMB
{
    private GameController _controller;

    //state variables
    public GameSetupState SetupState {  get; private set; }
    public GamePlayState PlayState { get; private set; }
    public GameWonState WonState { get; private set; }
    public GameLostState LostState { get; private set; }

    private void Awake()
    {
        _controller = GetComponent<GameController>();
        //state instantiation
        SetupState = new GameSetupState(this, _controller);
        PlayState = new GamePlayState(this, _controller);
        WonState = new GameWonState(this, _controller);
        LostState = new GameLostState(this, _controller);
    }

    private void Start()
    {
        ChangeState(SetupState);
    }
}
