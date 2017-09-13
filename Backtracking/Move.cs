using System;

namespace Backtracking
{
    sealed class Move
    {
        readonly Position Start;
        readonly Direction Direction;
        readonly Position obstacle;
        readonly Position target;
        readonly Board board;
        bool played = false;
        const int moveLength = 2;

        public bool IsValid
        {
            get
            {
                return board.Tiles[Start.Row, Start.Column].IsPlayable &&
                    board.Tiles[obstacle.Row, obstacle.Column].IsPlayable &&
                    board.Tiles[target.Row, target.Column].IsPlayable &&
                    board.Tiles[Start.Row, Start.Column].IsOccupied &&
                    board.Tiles[obstacle.Row, obstacle.Column].IsOccupied &&
                    !board.Tiles[target.Row, target.Column].IsOccupied;
            }
        }

        public Move(Board board, Position piece, Direction direction)
        {
            this.board = board;
            Start = piece;
            Direction = direction;
            target = determineTarget(piece, direction);
            obstacle = determineObstacle(piece, target);
        }

        private static Position determineObstacle(Position piece, Position target)
        {
            return new Position((piece.Row + target.Row) / 2, (piece.Column + target.Column) / 2);
        }

        private static Position determineTarget(Position piece, Direction direction)
        {
            Position target;
            switch (direction)
            {
                case Direction.Up:
                    target = new Position(piece.Row - moveLength, piece.Column);
                    break;
                case Direction.Right:
                    target = new Position(piece.Row, piece.Column + moveLength);
                    break;
                case Direction.Down:
                    target = new Position(piece.Row + moveLength, piece.Column);
                    break;
                case Direction.Left:
                    target = new Position(piece.Row, piece.Column - moveLength);
                    break;
                default:
                    throw new ArgumentException();
            }
            return target;
        }

        public void Play()
        {
            if (IsValid)
            {
                board.Tiles[Start.Row, Start.Column].RemovePiece();
                board.Tiles[obstacle.Row, obstacle.Column].RemovePiece();
                board.Tiles[target.Row, target.Column].AddPiece();
                played = true;
            }
        }

        public void Undo()
        {
            if(played)
            {
                board.Tiles[Start.Row, Start.Column].AddPiece();
                board.Tiles[obstacle.Row, obstacle.Column].AddPiece();
                board.Tiles[target.Row, target.Column].RemovePiece();
                played = false;
            }
        }

        public void Replay(Board board)
        {
            board.Tiles[Start.Row, Start.Column].RemovePiece();
            board.Tiles[obstacle.Row, obstacle.Column].RemovePiece();
            board.Tiles[target.Row, target.Column].AddPiece();
        }
    }
}
