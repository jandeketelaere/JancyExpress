namespace JancyExpress.Configuration
{
    public interface IJancyExpressConfigurationExpression
    {
        bool ValidateOnStartup { get; set; }
    }

    public class JancyExpressConfigurationExpression : IJancyExpressConfigurationExpression
    {
        public bool ValidateOnStartup { get; set; }
    }
}