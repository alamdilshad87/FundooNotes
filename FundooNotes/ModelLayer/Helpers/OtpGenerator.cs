using System.Security.Cryptography;

namespace ModelLayer.Helpers
{
    public static class OtpGenerator
    {
        public static string Generate()
            => RandomNumberGenerator.GetInt32(100000, 999999).ToString();
    }

}
