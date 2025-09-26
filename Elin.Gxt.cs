using System.Runtime.Serialization;
using Newtonsoft.Json;

#nullable enable

namespace Elin.Gxt
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
            return Ffi.Execute(new MakeIdCard<T> { Key = key, Meta = meta });
        }

        public static string MakeIdCard(string key, dynamic meta)
        {
            return Ffi.Execute(new MakeIdCardDynamic { Key = key, Meta = meta });
        }

        public static Envelope<T> VerifyMessage<T>(string message)
        {
            return JsonConvert.DeserializeObject<Envelope<T>>(Ffi.Execute(new Verify { Msg = message }));
        }

        public static string EncryptMessage<T>(string key, string id_card, T payload, string? parent = null)
        {
            return Ffi.Execute(new Encrypt<T> { Key = key, To = id_card, Payload = payload, Parent = parent });
        }

        public static string EncryptMessage(string key, string id_card, dynamic payload, string? parent = null)
        {
            return Ffi.Execute(new EncryptDynamic { Key = key, To = id_card, Payload = payload, Parent = parent });
        }

        public static Envelope<T> DecryptMessage<T>(string message, string key)
        {
            return JsonConvert.DeserializeObject<Envelope<T>>(Ffi.Execute(new Decrypt { Msg = message, Key = key }));
        }
    }
}
