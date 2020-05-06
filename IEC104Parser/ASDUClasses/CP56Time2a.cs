using System;
using System.Collections;

//http://docs.cntd.ru/document/1200027397
// Состоит из 7 байт, конструктор должен принимать на вход 7 байт информации

namespace IEC104Parser.ASDUClasses
{
    /// <summary>
    /// Время в формате CP56Time
    /// </summary>
    public class CP56Time2a
    {
        /// <summary>
        /// Дата и время
        /// </summary>
        public DateTime DateTime { private set; get; }

        /// <summary>
        /// Данные в байтах
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// Миллисекунды
        /// </summary>
        private int _milleseconds;

        /// <summary>
        /// Секунды
        /// </summary>
        private int _seconds;

        /// <summary>
        /// Минуты
        /// </summary>
        private int _minutes;

        /// <summary>
        /// Часы
        /// </summary>
        private int _hours;

        /// <summary>
        /// Число месяца
        /// </summary>
        private int _day;

        /// <summary>
        /// Номер месяца
        /// </summary>
        private int _month;

        /// <summary>
        /// Год
        /// </summary>
        private int _year;

        /// <summary>
        /// Сезон времени
        /// </summary>
        public Season TimeSeason { get; private set; }

        /// <summary>
        /// Временные сезоны
        /// </summary>
        public enum Season
        {
            Winter = 0,
            Summer = 1
        }

        /// <summary>
        /// Указатель на дейтсвительность
        /// </summary>
        public ValidationType IV { private set; get; }

        /// <summary>
        /// Типы указателей на дейтсвительность
        /// </summary>
        public enum ValidationType
        {
            Valid = 0,
            Invalid = 1
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="data">данные в байтах</param>
        public CP56Time2a(byte[] data)
        {
            _data = data;
            SetFields();
            SetDateTime();
        }

        /// <summary>
        /// Парсинг байтов и заполнение полей информацией
        /// </summary>
        private void SetFields()
        {
            SetSeconds();
            SetMinutes();
            SetHours();
            SetSeason();
            SetDay();
            SetMonth();
            SetYear();
            SETIV();
        }

        /// <summary>
        /// Установить миллесекунды и секунды
        /// </summary>
        private void SetSeconds()
        {
            BitArray binaryMilleseconds = new BitArray(new byte[] { _data[0], _data[1] });
            var milleseconds = Services.BitArrayToInt32(binaryMilleseconds);
            _milleseconds = milleseconds % 1000;
            _seconds = milleseconds / 1000;
        }

        /// <summary>
        /// Установить минуты
        /// </summary>
        private void SetMinutes()
        {
            BitArray binaryMinutes = new BitArray(new byte[] { _data[2] });
            BitArray binaryMask = new BitArray(new byte[] { 0b00111111 });
            _minutes = Services.BitArrayToInt32(binaryMinutes.And(binaryMask));
        }

        /// <summary>
        /// Установить действительность\недействительность
        /// </summary>
        private void SETIV()
        {
            var thirdByte = _data[2];
            var checkBit = FormBooleanString(thirdByte).Substring(0, 1);
            var type=Convert.ToInt32(checkBit,2);
            IV = (ValidationType)type;
        }

        /// <summary>
        /// Установить часы
        /// </summary>
        private void SetHours()
        {
            var fourthByte = _data[3];
            var hours = FormBooleanString(fourthByte);
            _hours = Convert.ToInt32(hours.Substring(3), 2);
        }

        /// <summary>
        /// Установить сезон времени
        /// </summary>
        private void SetSeason()
        {
            var fourthByte = _data[3];
            var hours = FormBooleanString(fourthByte);
            var type = Convert.ToInt32(hours.Substring(0, 1), 2);
            TimeSeason = (Season)type;
        }

        /// <summary>
        /// Установить день
        /// </summary>
        private void SetDay()
        {
            var fifthByte = _data[4];
            var day = FormBooleanString(fifthByte).Substring(3, 5);
            _day = Convert.ToInt32(day, 2);
        }

        /// <summary>
        /// Установить месяц
        /// </summary>
        private void SetMonth()
        {
            var sixthByte = _data[5];
            var month = FormBooleanString(sixthByte).Substring(4, 4);
            _month = Convert.ToInt32(month, 2);
        }

        /// <summary>
        /// Установить год
        /// </summary>
        private void SetYear()
        {
            var seventh = _data[6];
            var month = FormBooleanString(seventh).Substring(1, 7);
            _year = 2000 + Convert.ToInt32(month, 2);
        }

        /// <summary>
        /// Установить значение времени и даты
        /// </summary>
        private void SetDateTime()
        {
            DateTime = new DateTime(_year,_month,_day,_hours,_minutes,_seconds, _milleseconds);
        }

        /// <summary>
        /// Сформировать битовую строку из байта
        /// </summary>
        /// <param name="checkByte">Байт информации</param>
        /// <returns>Строку, состояющую из бит</returns>
        private string FormBooleanString(byte checkByte)
        {
            var newString = Convert.ToString(checkByte, 2);
            while (newString.Length < 8)
            {
                newString = "0" + newString;
            }
            return newString;
        }
    }
}
