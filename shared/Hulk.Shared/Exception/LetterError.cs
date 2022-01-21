using System.Collections.Generic;

namespace Hulk.Shared.Exception
{
    public class LetterError
    {
        public static Dictionary<int, string> Errors = new Dictionary<int, string>
        {
            // {0} = objectValue
            // {1} = objectPath
            // {2} = extraValue

            {-100, "Không tìm thấy dữ liệu {1} {0}"},
            {-140, "Không thể kết nối với máy chủ email (smtp server not found)."},
        };

        public LetterError(int code, string message = null, string objectPath = null, string objectValue = null, string extraValue = null)
        {
            Code = code;
            Message = message;
            ObjectPath = objectPath;
            ObjectValue = objectValue;

            // If message was not provided, lookup the message using the dictionary
            if (Message == null)
                Message = string.Format(Errors[code], objectValue, objectPath, extraValue);
        }

        public int Code { get; set; }
        public string Message { get; set; }
        public string ObjectPath { get; set; }
        public string ObjectValue { get; set; }
    }
}