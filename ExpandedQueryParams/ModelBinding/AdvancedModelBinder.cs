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
    public class AdvancedModelBinder : IModelBinder
    {
        private static readonly string Separator = ".";        
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var queryParams = bindingContext.HttpContext.Request.Query;
            string modelName = bindingContext.ModelName;
            Type modelType = bindingContext.ModelType;
                                      
            // Our query params will not be case sensitive
            var filteredParams = queryParams.Where(param => param.Key.ToLower().StartsWith((modelName + Separator).ToLower()));

            AdvancedQueryParam? newQuery = null;
            if (modelType == typeof(IntAdvancedQueryParam))
            {
                newQuery = new IntAdvancedQueryParam(modelName);
            }
            else if (modelType == typeof(DecimalAdvancedQueryParam)) 
            {
                newQuery = new DecimalAdvancedQueryParam(modelName);
            }
            else if (modelType == typeof(StringAdvancedQueryParam)) 
            {
                newQuery = new StringAdvancedQueryParam(modelName);
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
                    if (modelType == typeof(IntAdvancedQueryParam))
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
                    else if (modelType == typeof(DecimalAdvancedQueryParam))
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

                // Return query we just pulled out of the query string             
                bindingContext.Result = ModelBindingResult.Success(newQuery);            
            }

            return Task.CompletedTask;
        }
    }
}
