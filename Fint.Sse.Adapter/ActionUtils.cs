using System;
using Fint.Event.Model;
using FINT.Model.Utdanning.Elev;

namespace Fint.Sse.Adapter
{
    public class ActionUtils
    {
        public static bool IsValidElevAction(string eventAction)
        {
            if (Enum.TryParse(eventAction, true, out ElevActions action))
            {
                if (Enum.IsDefined(typeof(ElevActions), action))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsValidStatusAction(string eventAction)
        {
            if (Enum.TryParse(eventAction, true, out DefaultActions action))
            {
                if (Enum.IsDefined(typeof(DefaultActions), action))
                {
                    return true;
                }
            }
            return false;
        }
    }
}