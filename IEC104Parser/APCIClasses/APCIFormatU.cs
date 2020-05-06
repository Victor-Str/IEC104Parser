namespace IEC104Parser
{
    /// <summary>
    /// APCI - формат S
    /// </summary>
    public class APCIFormatU : APCIFormat
    {
        /// <summary>
        /// Тип сообщения
        /// </summary>
        public MessageType Message { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public enum MessageType
        {
            TESTFR_ACT,
            TESTFR_CON,
            STOPDT_ACT,
            STOPDT_CON,
            STARTDT_ACT,
            STARTDT_CON
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public APCIFormatU()
        {
            SetMessage();
        }

        /// <summary>
        /// Установить сообщение
        /// </summary>
        private void SetMessage()
        {
            switch (_controlField1)
            {
                case 0x43:
                    Message = MessageType.TESTFR_ACT;
                    break;
                case 0x83:
                    Message = MessageType.TESTFR_CON;
                    break;
                case 0x13:
                    Message = MessageType.STOPDT_ACT;
                    break;
                case 0x23:
                    Message = MessageType.STOPDT_CON;
                    break;
                case 0x07:
                    Message = MessageType.STARTDT_ACT;
                    break;
                case 0x0B:
                    Message= MessageType.STARTDT_CON;
                    break;
            }
        }
    }
}
