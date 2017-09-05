using System;
using System.Collections.Generic;

namespace Backtracking
{
    class Tile : ICloneable
    {
        TileType _type;
        TileState _state;

        public bool IsOccupied { get { return IsPlayable && _state == TileState.Occupied; } }
        public bool IsPlayable { get { return _type == TileType.Normal || _type == TileType.Goal; } }
        public bool IsGoal { get { return _type == TileType.Goal; } }

        public Tile(TileType type) : this(type, TileState.Free) { }

        public Tile(TileType type, TileState state)
        {
            _type = type;
            _state = state;
        }

        public Tile(Tile tile) : this(tile._type, tile._state) { }

        public override string ToString()
        {
            string output;
            switch (_type)
            {
                case TileType.Edge:
                    output = "░";
                    break;
                case TileType.Normal:
                case TileType.Goal:
                    switch (_state)
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

        internal void RemovePiece()
        {
            _state = TileState.Free;
        }

        internal void AddPiece()
        {
            if (_type == TileType.Edge)
            {
                throw new Exception();
            }
            _state = TileState.Occupied;
        }

        public object Clone()
        {
            return new Tile(_type, _state);
        }
    }
}
