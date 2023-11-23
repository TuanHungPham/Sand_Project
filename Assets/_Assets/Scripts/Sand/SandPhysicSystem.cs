using System.Collections.Generic;
using UnityEngine;

public class SandPhysicSystem : TemporaryMonoSingleton<SandPhysicSystem>
{
    [SerializeField] private bool _shouldMoveHorizontally;

    [SerializeField] private bool _shouldMoveVertically;

    [SerializeField] private Timer _timer;

    private GameBoard _gameBoard;

    public GameBoard GameBoard => GameBoard.Instance;

    private int Row => GameBoard.OnBoardRow;
    private int Column => GameBoard.OnBoardColumn;

    private Timer Timer
    {
        get
        {
            if (_timer == null)
                _timer = new Timer();
            return _timer;
        }
    }

    private bool HasPastInterval => Timer.HasPastInterval();

    private void Update()
    {
        if (!HasPastInterval)
            return;
        if (_shouldMoveVertically)
        {
            MoveVertically();
            return;
        }

        if (_shouldMoveHorizontally)
            MoveHorizontally();
    }

    private void MoveHorizontally()
    {
        var matrix = new List<List<SandController>>();
        for (var i = 0; i < Row; i++)
        {
            var sandsAtRow = GameBoard.AtRow(i);
            foreach (var sand in sandsAtRow) sand.CheckMoveDown();
            foreach (var sand in sandsAtRow) sand.CheckMoveDiagonally();

            matrix.Add(sandsAtRow);
        }

        // foreach (var row in matrix)
        // foreach (var sand in row)
        //     sand.CheckMoveDiagonally();
    }

    private void MoveVertically()
    {
        var matrix = new List<List<SandController>>();
        for (var i = 0; i < Column; i++)
        {
            var sandsAtColumn = GameBoard.AtColumn(i);
            foreach (var sand in sandsAtColumn) sand.CheckMoveDown();
            foreach (var sand in sandsAtColumn) sand.CheckMoveDiagonally();

            matrix.Add(sandsAtColumn);
        }

        // foreach (var column in matrix)
        // foreach (var sand in column)
        //     sand.CheckMoveDiagonally();
    }
}