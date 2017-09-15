using System;

namespace Backtracking
{
    public class Move
    {
        readonly Tile start;
        readonly Direction direction;
        readonly Tile obstacle;
        readonly Tile target;
        readonly Board board;
        bool played = false;
        const int moveLength = 2;

        public bool IsValid
        {
            get
            {
                bool isValid = false;
                if (allTilesDefined)
                {
                    isValid = start.IsPlayable
                        && obstacle.IsPlayable
                        && target.IsPlayable
                        && start.IsOccupied
                        && obstacle.IsOccupied
                        && !target.IsOccupied;
                }
                return isValid;
            }
        }

        private bool allTilesDefined
        {
            get { return start != null && obstacle != null && target != null; }
        }

        public Move(Board board, Position piece, Direction direction)
        {
            this.board = board;
            this.direction = direction;

            start = board.AtPosition(piece);
            
            Position targetPosition = determineTargetPosition(piece, direction);            
            Position obstaclePosition = determineObstaclePosition(piece, targetPosition);
            if (isOnBoard(board, obstaclePosition))
            {
                obstacle = board.AtPosition(obstaclePosition);
            }
            if (isOnBoard(board, targetPosition))
            {
                target = board.AtPosition(targetPosition);
            }
        }

        private static bool isOnBoard(Board board, Position position)
        {
            return 0 <= position.Row && position.Row < board.Height
                && 0 <= position.Column && position.Column < board.Width;
        }

        private static Position determineObstaclePosition(Position piece, Position target)
        {
            return new Position((piece.Row + target.Row) / 2, (piece.Column + target.Column) / 2);
        }

        private static Position determineTargetPosition(Position piece, Direction direction)
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
                start.RemovePiece();
                obstacle.RemovePiece();
                target.AddPiece();
                played = true;
            }
        }

        public void Undo()
        {
            if(played)
            {
                start.AddPiece();
                obstacle.AddPiece();
                target.RemovePiece();
                played = false;
            }
        }

        public void Replay(Board board)
        {
            Position start = this.board.GetPosition(this.start);
            Position obstacle = this.board.GetPosition(this.obstacle);
            Position target = this.board.GetPosition(this.target);
            board.AtPosition(start).RemovePiece();
            board.AtPosition(obstacle).RemovePiece();
            board.AtPosition(target).AddPiece();
        }
    }
}
