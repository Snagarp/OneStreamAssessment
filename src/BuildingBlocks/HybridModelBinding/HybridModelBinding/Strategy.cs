//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using System;
using System.Collections.Generic;
using System.Linq;

namespace HybridModelBinding
{
    public static class Strategy
    {
        public static bool FirstInWins(
            IEnumerable<string> previouslyBoundValueProviderIds,
            IEnumerable<string> allValueProviderIds) => !previouslyBoundValueProviderIds.Any();

        public static bool Passthrough(
            IEnumerable<string> previouslyBoundValueProviderIds,
            IEnumerable<string> allValueProviderIds)
        {
            ArgumentNullException.ThrowIfNull(previouslyBoundValueProviderIds);

            ArgumentNullException.ThrowIfNull(allValueProviderIds);

            return true;
        }
    }
}
