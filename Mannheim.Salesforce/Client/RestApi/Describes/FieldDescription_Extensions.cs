using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public static class FieldDescriptionExtensions
    {
        public static string GetCorrectedLabel(this FieldDescription fieldDescription)
        {
            return !string.IsNullOrWhiteSpace(fieldDescription.Label) ? fieldDescription.Label : "[" + fieldDescription.Name + "]";
        }

        public static string GetCategory(this FieldDescription fieldDescription)
        {
            return fieldDescription.Custom ? "Custom" : "Standard";
        }

        public static string GetTypeDescription(this FieldDescription fieldDescription)
        {
            var description = fieldDescription.Type.Substring(0, 1).ToUpper() + fieldDescription.Type.Substring(1);
            switch (fieldDescription.Type)
            {
                case "phone":
                case "email":
                case "string":
                case "encryptedstring":
                case "url":
                case "textarea":
                    description += " " + fieldDescription.Length;
                    break;
                case "picklist":
                case "multipicklist":
                    description = (fieldDescription.RestrictedPicklist ? "Restricted " : "")
                        + char.ToUpperInvariant(fieldDescription.Type[0]) + fieldDescription.Type.Substring(1)
                        + (fieldDescription.DependentPicklist ? " dependent on " + fieldDescription.ControllerName : "");
                    break;
                default:
                    break;
            }

            return description;
        }

        public static HashSet<string> SystemFieldNames = new HashSet<string>{ "Id", "LastModifiedById", "CreatedById", "LastReferencedById",
            "IsDeleted", "CurrencyIsoCode","SystemModstamp","LastModifiedDate", "CreatedDate",
            "OwnerId", "RecordTypeId" };

        public static bool GetIsSystemField(this FieldDescription fieldDescription)
        {
            return SystemFieldNames.Contains(fieldDescription.Name);
        }

        public static bool GetIsRequired(this FieldDescription fieldDescription)
        {
            return !fieldDescription.Nillable && !fieldDescription.AutoNumber && !fieldDescription.Type.Equals("boolean", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
