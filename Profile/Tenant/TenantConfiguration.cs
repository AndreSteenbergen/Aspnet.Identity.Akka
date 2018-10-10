using System.Collections.Generic;

namespace Profile.Tenant
{
    public class Field
    {
        public string ClaimType { get; set; }
        public string DisplayName { get; set; }
    }

    public class ValueField : Field
    {
        public ValueField()
        {

        }

        public ValueField(Field f)
        {
            ClaimType = f.ClaimType;
            DisplayName = f.DisplayName;
        }

        public string Value { get; set; }
    }

    public class TenantConfiguration
    {
        public string Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Theme { get; set; }
        public string Slogan { get; set; }
        public Field[] RequiredFields { get; set; } = new Field[0];
        public Field[] OptionalFields { get; set; } = new Field[0];
        public Dictionary<string, Dictionary<string,string>> SocialLogins { get; set; }
    }
}
