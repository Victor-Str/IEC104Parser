using IEC104Parser.ASDUClasses;
using System;
using System.Collections;

namespace IEC104Parser
{
    /// <summary>
    /// Блок данных прикладного уровня
    /// </summary>
    public class ASDU
    {
        /// <summary>
        /// Тип индентефикатора
        /// </summary>
        public TypeID Type { private set; get; }

        /// <summary>
        /// Значение SQ
        /// </summary>
        public bool SQ { private set; get; } = false;

        /// <summary>
        /// Количесвто информационных объектов
        /// </summary>
        public int NumberOfObjects { private set; get; }

        /// <summary>
        /// Тестовый бит
        /// </summary>
        public bool T { private set; get; } = false;

        /// <summary>
        /// Бит подтверждения активации
        /// </summary>
        public bool PN { private set; get; } = false;

        /// <summary>
        /// Причина передачи
        /// </summary>
        public CauseOfTransmission COT { private set; get; }

        /// <summary>
        /// Адрес инициатора
        /// </summary>
        public byte OA { private set; get; }

        /// <summary>
        /// Адрес АСДУ
        /// </summary>
        public int ASDUAddress { private set; get; }

        /// <summary>
        /// Данные ASDU в байтах
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="data">Данные ASDU в байтах</param>
        public ASDU(byte[] data)
        {
            _data = data;
            SetTypeID();
            SetSQ();
            SetNumberOfObjects();
            SetTByte();
            SetPNByte();
            SetCOT();
            SetOA();
            SetASDUAdress();
        }

        /// <summary>
        /// Установить тип индентефикатора 
        /// </summary>
        private void SetTypeID()
        {
            var key = _data[0];
            Type = (TypeID)key;
        }

        /// <summary>
        /// Установить SQ
        /// </summary>
        private void SetSQ()
        {
            var binarySQ =new BitArray(new byte[] { _data[1] });
            SQ = binarySQ[7];
        }

        /// <summary>
        /// Установить количество информационных объектов
        /// </summary>
        private void SetNumberOfObjects()
        {
            var binaryNumberOfObjects = new BitArray(new byte[] { _data[1] });
            var binaryMask = new BitArray(new byte[] { 0b01111111 });
            NumberOfObjects = 
                Services.BitArrayToInt32(binaryNumberOfObjects.And(binaryMask));
        }

        /// <summary>
        /// Установить тестовый бит
        /// </summary>
        private void SetTByte()
        {
            var binarySQ = new BitArray(new byte[] { _data[2] });
            T = binarySQ[7];
        }

        /// <summary>
        /// Установить бит подтверждения активации
        /// </summary>
        private void SetPNByte()
        {
            var binaryPN = new BitArray(new byte[] { _data[2] });
            PN = binaryPN[6];
        }

        /// <summary>
        /// Установить причину передачи
        /// </summary>
        private void SetCOT()
        {
            var binaryCOT = new BitArray(new byte[] { _data[2] });
            var binaryMask = new BitArray(new byte[] { 0b00111111 });
            var key = Services.BitArrayToInt32(binaryCOT.And(binaryMask));
            COT = (CauseOfTransmission)key;
        }

        /// <summary>
        /// Установить адрес инициатора
        /// </summary>
        private void SetOA()
        {
            OA = _data[3];
        }

        /// <summary>
        /// Установить адресс АСДУ
        /// </summary>
        private void SetASDUAdress()
        {
            ASDUAddress = BitConverter.ToInt32(new byte[] { _data[4], _data[5], 0, 0 }, 0);
        }
    }
}
