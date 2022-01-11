using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using WebAPIAutores1.DTOs;

namespace WebAPIAutores1.Services
{
    public class HashService
    {
        public ResultadoHash Hash(string textoPlano)
        {
            var sal = new byte[16];
            using (var random = RandomNumberGenerator.Create()){
                random.GetBytes(sal);
            }
            return Hash(textoPlano, sal);
        }

        public ResultadoHash Hash(string textPlano, byte[] sal)
        {
            var llaveDerivada = KeyDerivation.Pbkdf2(password: textPlano, salt: sal, prf: KeyDerivationPrf.HMACSHA1, iterationCount: 10000, numBytesRequested: 32);
            var hash = Convert.ToBase64String(llaveDerivada);
            return new ResultadoHash()
            {
                Hash = hash,
                Sal = sal
            };
        }
    }
}
