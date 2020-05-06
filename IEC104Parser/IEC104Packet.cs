using System;
using static IEC104Parser.APCI;

namespace IEC104Parser
{
    /// <summary>
    /// Пакет протокола IEC 60870-5-104
    /// </summary>
    public class IEC104Packet
    {
        /// <summary>
        /// Направление пакета
        /// </summary>
        public int Direction { set; get; }

        /// <summary>
        /// Управляющая информация прикладного протокола
        /// </summary>
        public APCI APCI { private set; get; }

        /// <summary>
        /// Блок данных прикладного уровня
        /// </summary>
        public ASDU ASDU { private set; get; }

        /// <summary>
        /// Длина пакета
        /// </summary>
        public int Lenght { private set; get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="data">Пакет в байтовом представлении</param>
        public IEC104Packet(byte[] data)
        {
            Lenght = data.Length;
            SetFields(data);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="data">Пакет в байтовом представлении</param>
        /// <param name="direction">Направление пакета</param>
        public IEC104Packet(byte[] data, int direction)
        {
            Direction = direction;
            Lenght = data.Length;
            SetFields(data);
        }

        /// <summary>
        /// Установка значения APCI
        /// </summary>
        /// <param name="data">Пакет в байтовом представлении</param>
        private void SetAPCI(byte[] data)
        {
            if (data.Length >= 6)
            {
                var APCIData = new byte[6];
                Array.Copy(data, 0, APCIData, 0, 6);
                APCI = new APCI(APCIData);
            }
        }

        /// <summary>
        /// Установка значения APCI
        /// </summary>
        /// <param name="data">Пакет в байтовом представлении</param>
        private void SetASDU(byte[] data)
        {
            var ASDUData = new byte[data.Length - 6];
            Array.Copy(data, 6, ASDUData, 0, data.Length - 6);
            ASDU = new ASDU(ASDUData);
        }

        /// <summary>
        /// Формирование IEC 104 пакета
        /// </summary>
        /// <param name="data">Данные пакета в байтах</param>
        private void SetFields(byte[] data)
        {
            SetAPCI(data);
            if (APCI.Type==IECPacketFormat.I)
            {
                SetASDU(data);
            }
        }
    }
}
