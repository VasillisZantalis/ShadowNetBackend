namespace ShadowNetBackend.Helpers;

public static class FileHelper
{
    private static readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg" };
    private const int MaxFileSize = 15 * 1024 * 1024; // 15 MB

    public static string ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return "File is required";

        if (file.Length > MaxFileSize)
            return "File is too large";

        if (!AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            return "Invalid file type";

        return string.Empty;
    }

    public static byte[] ConvertToBytes(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    public static string ConvertToBase64(byte[] fileBytes)
    {
        return Convert.ToBase64String(fileBytes);
    }

    public static byte[] ConvertFromBase64(string base64String)
    {
        return Convert.FromBase64String(base64String);
    }
}
