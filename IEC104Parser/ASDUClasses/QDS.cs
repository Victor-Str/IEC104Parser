using System.Collections;

//http://docs.cntd.ru/document/gost-r-mek-60870-5-101-2006
//Состоит из 1 байта

namespace IEC104Parser.ASDUClasses
{
    /// <summary>
    /// Описатель качества
    /// </summary>
    public class QDS
    {
        /// <summary>
        /// Данные
        /// </summary>
        private byte _data;

        /// <summary>
        /// Указатель на переполнение
        /// </summary>
        public OverflowType OV { private set; get; } = OverflowType.NoOverflow;

        /// <summary>
        /// Типы указателей на переполнение
        /// </summary>
        public enum OverflowType
        {
            NoOverflow = 0,
            Overflow = 1
        }

        /// <summary>
        /// Указатель на блокировку
        /// </summary>
        public BlockingType BL { private set; get; } = BlockingType.NotBlocked;

        /// <summary>
        /// Типы указателей на блокировку
        /// </summary>
        public enum BlockingType
        {
            NotBlocked = 0,
            Blocked = 1
        }

        /// <summary>
        /// Указатель на замещение
        /// </summary>
        public SubstitutionType SB { private set; get; } = SubstitutionType.NotSubstituted;

        /// <summary>
        /// Типы указателей на замещение
        /// </summary>
        public enum SubstitutionType
        {
            NotSubstituted = 0,
            Substituted = 1
        }

        /// <summary>
        /// Указатель на актуальность
        /// </summary>
        public RelevanceType NT { private set; get; } = RelevanceType.Topical;

        /// <summary>
        /// Типы указателей на актуальность
        /// </summary>
        public enum RelevanceType
        {
            Topical = 0,
            NotRelevant = 1
        }

        /// <summary>
        /// Указатель на дейтсвительность
        /// </summary>
        public ValidationType IV { private set; get; } = ValidationType.Valid;

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
        /// <param name="data">Байт информации</param>
        public QDS(byte data)
        {
            _data = data;
            SetFields();
        }

        /// <summary>
        /// Установить значение указателей
        /// </summary>
        private void SetFields()
        {
            var pointers = new BitArray(new byte[] { _data });
            if (pointers[7]) OV = OverflowType.Overflow;
            if (pointers[3]) BL = BlockingType.Blocked;
            if (pointers[2]) SB = SubstitutionType.Substituted;
            if (pointers[1]) NT = RelevanceType.NotRelevant;
            if (pointers[0]) IV = ValidationType.Invalid;
        }
    }
}
