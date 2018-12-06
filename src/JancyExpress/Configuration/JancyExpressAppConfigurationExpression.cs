using System.Collections.Generic;

namespace JancyExpress.Configuration
{
    public interface IJancyExpressAppConfigurationExpression
    {
        /// <summary>
        /// Register global HTTP and API handler middleware
        /// </summary>
        /// <returns></returns>
        IJancyExpressAppUseConfigurationExpression Use();

        /// <summary>
        /// Registers routing for a DELETE request
        /// </summary>
        /// <param name="template">The template of the request.</param>
        IJancyExpressAppVerbConfigurationExpression Delete(string template);

        /// <summary>
        /// Registers routing for a GET request
        /// </summary>
        /// <param name="template">The template of the request.</param>
        IJancyExpressAppVerbConfigurationExpression Get(string template);

        /// <summary>
        /// Registers routing for a POST request
        /// </summary>
        /// <param name="template">The template of the request.</param>
        IJancyExpressAppVerbConfigurationExpression Post(string template);

        /// <summary>
        /// Registers routing for a PUT request
        /// </summary>
        /// <param name="template">The template of the request.</param>
        IJancyExpressAppVerbConfigurationExpression Put(string template);
    }

    public class JancyExpressAppConfigurationExpression : IJancyExpressAppConfigurationExpression
    {
        public JancyExpressAppUseConfigurationExpression UseConfiguration { get; private set; }
        public List<JancyExpressAppVerbConfigurationExpression> VerbConfigurationList { get; set; }

        public JancyExpressAppConfigurationExpression()
        {
            UseConfiguration = new JancyExpressAppUseConfigurationExpression();
            VerbConfigurationList = new List<JancyExpressAppVerbConfigurationExpression>();
        }

        public IJancyExpressAppUseConfigurationExpression Use() => UseConfiguration;

        public IJancyExpressAppVerbConfigurationExpression Delete(string template) => Verb(JancyExpressAppVerb.Delete, template);

        public IJancyExpressAppVerbConfigurationExpression Get(string template) => Verb(JancyExpressAppVerb.Get, template);

        public IJancyExpressAppVerbConfigurationExpression Post(string template) => Verb(JancyExpressAppVerb.Post, template);

        public IJancyExpressAppVerbConfigurationExpression Put(string template) => Verb(JancyExpressAppVerb.Put, template);

        private IJancyExpressAppVerbConfigurationExpression Verb(JancyExpressAppVerb verb, string template)
        {
            var verbConfiguration = new JancyExpressAppVerbConfigurationExpression(verb, template);
            VerbConfigurationList.Add(verbConfiguration);
            return verbConfiguration;
        }
    }
}