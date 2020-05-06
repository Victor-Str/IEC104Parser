using System;
using System.Collections.Generic;
using System.Windows;

namespace IEC104Parser
{
    /// <summary>
    /// Парсер TCP данных в формат кадров IEC 104
    /// </summary>
    public class ParserTCPPayLoad
    {
        /// <summary>
        /// Коллекция сформированных пакетов IEC 104
        /// </summary>
        public List<IEC104Packet> IECPackets
                    { get; private set; } = new List<IEC104Packet>();

        /// <summary>
        /// Буфер передатчика
        /// </summary>
        private Buffer _receiverBuffer = new Buffer(0);

        /// <summary>
        /// Буфер приемника
        /// </summary>
        private Buffer _transmitterBuffer = new Buffer(1);

        /// <summary>
        /// Пакеты IEC 104 в байтовом представлении для одного TCPPayLoad
        /// </summary>
        private List<byte[]> _IECRawPackets = new List<byte[]>();

        /// <summary>
        /// Начало следющего IEC 104 пакета
        /// </summary>
        private byte[] _residualEndPart;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ParserTCPPayLoad() { }

        /// <summary>
        /// Расшифровать полезную нагрузку TCPPayLoad
        /// </summary>
        /// <param name="payLoad">Объект TCPPayLoad</param>
        /// <param name="buffer">Буффер в заивисмости от направления передачи</param>
        /// <returns></returns>
        private List<IEC104Packet> DecodePayLoad(TCPPayLoad payLoad, Buffer buffer)
        {
            var packets = new List<IEC104Packet>();
            try
            {
                var rawPacketsList = SplitPayLoad(payLoad.Data, buffer);
                foreach (var rawPacket in rawPacketsList)
                {
                    var packet = new IEC104Packet(rawPacket, payLoad.Direction);
                    packets.Add(packet);
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
                //MessageBox.Show(ex.Message.ToString(),
                //    ex.Source.ToString(), MessageBoxButton.OK,
                //    MessageBoxImage.Error);
            }
            return packets;
        }

        /// <summary>
        /// Разделение полезной нагрузки на пакеты IEC 104
        /// </summary>
        /// <param name="data">Данные объекта TCPPayLoad</param>
        /// <param name="buffer">Буффер в заивисмости от направления передачи</param>
        /// <returns>Коллекция пакетов IEC 104 в байтах</returns>
        private List<byte[]> SplitPayLoad(byte[] data, Buffer buffer)
        {
            var firstKey = FindFirstByte(data);
            switch (firstKey)
            {
                case -1:
                    break;
                case 0:
                    FormRawIECPacketsList(data, buffer);
                    break;
                default:
                    var newData = new byte[data.Length - firstKey];
                    Array.Copy(data, firstKey, newData, 0, data.Length - firstKey);
                    FormRawIECPacketsList(newData, buffer);
                    break;
            }
            return _IECRawPackets;
        }

        /// <summary>
        /// Проверка первого байта на соответсвие началу пакета IEC 104
        /// </summary>
        /// <param name="firstByte">Первый байт PayLoad</param>
        /// <returns>true - первый байт равен 0x68, false - первый байт не равен 0x68</returns>
        private bool ValidationFirstByte(byte firstByte)
        {
            return firstByte == 104 ? true : false;
        }

        /// <summary>
        /// Поиск первого байта пакета IEC 104
        /// </summary>
        /// <param name="data">Массив информации в байтах:)</param>
        /// <returns>Возваращает индекс начала пакета IEC 104</returns>
        private int FindFirstByte(byte[] data)
        {
            var firstindex = -1;
            for (int i = 0; i < data.Length; i++)
            {
                if (ValidationFirstByte(data[i]))
                {
                    var APDUSize = data[i + 1];
                    var packetSize = APDUSize + 2;
                    if ((i + packetSize) == data.Length)
                    {
                        firstindex = i;
                        break;
                    }
                    else if ((i + packetSize) < data.Length)
                    {
                        if (ValidationFirstByte(data[i + packetSize]))
                        {
                            firstindex = i;
                            break;
                        }
                    }
                }
            }
            return firstindex;
        }

        /// <summary>
        /// Формирование коллекции пакетов IEC 104 в байтовом представлении
        /// </summary>
        /// <param name="data">Данные объекта TCPPayLoad</param>
        /// <param name="buffer">Буффер в заивисмости от направления передачи</param>
        private void FormRawIECPacketsList(byte[] data, Buffer buffer)
        {
            var key = true;
            var firstIndex = 0;
            while (key)
            {
                if (ValidationFirstByte(data[firstIndex]))
                {
                    var packetSize = data[firstIndex + 1] + 2;
                    if (firstIndex + packetSize <= data.Length)
                    {
                        var dataBuffer = new byte[packetSize];
                        Array.Copy(data, firstIndex, dataBuffer, 0, packetSize);
                        _IECRawPackets.Add(dataBuffer);
                        firstIndex += packetSize;
                        if (firstIndex == data.Length) key = false;
                    }
                    else
                    {
                        buffer.ResidualEndPart = new byte[data.Length - firstIndex];
                        Array.Copy(data, firstIndex, buffer.ResidualEndPart,
                                        0, data.Length - firstIndex);
                        key = false;
                    }
                }
            }
        }

        /// <summary>
        /// Получить IEC 104 пакеты
        /// </summary>
        /// <param name="payLoadList">Список из объектов - полезная нагрузка TCP - пакета</param>
        /// <returns></returns>
        public List<IEC104Packet> GetIECPackets(List<TCPPayLoad> payLoadList)
        {
            IECPackets.Clear();
            if (payLoadList.Count != 0)
            {
                foreach (var payLoad in payLoadList)
                {
                    Buffer buffer = SetBuffer(payLoad.Direction);
                    List<IEC104Packet> payLoadPackets;
                    if (_residualEndPart != null)
                    {
                        payLoad.AddDataInStart(_residualEndPart);
                        buffer.ResidualEndPart = null;
                    }
                    payLoadPackets = DecodePayLoad(payLoad, buffer);
                    IECPackets.AddRange(payLoadPackets);
                    payLoadPackets.Clear();
                    _IECRawPackets.Clear();
                }
            }
            return IECPackets;
        }

        /// <summary>
        /// Установить значение буффера в заивисмости от направления передачи
        /// </summary>
        /// <param name="direction">Направление передачи TCP-пакета</param>
        /// <returns>Буфер для направления передачи</returns>
        private Buffer SetBuffer(int direction)
        {
            Buffer buffer;
            if (direction == 0)
            {
                buffer = _receiverBuffer;
                _residualEndPart = _receiverBuffer.ResidualEndPart;
            }
            else
            {
                buffer = _transmitterBuffer;
                _residualEndPart = _transmitterBuffer.ResidualEndPart;
            }
            return buffer;
        }
    }

    #region Вложенный класс Buffer
    /// <summary>
    /// Буфер для хранения частей пакетов IEC 104
    /// </summary>
    internal class Buffer
    {
        /// <summary>
        /// Направление передачи
        /// </summary>
        public int Direction { private set; get; }

        /// <summary>
        /// Начало следющего IEC 104 пакета
        /// </summary>
        public byte[] ResidualEndPart { set; get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="direction">Направление передачи</param>
        public Buffer(int direction)
        {
            Direction = direction;
        }

        /// <summary>
        /// Очистить буфер
        /// </summary>
        public void Clear()
        {
            ResidualEndPart = null;
        }
    }
    #endregion
}
