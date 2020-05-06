using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEC104Parser
{
    /// <summary>
    /// Application Protocol Control Information
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
        /// Поле управления 2
        /// </summary>
        private byte _controlField2;

        /// <summary>
        /// Поле управления 3
        /// </summary>
        private byte _controlField3;

        /// <summary>
        /// Поле управления 4
        /// </summary>
        private byte _controlField4;

        /// <summary>
        /// Данные
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// Тип кадра
        /// </summary>
        public string Type { private set; get; }

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
        /// <param name="data">Данные в байтах</param>
        public APCI(byte[] data)
        {
            APDULenght = data[1];
            _controlField1 = data[2];
            _controlField2 = data[3];
            _controlField3 = data[4];
            _controlField4 = data[5];
            SetValues();
        }

        /// <summary>
        /// Заполнение информации
        /// </summary>
        private void SetValues()
        {
            SetType();
            SetTx();
            SetRx();
        }

        /// <summary>
        /// Определить тип кадра APCI
        /// </summary>
        private void SetType()
        {
            var checkType = Convert.ToString(_controlField1, 2);
            if (checkType.Length<2)
            {
                checkType = "0" + checkType;
            }
            else
            {
                checkType = checkType.Substring(checkType.Length - 2);
            }
            switch (checkType)
            {
                case "00":
                    Type = "I";
                    break;
                case "10":
                    Type = "I";
                    break;
                case "01":
                    Type = "S";
                    break;
                case "11":
                    Type = "U";
                    break;
            }
        }

        /// <summary>
        /// Определить Tx
        /// </summary>
        private void SetTx()
        {
            if (Type=="I")
            {
                var CF1 = Convert.ToString(_controlField1,2);
                CF1 = CF1.Substring(0, CF1.Length - 1);
                var CF1Length = CF1.Length;
                if (CF1Length != 7)
                {
                    for (int i = 1; i <= 7- CF1Length; i++)
                    {
                        CF1 = "0" + CF1;
                    }
                }
                var CF2= Convert.ToString(_controlField2, 2);
                Tx = Convert.ToInt32((CF2+CF1),2);
            }
        }

        /// <summary>
        /// Определить Rx
        /// </summary>
        private void SetRx()
        {
            if (Type == "I" || Type == "S")
            {
                var CF3 = Convert.ToString(_controlField3, 2);
                CF3 = CF3.Substring(0, CF3.Length - 1);
                var CF1Length = CF3.Length;
                if (CF1Length != 7)
                {
                    for (int i = 1; i <= 7 - CF1Length; i++)
                    {
                        CF3 = "0" + CF3;
                    }
                }
                var CF4 = Convert.ToString(_controlField4, 2);
                Rx = Convert.ToInt32((CF4 + CF3), 2);
            }
        }

        /// <summary>
        /// Форматы кадров APCI
        /// </summary>
        private static Dictionary<string, string> APCITypes = new Dictionary<string, string>
        {
            {"I", "00"},
            {"S", "01"},
            {"U", "11"}
        };
    }
}
