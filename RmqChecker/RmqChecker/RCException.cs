using System;
using System.Collections.Generic;
using System.Text;

namespace RmqChecker
{
    public class RCException : Exception
    {
        public RCException()
        {
        }

        public RCException(string message)
            : base(message)
        {
        }

        public RCException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }

    public static class ExceptionExtension
    {
        public static string MessageToConsoleString(this Exception ex)
        {
            return MessageToConsoleStringRecoursive(ex);
        }

        private static string MessageToConsoleStringRecoursive(Exception ex, int lpad = 0)
        {
            if (ex == null)
                return string.Empty;

            var tab = new StringBuilder();
            var n = lpad;
            while (0 < n--)
                tab.Append("--- ");

            return tab.ToString() + ex.Message + MessageToConsoleStringRecoursive(ex.InnerException, ++lpad) + Environment.NewLine;
        }
    }
}
