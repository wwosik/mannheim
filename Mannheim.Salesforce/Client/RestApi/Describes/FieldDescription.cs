using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public partial class FieldDescription
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }



        [JsonPropertyName("aggregatable")]
        public bool Aggregatable { get; set; }

        [JsonPropertyName("aiPredictionField")]
        public bool AiPredictionField { get; set; }

        [JsonPropertyName("autoNumber")]
        public bool AutoNumber { get; set; }

        [JsonPropertyName("byteLength")]
        public int ByteLength { get; set; }

        [JsonPropertyName("calculated")]
        public bool Calculated { get; set; }

        [JsonPropertyName("calculatedFormula")]
        public string CalculatedFormula { get; set; }

        [JsonPropertyName("cascadeDelete")]
        public bool CascadeDelete { get; set; }

        [JsonPropertyName("caseSensitive")]
        public bool CaseSensitive { get; set; }

        [JsonPropertyName("compoundFieldName")]
        public string CompoundFieldName { get; set; }

        [JsonPropertyName("controllerName")]
        public string ControllerName { get; set; }

        [JsonPropertyName("createable")]
        public bool Createable { get; set; }

        [JsonPropertyName("custom")]
        public bool Custom { get; set; }

        [JsonPropertyName("defaultValue")]
        public object DefaultValue { get; set; }

        [JsonPropertyName("defaultValueFormula")]
        public string DefaultValueFormula { get; set; }

        [JsonPropertyName("defaultedOnCreate")]
        public bool DefaultedOnCreate { get; set; }

        [JsonPropertyName("dependentPicklist")]
        public bool DependentPicklist { get; set; }

        [JsonPropertyName("deprecatedAndHidden")]
        public bool DeprecatedAndHidden { get; set; }

        [JsonPropertyName("digits")]
        public int Digits { get; set; }

        [JsonPropertyName("displayLocationInDecimal")]
        public bool DisplayLocationInDecimal { get; set; }

        [JsonPropertyName("encrypted")]
        public bool Encrypted { get; set; }

        [JsonPropertyName("externalId")]
        public bool ExternalId { get; set; }

        [JsonPropertyName("extraTypeInfo")]
        public string ExtraTypeInfo { get; set; }

        [JsonPropertyName("filterable")]
        public bool Filterable { get; set; }

        [JsonPropertyName("filteredLookupInfo")]
        public object FilteredLookupInfo { get; set; }

        [JsonPropertyName("formulaTreatNullNumberAsZero")]
        public bool FormulaTreatNullNumberAsZero { get; set; }

        [JsonPropertyName("groupable")]
        public bool Groupable { get; set; }

        [JsonPropertyName("highScaleNumber")]
        public bool HighScaleNumber { get; set; }

        [JsonPropertyName("htmlFormatted")]
        public bool HtmlFormatted { get; set; }

        [JsonPropertyName("idLookup")]
        public bool IdLookup { get; set; }

        [JsonPropertyName("inlineHelpText")]
        public string InlineHelpText { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("mask")]
        public string Mask { get; set; }

        [JsonPropertyName("maskType")]
        public string MaskType { get; set; }

        [JsonPropertyName("nameField")]
        public bool NameField { get; set; }

        [JsonPropertyName("namePointing")]
        public bool NamePointing { get; set; }

        [JsonPropertyName("nillable")]
        public bool Nillable { get; set; }

        [JsonPropertyName("permissionable")]
        public bool Permissionable { get; set; }

        [JsonPropertyName("picklistValues")]
        public List<PicklistValue> PicklistValues { get; set; }

        [JsonPropertyName("polymorphicForeignKey")]
        public bool PolymorphicForeignKey { get; set; }

        [JsonPropertyName("precision")]
        public int Precision { get; set; }

        [JsonPropertyName("queryByDistance")]
        public bool QueryByDistance { get; set; }

        [JsonPropertyName("referenceTargetField")]
        public string ReferenceTargetField { get; set; }

        [JsonPropertyName("referenceTo")]
        public string[] ReferenceTo { get; set; }

        [JsonPropertyName("relationshipName")]
        public string RelationshipName { get; set; }

        [JsonPropertyName("relationshipOrder")]
        public int? RelationshipOrder { get; set; }

        [JsonPropertyName("restrictedDelete")]
        public bool RestrictedDelete { get; set; }

        [JsonPropertyName("restrictedPicklist")]
        public bool RestrictedPicklist { get; set; }

        [JsonPropertyName("scale")]
        public int Scale { get; set; }

        [JsonPropertyName("searchPrefilterable")]
        public bool SearchPrefilterable { get; set; }

        [JsonPropertyName("soapType")]
        public string SoapType { get; set; }

        [JsonPropertyName("sortable")]
        public bool Sortable { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("unique")]
        public bool Unique { get; set; }

        [JsonPropertyName("updateable")]
        public bool Updateable { get; set; }

        [JsonPropertyName("writeRequiresMasterRead")]
        public bool WriteRequiresMasterRead { get; set; }


        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }
    }

    public class PicklistValue
    {
        public bool Active { get; set; }
        public bool DefaultValue { get; set; }
        public string Label { get; set; }
        public string ValidFor { get; set; }
        public string Value { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }
    }
}
