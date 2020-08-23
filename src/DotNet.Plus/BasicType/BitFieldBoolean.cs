using System;

namespace DotNet.Plus.BasicType
{
    public interface IBitFieldBoolean<TContainer> : IBitField<bool, TContainer>
        where TContainer : struct, IConvertible
    {
        bool HasFlag(TContainer container);

        TContainer SetFlag(TContainer container);
        TContainer ClearFlag(TContainer container);
        TContainer UpdateFlag(bool value, TContainer container);
    }

    public readonly struct BitFieldBoolean<TContainer> : IBitFieldBoolean<TContainer>
        where TContainer : struct, IConvertible
    {
        private readonly BitField<bool, TContainer> _bitField;

        public TContainer Bitmask => _bitField.Bitmask;

        public BitFieldBoolean(int bitPosition)
        {
            var maxContainerBits = IntegerDefinition<TContainer>.Size * Byte.BitsInByte;
            if( bitPosition >= IntegerDefinition<TContainer>.Size * Byte.BitsInByte )
                throw new ArgumentOutOfRangeException(nameof(bitPosition), bitPosition, $"Bit position is 0 based and beyond the size of the container {maxContainerBits}");
            
            _bitField = new BitField<bool, TContainer>(1, maxContainerBits - bitPosition - 1);
        }

        public static BitFieldBoolean<TContainer> MakeFromBitmask(TContainer bitmask) => new BitFieldBoolean<TContainer>(GetBitPositionFromBitmask(bitmask));

        public static int GetBitPositionFromBitmask(TContainer bitmask)
        {
            var bitmaskInfo = BitField<bool, TContainer>.ParseBitmask(bitmask);
            if( bitmaskInfo.numBits > 1 )
                throw new ArgumentOutOfRangeException(nameof(bitmask), bitmask, $"The value can contain only a single bit set");

            // Need to convert from startBitOffset to a bit position.
            var maxContainerBits = IntegerDefinition<TContainer>.Size * Byte.BitsInByte;
            return (byte)(maxContainerBits - bitmaskInfo.startBitOffset - 1);
        }

        public bool Decode(TContainer container) => _bitField.Decode(container);

        public TContainer Encode(bool value, TContainer container) => _bitField.Encode(value, container);

        public bool HasFlag(TContainer container) => Decode(container);

        public TContainer SetFlag(TContainer container) => UpdateFlag(true, container);

        public TContainer ClearFlag(TContainer container) => UpdateFlag(false, container);

        public TContainer UpdateFlag(bool value, TContainer container) => Encode(value, container);

        public static implicit operator BitField<bool, TContainer>(BitFieldBoolean<TContainer> bitField) => new BitField<bool, TContainer>(bitField.Bitmask);

        public static implicit operator BitFieldBoolean<TContainer>(BitField<bool, TContainer> bitField) => new BitFieldBoolean<TContainer>(GetBitPositionFromBitmask(bitField.Bitmask));

    }

}
