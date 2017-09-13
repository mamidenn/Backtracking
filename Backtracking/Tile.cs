using System;
using System.Collections.Generic;

namespace Backtracking
{
    public class Tile
    {
        TileType type;
        TileState state;

        public bool IsOccupied { get { return IsPlayable && state == TileState.Occupied; } }
        public bool IsPlayable { get { return type == TileType.Normal || type == TileType.Goal; } }
        public bool IsGoal { get { return type == TileType.Goal; } }

        public Tile(TileType type) : this(type, TileState.Free) { }

        public Tile(TileType type, TileState state)
        {
            this.type = type;
            this.state = state;
        }

        public Tile(Tile tile) : this(tile.type, tile.state) { }

        public override string ToString()
        {
            string output;
            switch (type)
            {
                case TileType.Edge:
                    output = "░";
                    break;
                case TileType.Normal:
                case TileType.Goal:
                    switch (state)
                    {
                        case TileState.Free:
                            output = " ";
                            break;
                        case TileState.Occupied:
                            output = "o";
                            break;
                        default:
                            throw new KeyNotFoundException();
                    }
                    break;
                default:
                    throw new KeyNotFoundException();
            }
            return output;
        }

        public void RemovePiece()
        {
            if (!IsPlayable)
            {
                throw new TileIsNotPlayableException();
            }
            if (!IsOccupied)
            {
                throw new TileIsNotOccupiedException();
            }
            state = TileState.Free;
        }

        public void AddPiece()
        {
            if (!IsPlayable)
            {
                throw new TileIsNotPlayableException();
            }
            if (IsOccupied)
            {
                throw new TileIsOccupiedException();
            }
            state = TileState.Occupied;
        }

        public static Tile Parse(char tileCode)
        {
            Tile tile;
            switch (tileCode)
            {
                case 'X':
                    tile = new Tile(TileType.Edge);
                    break;
                case 'O':
                    tile = new Tile(TileType.Normal, TileState.Occupied);
                    break;
                case 'G':
                    tile = new Tile(TileType.Goal, TileState.Free);
                    break;
                case ' ':
                    tile = new Tile(TileType.Normal, TileState.Free);
                    break;
                default:
                    throw new ArgumentException("Tiles can only be X, O, G or space character.");
            }
            return tile;
        }
    }
}
