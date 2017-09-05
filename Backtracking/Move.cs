using System;

namespace Backtracking
{
    sealed class Move
    {
        public readonly Tuple<int,int> Piece;
        public readonly Direction Direction;
        readonly Tuple<int, int> _obstacle;
        readonly Tuple<int, int> _target;
        readonly Board _board;
        bool _played = false;

        public bool IsValid
        {
            get
            {
                return _board.Tiles[Piece.Item1, Piece.Item2].IsPlayable &&
                    _board.Tiles[_obstacle.Item1, _obstacle.Item2].IsPlayable &&
                    _board.Tiles[_target.Item1, _target.Item2].IsPlayable &&
                    _board.Tiles[Piece.Item1, Piece.Item2].IsOccupied &&
                    _board.Tiles[_obstacle.Item1, _obstacle.Item2].IsOccupied &&
                    !_board.Tiles[_target.Item1, _target.Item2].IsOccupied;
            }
        }

        public Move(Board board, Tuple<int, int> piece, Direction direction)
        {
            _board = board;
            Piece = piece;
            Direction = direction;
            switch (Direction)
            {
                case Direction.Up:
                    _target = new Tuple<int, int>(Piece.Item1 - 2, Piece.Item2);
                    break;
                case Direction.Right:
                    _target = new Tuple<int, int>(Piece.Item1, Piece.Item2 + 2);
                    break;
                case Direction.Down:
                    _target = new Tuple<int, int>(Piece.Item1 + 2, Piece.Item2);
                    break;
                case Direction.Left:
                    _target = new Tuple<int, int>(Piece.Item1, Piece.Item2 - 2);
                    break;
                default:
                    throw new ArgumentException();
            }
            _obstacle = new Tuple<int, int>((Piece.Item1 + _target.Item1) / 2, (Piece.Item2 + _target.Item2) / 2);
        }

        public void Play()
        {
            if (IsValid)
            {
                _board.Tiles[Piece.Item1, Piece.Item2].RemovePiece();
                _board.Tiles[_obstacle.Item1, _obstacle.Item2].RemovePiece();
                _board.Tiles[_target.Item1, _target.Item2].AddPiece();
                _played = true;
            }
        }

        public void Undo()
        {
            if(_played)
            {
                _board.Tiles[Piece.Item1, Piece.Item2].AddPiece();
                _board.Tiles[_obstacle.Item1, _obstacle.Item2].AddPiece();
                _board.Tiles[_target.Item1, _target.Item2].RemovePiece();
                _played = false;
            }
        }

        public void Replay(Board board)
        {
            board.Tiles[Piece.Item1, Piece.Item2].RemovePiece();
            board.Tiles[_obstacle.Item1, _obstacle.Item2].RemovePiece();
            board.Tiles[_target.Item1, _target.Item2].AddPiece();
        }
    }
}
