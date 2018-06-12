using System;

namespace ArmiesDomain.ValueObjects
{
    public class Tag
    {
        private readonly string value;

        public Tag(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Tag value is null or empty");
            }

            this.value = value;
        }
    }
}
