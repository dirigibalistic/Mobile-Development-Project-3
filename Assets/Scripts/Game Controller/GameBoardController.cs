using UnityEngine;
using UnityEngine.UIElements;

public class GameBoardController : MonoBehaviour
{
    [SerializeField] private Vector2Int _boardSize = new Vector2Int(11, 11);
    [SerializeField] private GameBoard _board;

    [SerializeField] private GameTileContentFactory tileContentFactory;

    private void Awake()
    {
        _board.Initialize(_boardSize, tileContentFactory);
    }

    private void OnValidate()
    {
        if (_boardSize.x < 2) _boardSize.x = 2;
        if (_boardSize.y < 2) _boardSize.y = 2;
    }

    public void HandleTouch(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        GameTile tile = _board.GetTile(ray);
        if(tile != null) _board.ToggleWall(tile);
    }
}