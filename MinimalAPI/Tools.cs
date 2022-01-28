public static class Tools
{
    public static string ConvertBase64ToFile(string base64, string saveToPath, HttpContext context = null)
    {
        if (base64 != null && IsBase64String(base64))
        {
            var base64array = Convert.FromBase64String(base64);
            // Assume file type
            // jpg as default
            string fileType = ".jpg";

            switch (base64[0])
            {
                case 'i': // png
                    fileType = ".png";
                    break;
                case 'R': // gif
                    fileType = ".gif";
                    break;
                case 'U': // webp
                    fileType = ".webp";
                    break;
                case '/': // jpeg
                default:
                    break;
            }

            var filePath = saveToPath + @"\" + Guid.NewGuid() + fileType;
            System.IO.File.WriteAllBytes(filePath, base64array);

            var fileName = System.IO.Path.GetFileName(filePath);
            return context == null ? fileName : RequestServerURL(context) + fileName;
        }
        return null;
    }

    public static string RequestServerURL(HttpContext context)
    {
        return context.Request.Scheme + "://" + context.Request.Host.Value + "/";
    }
    public static bool IsBase64String(string base64)
    {
        if (string.IsNullOrEmpty(base64)) return false;
        Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
    }
    public static bool DeleteFile(string filePath, string fileName)
    {
        string _fileName = System.IO.Path.GetFileName(fileName);
        if (System.IO.File.Exists(filePath + _fileName))
        {
            System.IO.File.Delete(filePath + _fileName);
            return true;
        }
        return false;
    }
}