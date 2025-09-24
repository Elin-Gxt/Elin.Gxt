using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gxt.Net
{
    public enum PayloadKind
    {
        [EnumMember(Value = "id")]
        Id,
        [EnumMember(Value = "msg")]
        Msg,
    }

    public struct Envelope<T>
    {
        public byte version;
        public string verification_key;
        public string encryption_key;
        public PayloadKind kind;
        public T payload;
        public string? parent;
        public string id;
        public string signature;
    }

    public static class GxtSdk
    {
        public static string MakeKey()
        {
            return Ffi.Execute(new MakeKey());
        }

        public static string MakeIdCard<T>(string key, T meta)
        {
            return Ffi.Execute(new MakeIdCard<T> { key = key, meta = meta });
        }

        public static Envelope<T> VerifyMessage<T>(string message)
        {
            return JsonConvert.DeserializeObject<Envelope<T>>(Ffi.Execute(new Verify { msg = message }));
        }

        public static string EncryptMessage<T>(string key, string id_card, T payload, string? parent = null)
        {
            return Ffi.Execute(new Encrypt<T> { key = key, to = id_card, payload = payload, parent = parent });
        }

        public static Envelope<T> DecryptMessage<T>(string message, string key)
        {
            return JsonConvert.DeserializeObject<Envelope<T>>(Ffi.Execute(new Decrypt { msg = message, key = key }));
        }
    }
}
