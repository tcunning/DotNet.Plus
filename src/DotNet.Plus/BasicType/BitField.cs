using System;

namespace DotNet.Plus.BasicType
{
    public interface IBitField<TValue> : IBitField<TValue, TValue>
        where TValue : struct, IConvertible
    {
    }
    
    public readonly struct BitField<TValue> : IBitField<TValue>
        where TValue : struct, IConvertible
    {
        private readonly BitField<TValue, TValue> _bitfield;

        public TValue Bitmask => _bitfield.Bitmask;

        public BitField(TValue bitfield)
        {
            _bitfield = new BitField<TValue, TValue>(bitfield);
        }

        public BitField(byte numBits, int startBitOffset)
        {
            _bitfield = new BitField<TValue, TValue>(numBits, startBitOffset);
        }

        public TValue Decode(TValue container) => _bitfield.Decode(container);

        public TValue Encode(TValue value, TValue container) => _bitfield.Encode(value, container);
    }
}
