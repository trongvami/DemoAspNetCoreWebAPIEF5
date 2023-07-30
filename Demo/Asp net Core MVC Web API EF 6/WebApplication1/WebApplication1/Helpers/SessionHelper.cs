using Newtonsoft.Json;

namespace WebApplication1.Helpers
{
    public static class SessionHelper
    {
        public static void setObjectJson(this ISession session,
                                           string key, Object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T getObjectJson<T>(this ISession session,
                                           string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                           JsonConvert.DeserializeObject<T>(value);
        }
    }
}
