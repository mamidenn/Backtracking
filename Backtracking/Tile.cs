using System;
using System.Collections.Generic;

namespace Backtracking
{
    public class Tile
    {
        TileType type;
        TileState state;

        const string edgeSymbol = "░";
        const string occupiedSymbol = "O";
        const string freeSymbol = " ";

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
                    output = edgeSymbol;
                    break;
                case TileType.Normal:
                case TileType.Goal:
                    output = stateToString(state);
                    break;
                default:
                    throw new KeyNotFoundException();
            }
            return output;
        }

        private static string stateToString(TileState state)
        {
            return state == TileState.Occupied ? occupiedSymbol : freeSymbol;
        }

        public void RemovePiece()
        {
            failOnTileState(TileState.Free);
            state = TileState.Free;
        }

        public void AddPiece()
        {
            failOnTileState(TileState.Occupied);
            state = TileState.Occupied;
        }

        void failOnTileState(TileState failState)
        {
            if (!IsPlayable)
            {
                throw new TileIsNotPlayableException();
            }
            if (state == failState)
            {
                throw new TileOccupationException();
            }
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
