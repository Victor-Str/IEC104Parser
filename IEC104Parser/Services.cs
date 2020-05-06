using System;
using System.Collections;

namespace IEC104Parser
{
    public static class Services
    {
        /// <summary>
        /// Преобразовать BitArray в Int32
        /// </summary>
        /// <param name="ba">BitArray-массив двоичных значений</param>
        /// <returns>Значание в Int32</returns>
        public static int BitArrayToInt32(BitArray ba)
        {
            int result = 0;
            for (int index = 0, m = 1; index < ba.Length; index++, m *= 2)
                result += ba.Get(index) ? m : 0;
            return result;
        }

        /// <summary>
        /// Преобразовать BitArray в Int32 с определенной позиции в массиве
        /// </summary>
        /// <param name="ba">BitArray-массив двоичных значений</param>
        /// <param name="position">Позиция в массиеве, начиная с которой будет осуществляться перевод в Int32</param>
        /// <returns>Значание в Int32</returns>
        public static int BitArrayToInt32(BitArray ba, int position)
        {
            int result = 0;
            for (int index = position, m = 1; index < ba.Length; index++, m *= 2)
                result += ba.Get(index) ? m : 0;
            return result;
        }
    }
}
