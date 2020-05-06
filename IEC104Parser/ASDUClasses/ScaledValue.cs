using System;

namespace IEC104Parser.ASDUClasses
{
    public class ScaledValue
    {
        private byte[] encodedValue = new byte[2];
    
        public ScaledValue(byte[] msg, int startIndex)
        {
            if (msg.Length < startIndex + 2)
                throw new Exception("Message too small for parsing ScaledValue");
    
            for (int i = 0; i < 2; i++)
                encodedValue[i] = msg[startIndex + i];
        }
    
        public ScaledValue()
        {
        }
    
        public ScaledValue(int value)
        {
            this.Value = value;
        }
    
        public ScaledValue(short value)
        {
            this.ShortValue = value;
        }
    
        public byte[] GetEncodedValue()
        {
            return encodedValue;
        }
    
        public int Value
        {
            get
            {
                int value;
    
                value = encodedValue[0];
                value += (encodedValue[1] * 0x100);
    
                if (value > 32767)
                    value = value - 65536;
                return value;
            }
            set
            {
                if (value > 32767)
                    value = 32767;
                else if (value < -32768)
                    value = -32768;
                short valueToEncode = (short)value;
                encodedValue[0] = (byte)(valueToEncode & 255);
                encodedValue[1] = (byte)(valueToEncode >> 8);
            }
        }
    
        public short ShortValue
        {
            get
            {
                UInt16 uintVal;
    
                uintVal = encodedValue[0];
                uintVal += (UInt16)(encodedValue[1] * 0x100);
    
                return (short)uintVal;
            }
    
            set
            {
                UInt16 uintVal = (UInt16)value;
    
                encodedValue[0] = (byte)(uintVal % 256);
                encodedValue[1] = (byte)(uintVal / 256);
            }
        }
    
        public override string ToString()
        {
            return "" + Value;
        }
    }
}
