using System;
using System.Runtime.Serialization;

namespace Mannheim.Salesforce.Client
{
    [Serializable]
    internal class SalesforceException : Exception
    {
        public SalesforceException()
        {
        }

        public SalesforceException(string message) : base(message)
        {
        }

        public SalesforceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SalesforceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}