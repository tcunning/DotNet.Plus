using System;

namespace DotNet.Plus.BasicType
{
    /// <inheritdoc cref="BitField{TValue}"/>
    public interface IBitField<TValue> : IBitField<TValue, TValue>
        where TValue : struct, IConvertible
    {
    }

    /// <summary>
    /// A BitField allows a sequence of bits to be identified within a container that is the same type.
    /// A value can then be encoded into or out of the sequence of bits and the BitField will take
    /// care of all the necessary masking and shifting. 
    /// <code>
    ///     // Define a value that is 4 bits and will be placed in a byte.
    ///     // The container is 8 bits and the value is in the 2nd nibble
    ///     var bitField = new BitField{byte}(0b1111_0000);
    ///
    ///     bitField.Encode(0b1010, 0b0000_0000);   // Returns a value of 0b1010_0000
    ///     bitField.Decode(0b1001_0000);           // Returns a value of 0b0000_1001
    /// </code>
    /// </summary>
    /// <typeparam name="TValue">An integer type that will be used to hold the encoded/decoded value.  
    /// BitMask must fit within this type size.</typeparam>
    public readonly struct BitField<TValue> : IBitField<TValue>
        where TValue : struct, IConvertible
    {
        private readonly BitField<TValue, TValue> _bitfield;

        /// <summary>
        /// This is the Bitmask that's used to represent where the value will be located.
        /// </summary>
        public TValue Bitmask => _bitfield.Bitmask;

        /// <summary>
        /// Creates BitField with the given bitmask.  
        /// </summary>
        /// <param name="bitmask">The bitmask must contain 0 or more sequential bits starting anywhere within
        /// the container.  There can only be ONE set of bits.  <c>For example 0b1100_0011 would be invalid.</c></param>
        public BitField(TValue bitmask)
        {
            _bitfield = new BitField<TValue, TValue>(bitmask);
        }

        /// <summary>
        /// Creates a BitField with the given number of bits starting at the given startBitOffset.  For example:
        /// <code>
        ///     var bitField = new BitField{byte}(3, 1);   // Would have a bitmask of 0b0111_0000
        /// </code>
        /// </summary>
        /// <param name="numBits">The number of bits used for the value</param>
        /// <param name="startBitOffset">A zero based index from the left most bit representing the start of the value.
        /// <code>
        ///                                              Bit 
        ///     Bit Order In Byte: 76543210 76543210    Offset
        ///                        |||||||| ||||||||    ======
        ///                        |||||||| |||||||\__    15  
        ///                        |||||||| ||||||\___    14
        ///                        |||||||| |||||\____    13
        ///                        |||||||| ||||\_____    12
        ///                        |||||||| |||\______    11
        ///                        |||||||| ||\_______    10
        ///                        |||||||| |\________    09
        ///                        |||||||| \_________    08
        ///                        |||||||\___________    07 
        ///                        ||||||\____________    06
        ///                        |||||\_____________    05
        ///                        ||||\______________    04
        ///                        |||\_______________    03
        ///                        ||\________________    02
        ///                        |\_________________    01
        ///                        \__________________    00
        /// </code>
        /// </param>
        public BitField(byte numBits, int startBitOffset)
        {
            _bitfield = new BitField<TValue, TValue>(numBits, startBitOffset);
        }

        /// <summary>
        /// Decodes a value that fits within TValue from the given container.
        /// <code>
        ///     // Define a value that is 4 bits and will be placed in a byte.
        ///     // The container is 8 bits and the value is in the 2nd nibble
        ///     var bitField = new BitField{byte}(0b1111_0000);
        ///
        ///     bitField.Decode(0b1001_0000);  // Returns a value of 0b0000_1010
        /// </code>
        /// </summary>
        /// <param name="container">The value is read from the container according to the BitField's bitmask as shifted appropriately</param>
        /// <returns></returns>
        public TValue Decode(TValue container) => _bitfield.Decode(container);

        /// <summary>
        /// Decodes a value that fits within TValue from the given container.
        /// <code>
        ///     // Define a value that is 4 bits and will be placed in a byte.
        ///     // The container is 16 bits and the value is in the 2nd nibble
        ///     var bitField = new BitField{byte}(0b1111_0000);
        /// 
        ///     bitField.Encode(0b1010, 0b0000_0000);   // Returns a value of 0b1010_0000
        /// </code>
        /// </summary>
        /// <param name="value">The value to be sifted and placed into the container according to BitField's bitmask</param>
        /// <param name="container">The value is applied to the container according to the BitField's bitmask as shifted appropriately</param>
        /// <returns>The original container with the value encoded into the appropriate place defined by the BitField's bitmask</returns>
        public TValue Encode(TValue value, TValue container) => _bitfield.Encode(value, container);
    }

}
