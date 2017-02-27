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
    }
}
