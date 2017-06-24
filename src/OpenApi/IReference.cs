namespace Tavis.OpenApi.Model
{

    public interface IReference
    {
        string Pointer { get; set; }
    }

    public static class IReferenceExtensions
    {
        public static bool IsReference(this IReference reference)
        {
            return !string.IsNullOrWhiteSpace(reference.Pointer);
        }

        public static void WriteRef(this IReference reference, IParseNodeWriter writer)
        {
            writer.WriteStartMap();
            writer.WriteStringProperty("$ref", reference.Pointer);
            writer.WriteEndMap();
        }

    }
}
