﻿using System;
using System.Collections.Generic;

namespace JancyExpress
{
    public class JancyExpressConfiguration
    {
        internal string Verb { get; }
        internal string Template { get; }

        internal List<Type> HttpHandlerDecoratorTypes { get; set; }
        internal List<Type> ApiHandlerDecoratorTypes { get; set; }
        internal Type HttpHandlerType { get; set; }
        internal Type ApiHandlerType { get; set; }

        public JancyExpressConfiguration(string verb, string template)
        {
            Verb = verb;
            Template = template;
            HttpHandlerDecoratorTypes = new List<Type>();
            ApiHandlerDecoratorTypes = new List<Type>();
        }
    }
}