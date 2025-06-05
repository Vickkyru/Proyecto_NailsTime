using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS_VR750
{
    public class Encriptador_750VR
    {
        private readonly string clave = "1234567890ABCDEF"; // 16 chars ASCII
        private readonly string iv = "ABCDEF1234567890";   // 16 chars ASCII


        public string HashearSHA256_750VR(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytesTexto = Encoding.UTF8.GetBytes(texto);
                byte[] hash = sha256.ComputeHash(bytesTexto);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public string HashearConSalt_750VR(string texto, string salt)
        {
            return HashearSHA256_750VR(texto + salt);
        }

        public string GenerarSalt_750VR()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes).Substring(0, 24);
        }

        // 🔐 Encriptación reversible AES
        public string EncriptarAES_750VR(string textoPlano)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(clave);
                aes.IV = Encoding.UTF8.GetBytes(iv);

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                        sw.Write(textoPlano);

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string DesencriptarAES_750VR(string textoCifrado)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.ASCII.GetBytes(clave);
                    aes.IV = Encoding.ASCII.GetBytes(iv);

                    using (var decryptor = aes.CreateDecryptor())
                    using (var ms = new MemoryStream(Convert.FromBase64String(textoCifrado)))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cs))
                        return sr.ReadToEnd();
                }
            }
            catch (FormatException ex)
            {
                return $"[ERROR Base64] {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"[ERROR General] {ex.Message}";
            }
        }
    }
}
