using System;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;

#nullable enable

namespace Elin.Gxt
{
    static class Native
    {
        private const string Lib = "gxt_simple_c.dll";

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern IntPtr gxt_execute(byte[] input, out int result);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void free_rust_cstr(IntPtr ptr);
    }

    public class FfiException : Exception
    {
        public int ErrorCode { get; set; }
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    internal class MakeKey
    {
        [JsonProperty(PropertyName = "action")]
        public string Action { get; } = "MakeKey";
    }
    internal class MakeIdCard<T>
    {
        [JsonProperty(PropertyName = "action")]
        public string Action { get; } = "MakeIdCard";
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
        [JsonProperty(PropertyName = "meta")]
        public T Meta { get; set; }
    }
    internal class MakeIdCardDynamic
    {
        [JsonProperty(PropertyName = "action")]
        public string Action { get; } = "MakeIdCard";
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
        [JsonProperty(PropertyName = "meta")]
        public dynamic Meta { get; set; }
    }
    internal class Verify
    {
        [JsonProperty(PropertyName = "action")]
        public string Action { get; private set; } = "Verify";
        [JsonProperty(PropertyName = "msg")]
        public string Msg { get; set; }
    }
    internal class Encrypt<T>
    {
        [JsonProperty(PropertyName = "action")]
        public string Action { get; private set; } = "Encrypt";
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }
        [JsonProperty(PropertyName = "payload")]
        public T Payload { get; set; }
        [JsonProperty(PropertyName = "parent")]
        public string? Parent { get; set; }
    }
    internal class EncryptDynamic
    {
        [JsonProperty(PropertyName = "action")]
        public string Action { get; private set; } = "Encrypt";
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }
        [JsonProperty(PropertyName = "payload")]
        public dynamic Payload { get; set; }
        [JsonProperty(PropertyName = "parent")]
        public string? Parent { get; set; }
    }
    internal class Decrypt
    {
        [JsonProperty(PropertyName = "action")]
        public string Action { get; private set; } = "Decrypt";
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
        [JsonProperty(PropertyName = "msg")]
        public string Msg { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    internal static class Ffi
    {
        public static string Execute(MakeKey cmd)
        {
            return ExecuteInternal(JsonConvert.SerializeObject(cmd));
        }
        public static string Execute<T>(MakeIdCard<T> cmd)
        {
            return ExecuteInternal(JsonConvert.SerializeObject(cmd));
        }
        public static string Execute(MakeIdCardDynamic cmd)
        {
            return ExecuteInternal(JsonConvert.SerializeObject(cmd));
        }
        public static string Execute(Verify cmd)
        {
            return ExecuteInternal(JsonConvert.SerializeObject(cmd));
        }
        public static string Execute<T>(Encrypt<T> cmd)
        {
            return ExecuteInternal(JsonConvert.SerializeObject(cmd));
        }
        public static string Execute(EncryptDynamic cmd)
        {
            return ExecuteInternal(JsonConvert.SerializeObject(cmd));
        }
        public static string Execute(Decrypt cmd)
        {
            return ExecuteInternal(JsonConvert.SerializeObject(cmd));
        }
        static string ExecuteInternal(string s)
        {
#if DEBUG
            UnityEngine.Debug.Log($"GXT::EXECUTE: {s}");
#endif

            byte[] utf8 = Encoding.UTF8.GetBytes(s ?? string.Empty);
            var buf = new byte[utf8.Length + 1];
            Buffer.BlockCopy(utf8, 0, buf, 0, utf8.Length);

            IntPtr p = Native.gxt_execute(buf, out int errorCode);

            if (errorCode != 0)
            {
                throw new FfiException { ErrorCode = errorCode };
            }

            if (p == IntPtr.Zero)
                throw new Exception("Something went wrong!");

            try
            {
                int len = StrLen(p);
                byte[] managed = new byte[len];
                Marshal.Copy(p, managed, 0, len);
                return Encoding.UTF8.GetString(managed);
            }
            finally
            {
                Native.free_rust_cstr(p);
            }
        }

        private static int StrLen(IntPtr p)
        {
            int len = 0;
            while (Marshal.ReadByte(p, len) != 0)
                len++;
            return len;
        }
    }
}
