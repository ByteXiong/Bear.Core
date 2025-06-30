using System;
using System.Security.Cryptography;
using System.Text;

namespace BearPlatform.Common.IdGenerator
{
    public static class StringToUuidConverter
    {
        // 预定义的命名空间 UUID（例如 DNS 命名空间）
        private static readonly Guid NamespaceDns = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");

        /// <summary>
        /// 将字符串转换为 UUID（版本3，基于MD5）
        /// </summary>
        public static Guid GenerateVersion3Uuid(string input, Guid? namespaceId = null)
        {
            Guid namespaceGuid = namespaceId ?? NamespaceDns;
            byte[] namespaceBytes = namespaceGuid.ToByteArray();
            byte[] nameBytes = Encoding.UTF8.GetBytes(input);
            byte[] combinedBytes = new byte[namespaceBytes.Length + nameBytes.Length];

            Buffer.BlockCopy(namespaceBytes, 0, combinedBytes, 0, namespaceBytes.Length);
            Buffer.BlockCopy(nameBytes, 0, combinedBytes, namespaceBytes.Length, nameBytes.Length);

            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(combinedBytes);
                return CreateUuidFromHash(hash, 3); // 版本3
            }
        }

        /// <summary>
        /// 将字符串转换为 UUID（版本5，基于SHA-1）
        /// </summary>
        public static Guid GenerateVersion5Uuid(string input, Guid? namespaceId = null)
        {
            Guid namespaceGuid = namespaceId ?? NamespaceDns;
            byte[] namespaceBytes = namespaceGuid.ToByteArray();
            byte[] nameBytes = Encoding.UTF8.GetBytes(input);
            byte[] combinedBytes = new byte[namespaceBytes.Length + nameBytes.Length];

            Buffer.BlockCopy(namespaceBytes, 0, combinedBytes, 0, namespaceBytes.Length);
            Buffer.BlockCopy(nameBytes, 0, combinedBytes, namespaceBytes.Length, nameBytes.Length);

            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(combinedBytes);
                return CreateUuidFromHash(hash, 5); // 版本5
            }
        }

        /// <summary>
        /// 根据哈希值生成符合规范的 UUID
        /// </summary>
        private static Guid CreateUuidFromHash(byte[] hash, int version)
        {
            byte[] uuidBytes = new byte[16];
            Array.Copy(hash, 0, uuidBytes, 0, 16);

            // 设置 UUID 版本和变体
            uuidBytes[6] = (byte)((uuidBytes[6] & 0x0F) | (version << 4)); // 版本号（高4位）
            uuidBytes[8] = (byte)((uuidBytes[8] & 0x3F) | 0x80);           // 变体为 RFC 4122

            return new Guid(uuidBytes);
        }
    }
}
