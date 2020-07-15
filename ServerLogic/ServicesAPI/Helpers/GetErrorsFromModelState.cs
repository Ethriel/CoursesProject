using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace ServicesAPI.Helpers
{
    public class GetErrorsFromModelState
    {
        public static IEnumerable<string> GetErrors(IReadOnlyDictionary<string, ModelStateEntry> modelState)
        {
            var errors = new List<string>();
            foreach (var state in modelState.Values)
            {
                foreach (var error in state.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            return errors;
        }
    }
}
