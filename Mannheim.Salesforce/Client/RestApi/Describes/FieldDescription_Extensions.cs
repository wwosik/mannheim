﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public partial class FieldDescription
    {
        [JsonIgnore]
        public string CorrectedLabel => !string.IsNullOrWhiteSpace(this.Label) ? this.Label : "[" + this.Name + "]";

        [JsonIgnore]
        public string Category => this.Custom ? "Custom" : "Standard";

        [JsonIgnore]

        public string TypeDescription
        {
            get
            {
                var description = this.Type.Substring(0, 1).ToUpper() + this.Type.Substring(1);
                switch (this.Type)
                {
                    case "phone":
                    case "email":
                    case "string":
                    case "encryptedstring":
                    case "url":
                    case "textarea":
                        description += " " + this.Length;
                        break;
                    case "picklist":
                    case "multipicklist":
                        description = (this.RestrictedPicklist ? "Restricted " : "")
                            + this.Type
                            + (this.DependentPicklist ? " Dependent on " + this.ControllerName : "");
                        break;
                    default:
                        break;
                }

                return description;
            }
        }

        public static HashSet<string> SystemFieldNames = new HashSet<string>{ "Id", "LastModifiedById", "CreatedById", "LastReferencedById",
            "IsDeleted", "CurrencyIsoCode","SystemModstamp","LastModifiedDate", "CreatedDate",
            "OwnerId", "RecordTypeId" };

        public bool IsSystemField => SystemFieldNames.Contains(this.Name);

        public bool IsRequired => this.Nillable && !this.AutoNumber && this.Type != "boolean";
    }
}