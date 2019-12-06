﻿using System;
using Impact.Core;
using Impact.Core.Matchers;
using Newtonsoft.Json.Linq;

namespace Impact.Provider
{
    public class Interaction
    {
        private readonly string description;
        private string providerState;
        private readonly object request;
        private readonly object response;
        private readonly IMatcher[] matchers;

        internal Interaction(JObject i, IRequestResponseDeserializer deserializer)
        {
            description = i["description"].ToString();
            providerState = i["providerState"].ToString();
            request = deserializer.DeserializeRequest(i["request"]);
            response = deserializer.DeserializeResponse(i["response"]);

            matchers = i["matchingRules"] is JObject rules ? MatcherParser.Parse(rules): new IMatcher[0];
        }

        public InteractionVerificationResult Honour(Func<object, object> sendRequest)
        {
            var actualResponse = sendRequest(request);
            var checker = new MatchChecker(matchers, false);

            var matches = checker.Matches(response, actualResponse);

            return new InteractionVerificationResult(description, matches.Matches, matches.FailureReasons);

        }
    }

    public class InteractionVerificationResult
    {
        public InteractionVerificationResult(string description, bool success, string failureReason = null)
        {
            Description = description;
            Success = success;
            FailureReason = failureReason;
        }

        public string Description { get; }
        public bool Success { get; }
        public string FailureReason { get; }
    }
}