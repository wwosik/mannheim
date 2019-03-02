using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public partial class FieldDescription
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }



        [JsonProperty("aggregatable")]
        public bool Aggregatable { get; set; }

        [JsonProperty("aiPredictionField")]
        public bool AiPredictionField { get; set; }

        [JsonProperty("autoNumber")]
        public bool AutoNumber { get; set; }

        [JsonProperty("byteLength")]
        public int ByteLength { get; set; }

        [JsonProperty("calculated")]
        public bool Calculated { get; set; }

        [JsonProperty("calculatedFormula")]
        public string CalculatedFormula { get; set; }

        [JsonProperty("cascadeDelete")]
        public bool CascadeDelete { get; set; }

        [JsonProperty("caseSensitive")]
        public bool CaseSensitive { get; set; }

        [JsonProperty("compoundFieldName")]
        public string CompoundFieldName { get; set; }

        [JsonProperty("controllerName")]
        public string ControllerName { get; set; }

        [JsonProperty("createable")]
        public bool Createable { get; set; }

        [JsonProperty("custom")]
        public bool Custom { get; set; }

        [JsonProperty("defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty("defaultValueFormula")]
        public string DefaultValueFormula { get; set; }

        [JsonProperty("defaultedOnCreate")]
        public bool DefaultedOnCreate { get; set; }

        [JsonProperty("dependentPicklist")]
        public bool DependentPicklist { get; set; }

        [JsonProperty("deprecatedAndHidden")]
        public bool DeprecatedAndHidden { get; set; }

        [JsonProperty("digits")]
        public int Digits { get; set; }

        [JsonProperty("displayLocationInDecimal")]
        public bool DisplayLocationInDecimal { get; set; }

        [JsonProperty("encrypted")]
        public bool Encrypted { get; set; }

        [JsonProperty("externalId")]
        public bool ExternalId { get; set; }

        [JsonProperty("extraTypeInfo")]
        public string ExtraTypeInfo { get; set; }

        [JsonProperty("filterable")]
        public bool Filterable { get; set; }

        [JsonProperty("filteredLookupInfo")]
        public string FilteredLookupInfo { get; set; }

        [JsonProperty("formulaTreatNullNumberAsZero")]
        public bool FormulaTreatNullNumberAsZero { get; set; }

        [JsonProperty("groupable")]
        public bool Groupable { get; set; }

        [JsonProperty("highScaleNumber")]
        public bool HighScaleNumber { get; set; }

        [JsonProperty("htmlFormatted")]
        public bool HtmlFormatted { get; set; }

        [JsonProperty("idLookup")]
        public bool IdLookup { get; set; }

        [JsonProperty("inlineHelpText")]
        public string InlineHelpText { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("mask")]
        public string Mask { get; set; }

        [JsonProperty("maskType")]
        public string MaskType { get; set; }

        [JsonProperty("nameField")]
        public bool NameField { get; set; }

        [JsonProperty("namePointing")]
        public bool NamePointing { get; set; }

        [JsonProperty("nillable")]
        public bool Nillable { get; set; }

        [JsonProperty("permissionable")]
        public bool Permissionable { get; set; }

        [JsonProperty("picklistValues")]
        public object[] PicklistValues { get; set; }

        [JsonProperty("polymorphicForeignKey")]
        public bool PolymorphicForeignKey { get; set; }

        [JsonProperty("precision")]
        public int Precision { get; set; }

        [JsonProperty("queryByDistance")]
        public bool QueryByDistance { get; set; }

        [JsonProperty("referenceTargetField")]
        public string ReferenceTargetField { get; set; }

        [JsonProperty("referenceTo")]
        public string[] ReferenceTo { get; set; }

        [JsonProperty("relationshipName")]
        public string RelationshipName { get; set; }

        [JsonProperty("relationshipOrder")]
        public string RelationshipOrder { get; set; }

        [JsonProperty("restrictedDelete")]
        public bool RestrictedDelete { get; set; }

        [JsonProperty("restrictedPicklist")]
        public bool RestrictedPicklist { get; set; }

        [JsonProperty("scale")]
        public int Scale { get; set; }

        [JsonProperty("searchPrefilterable")]
        public bool SearchPrefilterable { get; set; }

        [JsonProperty("soapType")]
        public string SoapType { get; set; }

        [JsonProperty("sortable")]
        public bool Sortable { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("unique")]
        public bool Unique { get; set; }

        [JsonProperty("updateable")]
        public bool Updateable { get; set; }

        [JsonProperty("writeRequiresMasterRead")]
        public bool WriteRequiresMasterRead { get; set; }


        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
