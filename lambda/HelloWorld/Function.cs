using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace HelloWorld
{
    public sealed class Function
    {
        public string FunctionHandler(string input, ILambdaContext context)
        {
            return string.IsNullOrWhiteSpace(input) ? string.Empty : string.Join("-", input.ToUpper());
        }
    }
}
