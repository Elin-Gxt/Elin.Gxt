using System;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;

namespace Gxt.Net
{
    static class Native
    {
        private const string Lib = "gxt_simple_c.dll";

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern IntPtr gxt_execute(byte[] input, out int result);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void free_rust_cstr(IntPtr ptr);
    }

    internal class FfiException : Exception
    {
        public int ErrorCode { get; set; }
    }

    internal struct MakeKey { }
    internal struct MakeIdCard<T>
    {
        public string key { get; set; }
        public T meta {  get; set; }
    }
    internal struct Verify
    {
        public string msg { get; set; }
    }
    internal struct Encrypt<T>
    {
        public string key { get; set; }
        public string to {  get; set; }
        public T payload { get; set; }
        public string? parent { get; set; }
    }
    internal struct Decrypt
    {
        public string key { get; set; }
        public string msg { get; set; }
    }

    internal static class Ffi
    {
        public static string Execute(MakeKey cmd) {
            return _execute(JsonConvert.SerializeObject(cmd));
        }
        public static string Execute<T>(MakeIdCard<T> cmd) {
            return _execute(JsonConvert.SerializeObject(cmd));
        }
        public static string Execute(Verify cmd) {
            return _execute(JsonConvert.SerializeObject(cmd));
        }
        public static string Execute<T>(Encrypt<T> cmd) {
            return _execute(JsonConvert.SerializeObject(cmd));
        }
        public static string Execute(Decrypt cmd) {
            return _execute(JsonConvert.SerializeObject(cmd));
        }
        static string _execute(string s)
        {
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
