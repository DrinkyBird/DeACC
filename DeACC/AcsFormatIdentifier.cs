using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csnxs.DeACC
{
    enum AcsFormat
    {
        NotAcs,
        Acs95,          // ACS\0
        ZDoomUpper,     // ACSE
        ZDoomLower      // ACSe
    }

    class AcsFormatIdentifier
    {
        // Disallow construction of this class.
        private AcsFormatIdentifier() {}

        public static AcsFormat IdentifyFormat(Stream stream)
        {
            long originalPos = stream.Position;

            stream.Seek(0, SeekOrigin.Begin);

            byte[] sig = new byte[4];
            stream.Read(sig, 0, 4);
            stream.Seek(originalPos, SeekOrigin.Begin);

            if (sig[0] != 'A' || sig[1] != 'C' || sig[2] != 'S')
            {
                return AcsFormat.NotAcs;
            }

            if (sig[3] == 'e')
            {
                return AcsFormat.ZDoomLower;
            }
            else if (sig[3] == 'E')
            {
                return AcsFormat.ZDoomUpper;
            }
            else if (sig[3] == '\0')
            {
                stream.Seek(4, SeekOrigin.Begin);
                BinaryReader reader = new BinaryReader(stream);
                int dirOffset = reader.ReadInt32();

                stream.Seek(dirOffset - 4, SeekOrigin.Begin);
                stream.Read(sig, 0, 4);
                stream.Seek(originalPos, SeekOrigin.Begin);

                if (sig[0] != 'A' || sig[1] != 'C' || sig[2] != 'S')
                {
                    return AcsFormat.Acs95;
                }

                if (sig[3] == 'e')
                {
                    return AcsFormat.ZDoomLower;
                }
                else if (sig[3] == 'E')
                {
                    return AcsFormat.ZDoomUpper;
                }
            }

            return AcsFormat.NotAcs;
        }
    }
}
