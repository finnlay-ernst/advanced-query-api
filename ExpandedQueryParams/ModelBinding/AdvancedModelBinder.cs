using ExpandedQueryParams.QueryParams;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ExpandedQueryParams.ModelBinding
{
    public class AdvancedModelBinder<T> : IModelBinder
    {
        private static readonly string Separator = ".";        
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var queryParams = bindingContext.HttpContext.Request.Query;
            string modelName = bindingContext.ModelName;
            
            List<object> processedQueryParams = new();

            var modelObjectProperties = typeof(T).GetProperties();
            foreach (PropertyInfo modelObjectProp in modelObjectProperties) 
            {                
                // Our query params will not be case sensitive
                var filteredParams = queryParams.Where(param => param.Key.ToLower().StartsWith((modelObjectProp.Name + Separator).ToLower()));

                AdvancedQueryParam? newQuery = null;
                if (modelObjectProp.PropertyType == typeof(int))
                {
                    newQuery = new IntAdvancedQueryParam(modelObjectProp.Name);
                }
                else if (modelObjectProp.PropertyType == typeof(decimal)) 
                {
                    newQuery = new DecimalAdvancedQueryParam(modelObjectProp.Name);
                }
                else if (modelObjectProp.PropertyType == typeof(string)) 
                {
                    newQuery = new StringAdvancedQueryParam(modelObjectProp.Name);
                }
                                
                if (newQuery != null) 
                {
                    foreach (KeyValuePair<string, StringValues> param in filteredParams) 
                    {
                        var nameOp = param.Key.Split(Separator, 2);
                        if (nameOp.Length != 2) 
                        {
                            // Malformed query string param
                            continue;
                        }
                
                        var operation = nameOp[1];
                        
                        // Multiple values for op not supported, just take first
                        // TODO: fix this code duplication if possible
                        if (modelObjectProp.PropertyType == typeof(int))
                        {
                            int opValue = int.Parse(param.Value.First().ToString());
                            try
                            {
                                newQuery[operation.ToUpper()] = opValue;
                            }
                            catch (Exception e) 
                            {
                                // Invalid operation for the parameter type
                            }                
                        }
                        else if (modelObjectProp.PropertyType == typeof(decimal))
                        {
                            decimal opValue = decimal.Parse(param.Value.First().ToString());
                            try
                            {
                                newQuery[operation.ToUpper()] = opValue;
                            }
                            catch (Exception e) 
                            {
                                // Invalid operation for the parameter type
                            }  
                        }
                        else
                        {
                            string opValue = param.Value.First();
                            try
                            {
                                newQuery[operation.ToUpper()] = opValue;
                            }
                            catch (Exception e) 
                            {
                                // Invalid operation for the parameter type
                            }   
                        }
                    }

                    // Add query we just pulled out of the query string to our return list
                    processedQueryParams.Add(newQuery);               
                }
            }

            bindingContext.Result = ModelBindingResult.Success(processedQueryParams);            
            return Task.CompletedTask;
        }
    }
}
