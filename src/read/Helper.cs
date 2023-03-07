namespace Noken.Assets;

internal static class Helper
{
    public const string Signature = "noken";
    public static readonly Version Version = new(1, 0);

    public static void WriteNullTerminatedString(this BinaryWriter writer, string value)
    {
        writer.Write(value.ToCharArray());
        writer.Write((byte)0);
    }

    public static string ReadNullTerminatedString(this BinaryReader reader)
    {
        var result = string.Empty;

        while (true)
        {
            var buffer = reader.ReadByte();
            if (buffer == 0)
                break;
            result += Convert.ToChar(buffer);
        }

        return result;
    }
}
