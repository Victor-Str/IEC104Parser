using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEC104Parser
{
    /// <summary>
    /// Буфер для хранения частей пакетов IEC 104
    /// </summary>
    internal class Buffer
    {
        /// <summary>
        /// 
        /// </summary>
        public int Direction { private set; get; }

        /// <summary>
        /// Начало следющего IEC 104 пакета
        /// </summary>
        public byte[] ResidualEndPart { set; get; }

        /// <summary>
        /// Продолжение предыдущего IEC 104 пакета
        /// </summary>
        public byte[] ResidualStartPart { set; get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="direction">Направление передачи</param>
        public Buffer(int direction)
        {
            Direction = direction;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {

        }

    }
}
