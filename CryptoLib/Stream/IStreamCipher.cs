using System.Collections;
using System.Numerics;

namespace C.Stream;

public interface IStreamCipher<T>
    where T : IBitwiseOperators<T,T,T>
{
    public T Next();
    public void Reset();

    public IEnumerable<T> AsEnumerable(int maxLength = -1)
    {
        int i = 0;
        while (maxLength == -1 || i < maxLength)
        {
            yield return Next();
            i++;
        }
    }

    public IEnumerable<T> Encrypt(IEnumerable<T> input)
    {
        return input.Xor(this.AsEnumerable());
    }
}

public interface IStreamCipher
{
    public bool Next();
    public void Reset();
    
    public IEnumerable<bool> AsEnumerable(int maxLength = -1)
    {
        int i = 0;
        while(maxLength == -1 || i < maxLength)
        {
            yield return Next();
            i++;
        }
    }

    public IEnumerable<bool> Encrypt(IEnumerable<bool> input)
    {
        return input.Xor(this.AsEnumerable());
    }
}

