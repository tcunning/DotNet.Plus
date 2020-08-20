using System;
using DotNet.Plus.Endian;

namespace DotNet.Plus.BasicType
{
    /// <summary>
    /// Extensions to work with GUIDs
    /// </summary>
    public static class GuidUtil
    {
        /// <summary>
        /// Required size of a GUID
        /// </summary>
        public const int GuidSize = 16;

        /// <summary>
        /// Converts the give byte into a guid.  The guid can be stored in a big endian or little
        /// endian format.
        /// </summary>
        /// <param name="values">The byte array</param>
        /// <param name="startOffset">Starting offset of where to start reading the guid</param>
        /// <param name="endian">How to interpret the bytes (big or little endian)</param>
        /// <returns>The byte array as a guid</returns>
        /// <exception cref="ArgumentOutOfRangeException">If there aren't enough bytes in the guid</exception>
        public static Guid ToGuid(this byte[] values, int startOffset = 0, EndianFormat endian = EndianFormat.Big)
        {
            var neededSize = startOffset + GuidSize;
            if( neededSize > values.Length )
                throw new ArgumentOutOfRangeException(nameof(values), values.Length, $"Array too small to read full GUID with given startOffset");

            var index = startOffset;
            var a = values.ToInt32(index, endian);
            index += sizeof(Int32);

            var b = values.ToInt16(index, endian);
            index += sizeof(Int16);

            var c = values.ToInt16(index, endian);
            index += sizeof(Int16);

            var byteArray = new byte[8];
            for( var byteIndex = 0; byteIndex < 8; byteIndex += 1 )
                byteArray[byteIndex] = values[index++];

            var guid = new Guid(a, b, c, byteArray);
            return guid;
        }
    }
}
