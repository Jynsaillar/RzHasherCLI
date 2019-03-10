using System;
using System.Text;

namespace RzHasherCLI
{
    public static class RappelzHasher
    {
        private static Byte[] Key_1 = new Byte[0x80]
        { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x67, 0x20, 0x00, 0x26, 0x77, 0x2C, 0x6C, 0x4E, 0x58, 0x4F, 0x00, 0x37, 0x2E, 0x25, 0x65, 0x00, 0x38, 0x5F, 0x5D, 0x23, 0x50, 0x31, 0x2D, 0x24, 0x56, 0x5B, 0x00, 0x59, 0x00, 0x5E, 0x00, 0x00,
      0x4B, 0x7D, 0x6A, 0x30, 0x40, 0x47, 0x53, 0x29, 0x41, 0x78, 0x79, 0x36, 0x39, 0x45, 0x46, 0x7B, 0x57, 0x62, 0x3D, 0x52, 0x76, 0x74, 0x68, 0x32, 0x34, 0x4D, 0x28, 0x6B, 0x00, 0x6D, 0x61, 0x2B,
      0x7E, 0x44, 0x27, 0x43, 0x21, 0x4A, 0x49, 0x64, 0x42, 0x55, 0x60, 0x71, 0x66, 0x70, 0x48, 0x51, 0x33, 0x4C, 0x6E, 0x6F, 0x5A, 0x69, 0X72, 0x73, 0x75, 0x3B, 0x7A, 0x63, 0x00, 0x54, 0x35, 0x00 };

        private static Byte[] Key_2 = new Byte[0x55]
        { 0x5E, 0x26, 0x54, 0x5F, 0x4E, 0x73, 0x64, 0x7B, 0x78, 0x6F, 0x35, 0x76, 0x60, 0x72, 0x4F, 0x59, 0x56, 0x2B, 0x2C, 0x69, 0x49, 0x55, 0x23, 0x6B, 0x43, 0x4A, 0x71, 0x38, 0x24, 0x27, 0x7E, 0x4C,
      0x30, 0x50, 0x5D, 0x46, 0x65, 0x42, 0x6E, 0x2D, 0x41, 0x75, 0x28, 0x70, 0x58, 0x48, 0x5A, 0x68, 0x77, 0x44, 0x79, 0x32, 0x7D, 0x61, 0x67, 0x57, 0x47, 0x37, 0x4B, 0x3D, 0x62, 0x51, 0x3B, 0x53,
      0x52, 0x74, 0x29, 0x34, 0x36, 0x6C, 0x40, 0x6A, 0x45, 0x25, 0x39, 0x21, 0x63, 0x31, 0x5B, 0x33, 0x66, 0x6D, 0x4D, 0x7A, 0x00 };

        public static String Encrypt(String name)
        {
            if (name.Length != 0)
            {
                Byte[] buff = ASCIIEncoding.ASCII.GetBytes(name);

                UInt32 cv = 0;
                UInt32 tv = 0;
                Int32 bl = buff.Length;
                UInt32 _esi = 0;
                Int32 _seed = 0;
                Byte _bl = 0;
                Byte a = 0;
                Int32 b = 0;
                Byte[] b1 = new Byte[1];
                Byte[] b2 = new Byte[1];

                for (int i = 0; i < bl; i++)
                {
                    cv = buff[i];
                    cv = cv << 4;
                    tv += cv + buff[i] + 1;
                }

                tv += (UInt32)bl;
                tv &= 0x8000001F;

                if (tv == 0)
                    tv = 32;

                _seed = (Int32)tv;

                while (_esi < bl)
                {
                    _bl = buff[_esi];
                    a = _bl;

                    if (_seed == 0)
                        _seed = 32;

                    if (_seed > 0)
                    {
                        b = _seed;

                        while (b != 0)
                        {
                            _bl = Key_1[_bl];
                            b--;
                        }
                    }

                    buff[_esi] = _bl;
                    _seed = (a + _seed + 16 * a + 1) % 32;
                    _esi++;
                }

                HashExtensions.swapchars(ref buff);

                b1[0] = HashExtensions.get_add_char(buff, Key_2);
                b2[0] = Key_2[tv];

                return HashExtensions.BytesToString(b1) + HashExtensions.BytesToString(buff) + HashExtensions.BytesToString(b2);
            }
            else return "";
        }

        public static String Decrypt(String hash)
        {
            if (hash.Length > 1)
            {
                Byte[] seed = Encoding.ASCII.GetBytes(hash);
                Int32 _seed = HashExtensions.getXorIndex((Byte)seed[seed.Length - 1], Key_2);
                Byte _esi = 0;
                Byte _bl = 0;
                Byte a = 0;
                Int32 b = 0;

                Byte[] buff = Encoding.ASCII.GetBytes(hash.Substring(1, hash.Length - 2));

                HashExtensions.swapchars(ref buff);

                while (_esi < buff.Length)
                {
                    a = buff[_esi];
                    _bl = a;

                    if (_seed > 0)
                    {
                        b = _seed;

                        while (b != 0)
                        {
                            _bl = HashExtensions.getXorIndex(_bl, Key_1);
                            b--;
                        }
                    }

                    buff[_esi] = _bl;
                    a = buff[_esi];
                    _seed = (a + _seed + 16 * a + 1) % 32;

                    if (_seed == 0)
                        _seed = 32;

                    _esi++;
                }

                return Encoding.ASCII.GetString(buff);
            }
            else return "";
        }
    }

    public static class HashExtensions
    {
        public static void swapchars(ref Byte[] pHash)
        {
            if (pHash.Length > 4)
            {
                double strlength;

                strlength = (float)pHash.Length;
                double off = strlength * 0.6600000262260437;
                Byte pb = (Byte)off;
                Byte t = pHash[pb];
                pHash[pb] = pHash[0];
                pHash[0] = t;

                off = strlength * 0.3300000131130219;
                pb = (Byte)off;
                t = pHash[pb];
                pHash[pb] = pHash[1];
                pHash[1] = t;
            }
        }


        public static Byte get_add_char(Byte[] pHash, Byte[] Key)
        {
            UInt32 var1;
            UInt32 var2;

            var1 = 0;
            var2 = 0;

            Byte strlength = Convert.ToByte(pHash.Length);
            if (strlength != 0)
            {
                while (var2 < strlength)
                {
                    var1 += (Byte)pHash[var2]; var2++;
                }
            }

            Byte index = Convert.ToByte(var1 % 84);
            return Key[index];
        }

        public static Byte getXorIndex(Byte val, Byte[] Key)
        {
            for (int i = 0; i < Key.Length; ++i)
            {
                if (Key[i] == val)
                    return (Byte)i;
            }
            return 0xff;
        }

        public static string BytesToString(byte[] b)
        {
            int num = 0;
            for (int i = 0; i < (int)b.Length && b[i] > 0; i++)
            {
                num++;
            }
            byte[] numArray = new byte[num];
            for (int i = 0; i < num && b[i] > 0; i++)
            {
                numArray[i] = b[i];
            }

            return Encoding.ASCII.GetString(numArray);
        }
    }
}
