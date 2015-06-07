using System.IO;
using System.Xml.Serialization;

namespace Engine.Utils.ExtensionMethods
{
	/// <summary>
	/// Расширение для сериализации в строку и десериализации из строки
	/// </summary>
	public static class SerializationExtension
	{
		public static T DeserializeObject<T>(this string toDeserialize)
		{
			var xmlSerializer = new XmlSerializer(typeof(T));
			var textReader = new StringReader(toDeserialize);
			return (T)xmlSerializer.Deserialize(textReader);
		}

		public static string SerializeObject<T>(this T toSerialize)
		{
			var xmlSerializer = new XmlSerializer(typeof(T));
			var textWriter = new StringWriter();
			xmlSerializer.Serialize(textWriter, toSerialize);
			return textWriter.ToString();
		}

	}
}
