using System;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using AdGeneric.Ext;
using UnityEngine;
using UnityEngine.Networking;

namespace AdGeneric
{
    public static class AdUtils
    {
        public const string PrivacyPath="AdGeneric/修改隐私";

        public static T Log<T>(this T t)
        {
            Debug.Log(t);
            return t;
        }
        public static T LogError<T>(this T t)
        {
            Debug.LogError(t);
            return t;
        }

        public static bool IsWeekend(this DateTime dateTime)
        {
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    return false;
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static AdDateTime GetShieldTime()
        {
            var t = DateTime.Now;
            switch (t.DayOfWeek)
            {
                
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                    t = t.AddDays(2);
                    break;
                case DayOfWeek.Thursday:
                    t = t.AddDays(1);
                    break;
                case DayOfWeek.Friday:
                    t = t.Hour <= 12 ? t : t.AddDays(4);
                    break;
                case DayOfWeek.Saturday:
                    t = t.AddDays(3);
                    break;
                case DayOfWeek.Sunday:
                    t = t.AddDays(2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new AdDateTime(t.Year, t.Month, t.Day, 19, 0, 0);
        }
        public static bool IsInRangeOf(this TimeSpan time,TimeSpan start,TimeSpan end) => time >= start && time < end;
        
        private const string Url = "https://api.live.bilibili.com/client/v1/Ip/getInfoNew";

        private static readonly string[] Are =
        {
            "5bm/5bee", "5Y6m6Zeo", "5YyX5Lqs", 
            "6KW/5rex5Zyz", "6ZW/5rKZ", "5Y2X5Lqs",
            "5Lic6I6e", "5p2t5bee", "6YeN5bqG", 
            "5oiQ6YO9", "5q2m5rGJ", "5LiK5rW3", 
            /*"6Z2S5bKb"*/
        };

        public static IEnumerator RegionShieldFunc(Action<bool> end)
        {
            var request = UnityWebRequest.Get(Url);
            request.timeout = 3;
            yield return request.SendWebRequest();
            if (!request.isDone) AreaShield();

            var b = Analysis(request.downloadHandler.text);
            end?.Invoke(b);
        }

        private static bool Analysis(string requestData)
        {
            requestData.Log();
            return IsShieldedCity(JsonUtility.FromJson<ResponseRootData>(requestData));
        }
        
        private static bool IsShieldedCity(ResponseRootData data)
        {
            
            var city = data.data.city.Replace( "市","");
            $"current city :{city}".Log();
            var encodeCity = NewBaseCode(city);
            return Are.All(e => !e.Contains(encodeCity));
        }
        private static string NewBaseCode(string cityName)
        {
            byte[] byteStr = System.Text.Encoding.UTF8.GetBytes(cityName);
            return Convert.ToBase64String(byteStr);
        }

        [Serializable]
        public class ResponseRootData
        {
            public int code;
            public ResponseData data;
        }

        [Serializable]
        public class ResponseData
        {
            public string country;
            public string province;
            public string city;
            public string ip;
            public string server_time;
        }
#if UNITY_EDITOR
        public static void AreaShield()
        {
            GameObject.Find(AdTotalManager.Instance.name).SendMessage("AreaReceiver", "HasAd");
        }
#else 
        [DllImport("__Internal")]
        public static extern void AreaShield();
#endif
        
    }
}