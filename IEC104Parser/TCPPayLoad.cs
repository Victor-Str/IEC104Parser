using System;
using System.Windows;

namespace IEC104Parser
{
    /// <summary>
    /// Класс - полезная нагрузка TCP пакета
    /// </summary>
    public class TCPPayLoad
    {
        /// <summary>
        /// Направление передачи
        /// </summary>
        public int Direction { get; set; }

        /// <summary>
        /// Номер пакета
        /// </summary>
        public int PeerNumber { get; set; }

        /// <summary>
        /// Данные в байтах
        /// </summary>
        public byte[] Data { private set; get; }

        /// <summary>
        /// Количество байт в PayLoad
        /// </summary>
        public int PayLoadSize { private set; get; } = 0;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="direction">Направление передачи</param>
        /// <param name="peerNumber">Номер пакета</param>
        /// <param name="rawDataBase64">Полезная нагрузка TCP-пакета в формате Base64</param>
        public TCPPayLoad(int direction, int peerNumber, string rawDataBase64)
        {
            Direction = direction;
            PeerNumber = peerNumber;
            SetData(rawDataBase64);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="direction">Направление передачи</param>
        /// <param name="peerNumber">Номер пакета</param>
        /// <param name="data">Полезная нагрузка TCP-пакета в байтах</param>
        public TCPPayLoad(int direction, int peerNumber, byte[] data)
        {
            Direction = direction;
            PeerNumber = peerNumber;
            if (data.Length != 0) Data = data;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="rawDataBase64">Полезная нагрузка TCP-пакета в формате Base64</param>
        public TCPPayLoad(string rawDataBase64)
        {
            SetData(rawDataBase64);
        }

        /// <summary>
        /// Получить нагрузку TCP-пакета в байтах
        /// </summary>
        /// <param name="rawDataBase64">Полезная нагрузка TCP-пакета</param>
        private void SetData(string rawDataBase64)
        {
            if (!string.IsNullOrEmpty(rawDataBase64))
            {
                try
                {
                    Data = Convert.FromBase64String(rawDataBase64);
                    PayLoadSize = Data.Length;
                }
                catch (FormatException exeption)
                {
                    MessageBox.Show(exeption.Message.ToString(),
                        exeption.Source.ToString(), MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Добавить данные в начало Data
        /// </summary>
        /// <param name="startPart">Массив данных в байтах</param>
        public void AddDataInStart(byte[] startPart)
        {
            byte[] newData = new byte[startPart.Length + Data.Length];
            Array.Copy(startPart, 0, newData, 0, startPart.Length);
            Array.Copy(Data, 0, newData, startPart.Length, Data.Length);
            Data = newData;
        }
    }
}
