using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WurmMesi
{
    internal static class Json
    {
        public static string ToString<T>(T model)
        {
            try
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                using (var ms = new MemoryStream())
                {
                    ser.WriteObject(ms, model);
                    var json = Encoding.UTF8.GetString(ms.ToArray());
                    return json;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string ToString(object model, Type type)
        {
            try
            {
                var ser = new DataContractJsonSerializer(type);
                using (var ms = new MemoryStream())
                {
                    ser.WriteObject(ms, model);
                    var json = Encoding.UTF8.GetString(ms.ToArray());
                    return json;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static T Parse<T>(string json)
        {
            try
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                using (var ms = new MemoryStream())
                {
                    var bytes = Encoding.UTF8.GetBytes(json);
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Seek(0, SeekOrigin.Begin);

                    return (T)ser.ReadObject(ms);
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static object Parse(string json, Type type)
        {
            try
            {
                var ser = new DataContractJsonSerializer(type);
                using (var ms = new MemoryStream())
                {
                    var bytes = Encoding.UTF8.GetBytes(json);
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Seek(0, SeekOrigin.Begin);

                    return ser.ReadObject(ms);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Dictionary<string, string> ToDictionary<T>(T model)
        {
            var dic = new Dictionary<string, string>();
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(model);
                if (null == value)
                {
                    var opt = prop.GetCustomAttribute<OptionalAttribute>();
                    var req = prop.GetCustomAttribute<RequiredAttribute>();

                    if (null != opt)
                    {
                        continue;
                    }
                    if (null != req)
                    {
                        throw new ArgumentNullException();
                    }
                }
                dic[prop.Name] = value.ToString();
            }
            return dic;
        }
    }

    public class OptionalAttribute : Attribute
    {
    }

    public class RequiredAttribute : Attribute
    {
    }
}
