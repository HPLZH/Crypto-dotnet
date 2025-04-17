namespace C.Stream;

public class RC4 : IStreamCipher<byte>
{
    readonly byte[] S = new byte[256];
    readonly byte[] I = new byte[256];

    byte i, j;

    public RC4(ReadOnlySpan<byte> k)
    {
        byte[] K = new byte[256];
        for (int i = 0; i <= byte.MaxValue; i++)
        {
            S[i] = (byte)i;
            K[i] = k[i % k.Length];
        }
        for (int i = 0; i <= byte.MaxValue; i++)
        {
            unchecked
            {
                j = (byte)(j + S[i] + K[i]);
                (S[i], S[j]) = (S[j], S[i]);
            }
        }
        S.CopyTo(I, 0);
        i = j = 0;
    }

    public RC4(RC4 inst)
    {
        inst.I.CopyTo(I, 0);
        inst.S.CopyTo(S, 0);
        i = inst.i;
        j = inst.j;
    }

    public byte Next()
    {
        unchecked
        {
            i++;
            j += S[i];
            (S[i], S[j]) = (S[j], S[i]);
            return (byte)(S[i] + S[j]);
        }
    }

    public void Reset()
    {
        I.CopyTo(S, 0);
        i = j = 0;
    }

    public byte[] Current()
    {
        byte[] ret = new byte[256];
        S.CopyTo(ret, 0);
        return ret;
    }
}
