// OctoRestApi copyright 2022 Danny Glover.

using System.Security.Cryptography;
using System.Text;
using Integrative.Encryption;

namespace TestConsoleApp.Utils;

public static class SecureDataUtil
{
	private static readonly byte[] Entropy = {100, 25, 31, 213};

	public static string ReadData(string filePath)
	{
		// ensure the file exists
		if (!File.Exists(filePath)) return string.Empty;

		using var fileStream = new FileStream(filePath, FileMode.Open);
		using var binaryReader = new BinaryReader(fileStream);

		// decrypt the data
		var decryptedData = CrossProtect.Unprotect(binaryReader.ReadBytes((int) fileStream.Length), Entropy,
			DataProtectionScope.CurrentUser);

		return Encoding.UTF8.GetString(decryptedData);
	}

	public static bool WriteData(string data, string filePath)
	{
		var bytes = Encoding.UTF8.GetBytes(data);
		var protectedBytes = CrossProtect.Protect(bytes, Entropy,
			DataProtectionScope.CurrentUser);
		using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
		using var binaryWriter = new BinaryWriter(fileStream);

		// write the data
		try
		{
			binaryWriter.Write(protectedBytes);
		}
		catch (Exception exception)
		{
			Console.WriteLine($@"SecureDataUtil.WriteData() exception raised: {exception.Message}");
			return false;
		}

		return true;
	}
}