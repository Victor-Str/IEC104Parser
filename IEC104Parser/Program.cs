using IEC104Parser.ASDUClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace IEC104Parser
{
    class Program
    {
        static void Main(string[] args)
        {


            byte[] data = new byte[] { 0xd4, 0x05, 0x00 };
            var asdasa = new byte[] { 0x04, 0x0f };

            var a = (byte)(0x04 % 256);
            var b = (byte)(0x0f / 256);

            BitArray bitArray = new BitArray(data);

            var aa = Services.BitArrayToInt32(bitArray);






            var strtTime = DateTime.Now;
            Console.WriteLine(strtTime);

            var filePath = @"C:\Users\Baderko-RV\Desktop\2.txt";
            string rawDataBase64 = null;
            using (StreamReader stream = new StreamReader(filePath))
            {
                rawDataBase64 = stream.ReadToEnd();
            }

            var TCPPayLoadList = GetTCPPayLoadList(rawDataBase64);
            ParserTCPPayLoad parser = new ParserTCPPayLoad();

            Console.WriteLine(DateTime.Now - strtTime);
            var IECPakets = parser.GetIECPackets(TCPPayLoadList);
            
            Console.WriteLine(DateTime.Now- strtTime);
            Console.ReadLine();

        }

        #region Парсинг данных из файла Wireshark
        /// <summary>
        /// Получить направление передачи и peerNumber одного TCP пакета
        /// </summary>
        /// <param name="payLoad">Пакет полученный из Wireshark в формате Base64</param>
        /// <returns>Направление передачи, peerNumber</returns>
        public static int[] GetDirection(string payLoad)
        {
            int[] direction = new int[2];
            if ((!string.IsNullOrEmpty(payLoad)))
            {
                var pattern = @"\d*_\d*";
                var regex = new Regex(pattern, RegexOptions.Compiled);
                var match = regex.Match(payLoad);
                var matchValue = match.Value.Split('_');
                direction[0] = Convert.ToInt32(matchValue[0]);
                direction[1] = Convert.ToInt32(matchValue[1]);
            }
            return direction;
        }

        /// <summary>
        /// Получить даные payLoad одного TCP пакета
        /// </summary>
        /// <param name="payLoad">Пакет полученный из Wireshark в формате Base64</param>
        /// <returns>Полезная нагрузка TCP пакета</returns>
        public static string GetPayLoad(string payLoad)
        {
            if (!string.IsNullOrEmpty(payLoad))
            {
                var pattern = @"# Packet \d*\npeer\d*_\d*:\s!!binary\s*[|]";
                var regex = new Regex(pattern, RegexOptions.Compiled);
                var match = regex.Match(payLoad);
                var load = payLoad.
                    Substring(match.Length, payLoad.Length - match.Length);
                load = load.Replace(" ", "");
                return load;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Получить список объектов TCPPayLoad из файла, сформированного Wireshark в формате Base64
        /// </summary>
        /// <param name="rawData">Данные файла, сформированного Wireshark в формате Base64</param>
        /// <returns>Cписок объектов TCPPayLoad</returns>
        public static List<TCPPayLoad> GetTCPPayLoadList(string rawData)
        {
            var payLoadList = new List<TCPPayLoad>();
            var pattern = @"# Packet \d*\npeer\d*_\d*";
            var regex = new Regex(pattern, RegexOptions.Compiled);
            var matches = regex.Matches(rawData);
            foreach (Match match in matches)
            {
                var firstIndex = match.Index;
                var endIndex = match.NextMatch().Index;
                if (endIndex == 0)
                    endIndex = rawData.Length;
                var payLoadTCP = rawData.
                    Substring(firstIndex, endIndex - firstIndex);
                var payLoad = GetPayLoad(payLoadTCP);
                var direction = GetDirection(payLoadTCP);
                payLoadList.
                    Add(new TCPPayLoad(direction[0], direction[1], payLoad));
            }
            return payLoadList;
        }
        #endregion

    }
}
