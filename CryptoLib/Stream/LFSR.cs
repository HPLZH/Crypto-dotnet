using System.Collections;
using System.Diagnostics;

namespace C.Stream;

public class LFSR : IStreamCipher
{
    private readonly BitArray algv;
    protected BitArray curv;
    public BitArray Current => new(curv);

    private readonly BitArray init;

    public LFSR(IEnumerable<bool> alg, IEnumerable<bool> key)
    {
        algv = new(alg.ToArray());
        curv = new(key.Take(algv.Length).ToArray());
        init = new BitArray(curv);
    }

    public LFSR(BitArray alg, IEnumerable<bool> key)
    {
        algv = new(alg);
        curv = new(key.Take(algv.Length).ToArray());
        init = new BitArray(curv);
    }

    public LFSR(BitArray alg, BitArray key)
    {
        algv = new(alg);
        curv = new(key);
        Debug.Assert(algv.Length == curv.Length);
        init = new BitArray(curv);
    }

    public LFSR(BitArray alg, bool init = false)
    {
        algv = new(alg);
        curv = new(algv.Length, init);
        this.init = new BitArray(curv);
    }

    public bool Peek() => curv[^1];
    public bool Next() => Next(false);

    public void Reset()
    {
        curv = new(init);
    }

    protected bool Next(bool xorb)
    {
        bool ret = Peek();
        BitArray crt = new BitArray(algv).And(curv);
        foreach(bool v in crt)
        {
            xorb ^= v;
        }
        curv.LeftShift(1);
        curv[0] = xorb;
        return ret;
    }
}
