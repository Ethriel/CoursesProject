using Microsoft.AspNetCore.Mvc;
using ServicesAPI.Responses;
using System.Collections.Generic;

namespace ServerAPI.Extensions
{
    public static class ControllerExtension
    {
        /// <summary>
        /// An extension for ControllerBase. Checks <paramref name="data"/> and return Ok or BadRequest result
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IActionResult GetCorrespondingResponse(this ControllerBase controller, object data)
        {
            var isValid = data != null && !(data is IEnumerable<string>);
            if (!isValid)
            {
                var error = new ErrorObject("Invalid sign in attempt", data as IEnumerable<string>);
                return controller.BadRequest(error);
            }
            else
            {
                return controller.Ok(data);
            }
        }
        public static IEnumerable<string> GetErrorsFromModelState(this ControllerBase controller)
        {
            var errors = new List<string>();
            foreach (var state in controller.ModelState.Values)
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
