using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var propertyName = bindingContext.ModelName;
            var valuesProvider = bindingContext.ValueProvider.GetValue(propertyName);

            if (valuesProvider == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {
                var deserializedObject = JsonConvert.DeserializeObject<T>(valuesProvider.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(deserializedObject);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(propertyName, "Invalid value for List<int> type");
            }

            return Task.CompletedTask;
        }
    }
}
