using System.Collections;

namespace IEC104Parser
{
    /// <summary>
    /// Управляющая информация прикладного протокола (Application Protocol Control Information (APDU))
    /// </summary>
    public class APCI
    {
        /// <summary>
        /// Длина APDU
        /// </summary>
        public byte APDULenght { private set; get; }

        /// <summary>
        /// Поле управления 1
        /// </summary>
        private byte _controlField1;

        /// <summary>
        /// Тип кадра
        /// </summary>
        public IECPacketFormat Type { private set; get; }

        /// <summary>
        /// Форматы кадров IEC
        /// </summary>
        public enum IECPacketFormat
        {
            I = 0,
            S = 1,
            U = 2
        };

        /// <summary>
        /// Характеристики APCI
        /// </summary>
        public APCIFormat FormatData { private set; get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="data">Данные APCI в байтах</param>
        public APCI(byte[] data)
        {
            APDULenght = data[1];
            _controlField1 = data[2];
            SetType(data);
        }

        /// <summary>
        /// Определить тип кадра APCI
        /// </summary>
        private void SetType(byte[] data)
        {
            var binaryControlField1 = 
                new BitArray(new byte[] { _controlField1 });
            var binaryMask = new BitArray(new byte[] { 0b00000011 });
            var key = Services.BitArrayToInt32(binaryControlField1.And(binaryMask));
            switch (key)
            {
                case 0:
                    Type = IECPacketFormat.I;
                    FormatData = new APCIFormatI(data);
                    break;
                case 1:
                    Type = IECPacketFormat.S;
                    FormatData = new APCIFormatS();
                    break;
                case 2:
                    goto case 0;
                case 3:
                    Type = IECPacketFormat.U;
                    FormatData = new APCIFormatU();
                    break;
            }
        }
    }
}
