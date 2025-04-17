
using System.Collections;

namespace C.Stream;

public class A5 : IStreamCipher
{
    class A5LFSR : LFSR
    {
        public BitArray CurArr => curv;
        public bool Next1(bool xorb) => Next(xorb);
        public bool Next2(bool chk, int pos) => curv[pos] == chk ? Next() : Peek();

        public A5LFSR(BitArray alg, BitArray key) : base(alg, key) { }
        public A5LFSR(BitArray alg) : base(alg) { }
    }

    readonly A5LFSR R1;
    readonly A5LFSR R2;
    readonly A5LFSR R3;

    readonly static BitArray AL1 = new(19);
    readonly static BitArray AL2 = new(22);
    readonly static BitArray AL3 = new(23);

    static A5()
    {
        AL1[18] = AL1[17] = AL1[16] = AL1[13] = true;
        AL2[21] = AL2[20] = AL2[16] = AL2[12] = true;
        AL3[22] = AL3[21] = AL3[18] = AL3[17] = true;
    }

    public A5(ulong key, int frame)
    {
        BitArray k = new(BitConverter.GetBytes(key));
        BitArray f = new(BitConverter.GetBytes(frame));
        R1 = new(AL1);
        R2 = new(AL2);
        R3 = new(AL3);

        foreach(bool b in k)
        {
            R1.Next1(b);
            R2.Next1(b);
            R3.Next1(b);
        }

        foreach(bool b in f.Cast<bool>().Take(22))
        {
            R1.Next1(b);
            R2.Next1(b);
            R3.Next1(b);
        }

        R1 = new(AL1, R1.CurArr);
        R2 = new(AL2, R2.CurArr);
        R3 = new(AL3, R3.CurArr);
    }

    public bool Next()
    {
        int C1 = R1.CurArr[9] ? 1 : -1;
        int C2 = R2.CurArr[11] ? 1 : -1;
        int C3 = R3.CurArr[11] ? 1 : -1;
        bool chk = C1 + C2 + C3 > 0;
        return R1.Next2(chk, 9) ^ R2.Next2(chk, 11) ^ R3.Next2(chk, 11);
    }

    public void Reset()
    {
        R1.Reset(); R2.Reset(); R3.Reset();
    }

    public (BitArray r1, BitArray r2, BitArray r3) Current() => (R1.Current, R2.Current, R3.Current);
}