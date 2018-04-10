using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public sealed class Md5Validator : IHashValidator
    {
        private readonly string _key;

        public Md5Validator()
        {
            _key = Constants.signKey;
        }

        public string GetKeyValuePairString(object data)
        {
            return KeyValueConverter.ToKeyValuePairString(data);
        }

        public string ComputeHashFromString(string raw)
        {
            raw += "&key=" + _key;
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(raw));
                return BitConverter.ToString(bytes).Replace("-", "");
            }
        }
        public string ComputeHashFromObject(object data)
        {
            return ComputeHashFromString(GetKeyValuePairString(data));
        }

        public bool Validate(string sign, string raw)
        {
            var md5Value = ComputeHashFromString(raw);
            return string.Compare(sign, md5Value) == 0;
        }

        public bool ValidateObject(string sign, object data)
        {
            var md5Value = ComputeHashFromObject(data);
            return string.Compare(sign, md5Value) == 0;
        }
    }
}
