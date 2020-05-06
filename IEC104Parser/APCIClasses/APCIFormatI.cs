using System.Collections;

namespace IEC104Parser
{
    /// <summary>
    /// APCI - формат I
    /// </summary>
    public class APCIFormatI : APCIFormat
    {
        /// <summary>
        /// Tx
        /// </summary>
        public int Tx { private set; get; }

        /// <summary>
        /// Rx
        /// </summary>
        public int Rx { private set; get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="data"></param>
        public APCIFormatI(byte [] data)
        {
            _controlField1 = data[2];
            _controlField2 = data[3];
            _controlField3 = data[4];
            _controlField4 = data[5];
            SetRx();
            SetTx();
        }

        /// <summary>
        /// Определить Tx
        /// </summary>
        private void SetTx()
        {
            var binaryTxValue = 
                new BitArray(new byte[] { _controlField1, _controlField2});
            var binaryMask = 
                new BitArray(new byte[] { 0b11111110, 0b11111111 });
            Tx = Services.BitArrayToInt32(binaryTxValue.And(binaryMask), 1);
        }

        /// <summary>
        /// Определить Rx
        /// </summary>
        private void SetRx()
        {
            var binaryRxValue = 
                new BitArray(new byte[] { _controlField3, _controlField4 });
            var binaryMask = 
                new BitArray(new byte[] { 0b11111110, 0b11111111 });
            Rx = Services.BitArrayToInt32(binaryRxValue.And(binaryMask), 1);
        }
    }
}
