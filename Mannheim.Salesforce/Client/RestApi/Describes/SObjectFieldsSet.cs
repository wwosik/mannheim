using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public class SObjectFieldsSet : HashSet<string>
    {
        public SObjectFieldsSet(IEnumerable<string> fields) : base(fields ?? Enumerable.Empty<string>())
        {
        }

        public string GetFieldsListForSelect()
        {
            return string.Join(", ", this.OrderBy(f => f));
        }

        public void ReplaceUserIdsWithFullObject()
        {
            void replace(string baseField)
            {
                if (this.Contains($"{baseField}Id"))
                {
                    this.Remove($"{baseField}Id");
                    this.Add($"{baseField}.Name");
                }
            }

            replace("CreatedBy");
            replace("LastModifiedBy");
            replace("Owner");
        }

        public override string ToString()
        {
            return "SObjectFieldsSet: " + this.GetFieldsListForSelect();
        }
    }
}
