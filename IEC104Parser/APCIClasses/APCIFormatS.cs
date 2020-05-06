using System.Collections;

namespace IEC104Parser
{
    /// <summary>
    /// APCI - формат S
    /// </summary>
    public class APCIFormatS : APCIFormat
    {
        /// <summary>
        /// Rx
        /// </summary>
        public int Rx { private set; get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public APCIFormatS()
        {
            SetRx();
        }

        /// <summary>
        /// Определить Rx
        /// </summary>
        private void SetRx()
        {
            var binaryRxValue =
                new BitArray(new byte[] { _controlField3, _controlField4 });
            var binaryMask =
                new BitArray(new byte[] { 0b01111111, 0b11111111 });
            Rx = Services.BitArrayToInt32(binaryRxValue.And(binaryMask), 1);
        }
    }
}
