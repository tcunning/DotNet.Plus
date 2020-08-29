using System;
using System.Collections.Generic;

namespace DotNet.Plus.BasicType
{
    /// <inheritdoc cref="BitField{TContainer}"/>
    public interface IBitFieldBoolean<TContainer> : IBitField<bool, TContainer>
        where TContainer : struct, IConvertible
    {
        /// <inheritdoc cref="BitFieldBoolean{TContainer}.HasFlag(TContainer)"/>
        bool HasFlag(TContainer container);

        /// <inheritdoc cref="BitFieldBoolean{TContainer}.SetFlag(TContainer)"/>
        TContainer SetFlag(TContainer container);

        /// <inheritdoc cref="BitFieldBoolean{TContainer}.ClearFlag(TContainer)"/>
        TContainer ClearFlag(TContainer container);

        /// <inheritdoc cref="BitFieldBoolean{TContainer}.UpdateFlag(bool, TContainer)"/>
        TContainer UpdateFlag(bool value, TContainer container);
    }

    /// <summary>
    /// A BitFieldBoolean allows a bit to be identified within a container.  
    /// <code>
    ///     // Define a boolean that is 1 bit within a UInt16.
    ///     var bitField = new BitFieldBoolean{UInt16}(0b0000_0000_1000_0000);
    ///
    ///     bitField.SetFlag(0b0000_0000_0000_0000);    // Returns a value of  0b0000_0000_1000_0000
    ///     bitField.ClearFlag(0b1111_1111_1111_1111);  // Returns a value of  0b1111_1111_0111_1111
    /// </code>
    /// </summary>
    /// <typeparam name="TContainer">An integer type that will be used as the source/destination of the
    /// boolean value.</typeparam>
    public readonly struct BitFieldBoolean<TContainer> : IBitFieldBoolean<TContainer>
        where TContainer : struct, IConvertible
    {
        private readonly BitField<bool, TContainer> _bitField;

        /// <summary>
        /// This is the Bitmask that's used to represent where the value is located.
        /// </summary>
        public TContainer Bitmask => _bitField.Bitmask;
        
        /// <summary>
        /// Creates a BitField with the given number of bits starting at the given startBitOffset.  For example:
        /// <code>
        ///     var bitField = new BitFieldBoolean{byte}(3);   // Would have a bitmask of 0b0000_1000
        /// </code>
        /// </summary>
        /// <param name="bitPosition">A zero based index from the left most bit representing the start of the value.
        /// Note: This is different from the bitOffset that the other BitField's use.   
        /// <code>
        ///                                               Bit 
        ///     Bit Order In Byte: 76543210 76543210    Position
        ///                        |||||||| ||||||||    ========
        ///                        |||||||| |||||||\__     00  
        ///                        |||||||| ||||||\___     01
        ///                        |||||||| |||||\____     02
        ///                        |||||||| ||||\_____     03
        ///                        |||||||| |||\______     04
        ///                        |||||||| ||\_______     05
        ///                        |||||||| |\________     06
        ///                        |||||||| \_________     07
        ///                        |||||||\___________     08 
        ///                        ||||||\____________     09
        ///                        |||||\_____________     10
        ///                        ||||\______________     11
        ///                        |||\_______________     12
        ///                        ||\________________     13
        ///                        |\_________________     14
        ///                        \__________________     15
        /// </code>
        /// </param>
        public BitFieldBoolean(int bitPosition)
        {
            var maxContainerBits = IntegerDefinition<TContainer>.Size * Byte.BitsInByte;
            if( bitPosition >= IntegerDefinition<TContainer>.Size * Byte.BitsInByte )
                throw new ArgumentOutOfRangeException(nameof(bitPosition), bitPosition, $"Bit position is 0 based and beyond the size of the container {maxContainerBits}");
            
            _bitField = new BitField<bool, TContainer>(1, maxContainerBits - bitPosition - 1);
        }

        /// <summary>
        /// Creates BitField with the given bitmask.  
        /// </summary>
        /// <param name="bitmask">The bitmask must contain none or at most 1 bit set.</param>
        /// <returns>A BitFieldBoolean that matches the given bitmask</returns>
        public static BitFieldBoolean<TContainer> MakeFromBitmask(TContainer bitmask) => 
            new BitFieldBoolean<TContainer>(GetBitPositionFromBitmask(bitmask));

        private static int GetBitPositionFromBitmask(TContainer bitmask)
        {
            var bitmaskInfo = BitField<bool, TContainer>.ParseBitmask(bitmask);
            if( bitmaskInfo.numBits > 1 )
                throw new ArgumentOutOfRangeException(nameof(bitmask), bitmask, $"The bitmask can contain only a single bit set");

            if( bitmaskInfo.numBits == 0 )
                throw new ArgumentOutOfRangeException(nameof(bitmask), bitmask, $"The bitmask must contain a set bit");

            // Need to convert from startBitOffset to a bit position.
            var maxContainerBits = IntegerDefinition<TContainer>.Size * Byte.BitsInByte;
            return (byte)(maxContainerBits - bitmaskInfo.startBitOffset - 1);
        }

        /// <inheritdoc cref="BitField{TContainer}.Decode(TContainer)"/>
        public bool Decode(TContainer container) => _bitField.Decode(container);

        /// <summary>
        /// Decodes a value that fits within TValue from the given buffer.  The buffer must be big enough
        /// to fit the entire TContainer
        /// <code>
        ///     // Define a value that is 4 bits and will be placed in a byte.
        ///     // The container is 8 bits and the value is in the 2nd nibble
        ///     var bitField = new BitField{byte}(0b1000_0000);
        /// 
        ///     bitField.Decode(new byte[] { 0x80, 0x00 }, 0);  // Returns true
        /// </code>
        /// </summary>
        /// <param name="buffer">The buffer to read the bit from.  The buffer must be big enough to fit the entire
        /// TContainer</param>
        /// <param name="offset">A zero based index from where to start within the given buffer</param>
        /// <returns>True if the bit is set, otherwise false</returns>
        public bool Decode(IList<byte> buffer, int offset = 0)
        {
            var byteOffset = _bitField.StartBitOffset / Byte.BitsInByte;
            var bitShift = (_bitField.StartBitOffset % Byte.BitsInByte);
            var byteBitmask = 0b1000_0000 >> bitShift;
            return (buffer[offset + byteOffset] & byteBitmask) != 0;
        }

        /// <inheritdoc cref="BitField{TValue, TContainer}.Encode(TValue, TContainer)"/>
        public TContainer Encode(bool value, TContainer container) => _bitField.Encode(value, container);

        /// <summary>
        /// Encodes a value into the given buffer
        /// <code>
        ///     // Define a value that is 4 bits and will be placed in a byte.
        ///     // The container is 16 bits and the value is in the 2nd nibble
        ///     var bitField = new BitField{byte, UInt16}(0b0000_0000_1111_0000);
        /// 
        ///     bitField.Encode(0b1010, 0b0000_0000_0000_0000);   // Returns a value of 0b0000_0000_1010_0000
        /// </code>
        /// </summary>
        /// <param name="value">The value to be sifted and placed into the buffer according to BitField's bitmask</param>
        /// <param name="buffer">That buffer that will have a bit set, it must be large enough to old the TContainer</param>
        /// <param name="offset">A zero based index from where to start within the given buffer</param>
        /// <returns>To allow for chaining it returns the passed in buffer</returns>
        public TBuffer Encode<TBuffer>(bool value, TBuffer buffer, int offset = 0)
            where TBuffer : IList<byte>
        {
            var byteOffset = _bitField.StartBitOffset / Byte.BitsInByte;
            var byteBitmask = (byte)(0b1000_0000 >> (_bitField.StartBitOffset % Byte.BitsInByte));
            var index = offset + byteOffset;
            if (value)
                buffer[index] |= byteBitmask;
            else
                buffer[index] &= (byte)~byteBitmask;

            return buffer;
        }

        /// <summary>
        /// Tests to see if the flag is set within the given container
        /// </summary>
        /// <param name="container">The container</param>
        /// <returns>True if the bit is set within the container</returns>
        public bool HasFlag(TContainer container) => Decode(container);

        /// <summary>
        /// Sets the flag to true within the given container
        /// </summary>
        /// <param name="container">The container</param>
        /// <returns>The container with the specified bit set</returns>
        public TContainer SetFlag(TContainer container) => UpdateFlag(true, container);

        /// <summary>
        /// Clears the flag to false within the given container
        /// </summary>
        /// <param name="container">The container</param>
        /// <returns>The container with the specified bit cleared</returns>
        public TContainer ClearFlag(TContainer container) => UpdateFlag(false, container);

        /// <summary>
        /// Sets or clears the flag within the container depending on the value given
        /// </summary>
        /// <param name="value">True will set the flag and false will clear the flag</param>
        /// <param name="container">The container</param>
        /// <returns>The container with the specified bit cleared or set</returns>
        public TContainer UpdateFlag(bool value, TContainer container) => Encode(value, container);

        public static implicit operator BitField<bool, TContainer>(BitFieldBoolean<TContainer> bitField) => new BitField<bool, TContainer>(bitField.Bitmask);

        public static implicit operator BitFieldBoolean<TContainer>(BitField<bool, TContainer> bitField) => new BitFieldBoolean<TContainer>(GetBitPositionFromBitmask(bitField.Bitmask));

    }

}
