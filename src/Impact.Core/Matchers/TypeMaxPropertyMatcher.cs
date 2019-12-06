﻿using Newtonsoft.Json.Linq;

namespace Impact.Core.Matchers
{
    public class TypeMaxPropertyMatcher : TypeCountMatcher, IMatcher
    {
        private readonly long max;

        public TypeMaxPropertyMatcher(string path, long max)
        {
            this.max = max;
            PropertyPath = path;
        }

        public bool Matches(object expected, object actual)
        {
            if (expected.GetType() != actual.GetType())
            {
                return false;
            }

            var count = Count(actual, true);

            return !count.HasValue || count.Value <= max;
        }

        public string PropertyPath { get; }

        public JObject ToPactMatcher()
        {
            return new JObject
            {
                ["match"] = "type",
                ["max"] = max
            };
        }

        public IMatcher Clone(string propertyPath)
        {
            return new TypeMaxPropertyMatcher(propertyPath, max);
        }

        public string FailureMessage(object expected, object actual)
        {
            if (expected.GetType() != actual.GetType())
            {
                return $"Expected type {expected.GetType().Name}, but got {actual.GetType().Name}";
            }

            var count = Count(actual, true);

            if (!count.HasValue)
            {
                return string.Empty;
            }
            

            return $"Expected <= {max} elements, but got {count.Value}";
        }
    }
}