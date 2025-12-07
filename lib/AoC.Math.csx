using System.Numerics;
    
BigInteger Factorial(int n)
{
    if (n < 0)
        throw new ArgumentException("Negative numbers do not have a factorial.");
    if (n == 0 || n == 1)
        return BigInteger.One;

    BigInteger result = BigInteger.One;
    for (int i = 2; i <= n; i++)
    {
        result *= i;
    }
    return result;
}