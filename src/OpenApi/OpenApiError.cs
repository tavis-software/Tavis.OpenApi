using Tavis.OpenApi.Model;

namespace Tavis.OpenApi
{

    public class OpenApiError
    {
        string pointer;
        string message;

        public OpenApiError(DomainParseException ex)
        {
            this.message = ex.Message;
        }
        public OpenApiError(string pointer, string message)
        {
            this.pointer = pointer;
            this.message = message;
        }

        public override string ToString()
        {
            return this.message;
        }
    }


}
