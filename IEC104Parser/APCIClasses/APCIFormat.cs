namespace IEC104Parser
{
    /// <summary>
    /// Абстрактный класс - данные APCI
    /// </summary>
    public abstract class APCIFormat
    {
        /// <summary>
        /// Поле управления 1
        /// </summary>
        protected byte _controlField1;

        /// <summary>
        /// Поле управления 2
        /// </summary>
        protected byte _controlField2;

        /// <summary>
        /// Поле управления 3
        /// </summary>
        protected byte _controlField3;

        /// <summary>
        /// Поле управления 4
        /// </summary>
        protected byte _controlField4;
    }
}
