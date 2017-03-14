namespace Tavis.OpenApi.Model
{
    public class VariableExpression
    {
        public static VariableExpression Load(ParseNode node)
        {
            var value = node.GetScalarValue();
            return new VariableExpression(value);
        }

        string expression;
        public VariableExpression(string expression)
        {
            this.expression = expression;
        }

    }
}
