using ExpandedQueryParams.ModelBinding;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExpandedQueryParams.SwaggerSetup
{
    public class AddAdvancedQueryParam<T> : IOperationFilter
    {        
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ControllerActionDescriptor descriptor = (ControllerActionDescriptor)context.ApiDescription.ActionDescriptor;

            foreach (ControllerParameterDescriptor parameter in descriptor.Parameters)
            {
                BindingInfo? binderInfo = parameter.BindingInfo;
                if (binderInfo == null || binderInfo.BinderType == null || binderInfo.BinderModelName == null) 
                {
                    return;
                }
            
                // If the parameter is bound by our custom binder
                if (descriptor != null && binderInfo.BinderType.Name.Contains("AdvancedModelBinder")) 
                {             
                    
                                
                    string enabledProperty = binderInfo.BinderModelName;
                    Type enabledPropertyType = typeof(T).GetProperty(enabledProperty).PropertyType;                    
                    
                    operation.Parameters.Remove(operation.Parameters.ToList().Find(parameter => parameter.Name == enabledProperty));
                    
                    // C# switch case can't match types :( (or other non-static cases)
                    switch (enabledPropertyType.Name) 
                    {
                        case nameof(Int32):
                            IntApiParams(enabledProperty).ForEach(param => operation.Parameters.Add(param));                                                      
                            break;
                        case nameof(Decimal):
                            DecimalApiParams(enabledProperty).ForEach(param => operation.Parameters.Add(param));                                                      
                            break;
                        case nameof(String):
                            StringApiParams(enabledProperty).ForEach(param => operation.Parameters.Add(param));                                                      
                            break;
                        default:
                            // Unrecognised query type
                            continue;
                    }                    
                }
            }
        }


        /// <summary>
        /// Creates list of all extension params for an int model property.
        /// </summary>
        /// <param name="baseParamName"></param>
        private static List<OpenApiParameter> IntApiParams(string baseParamName) 
        {
            return new List<OpenApiParameter>() 
            { 
                new OpenApiParameter()
                {
                    Name = baseParamName + ".gt",
                    In = ParameterLocation.Query,
                    Description = $"Exclusive lower bound for {baseParamName}"
                },
                new OpenApiParameter()
                {
                    Name = baseParamName + ".lt",
                    In = ParameterLocation.Query,
                    Description = $"Exclusive upper bound for {baseParamName}"
                },
                new OpenApiParameter()
                {
                    Name = baseParamName + ".gte",
                    In = ParameterLocation.Query,
                    Description = $"Inclusive lower bound for {baseParamName}"
                },
                new OpenApiParameter()
                {
                    Name = baseParamName + ".lte",
                    In = ParameterLocation.Query,
                    Description = $"Inclusive lower bound for {baseParamName}"
                },
                new OpenApiParameter()
                {
                    Name = baseParamName + ".ne",
                    In = ParameterLocation.Query,
                    Description = $"{baseParamName} not equal to"
                },
                new OpenApiParameter()
                {
                    Name = baseParamName + ".eq",
                    In = ParameterLocation.Query,
                    Description = $"{baseParamName} equal to"
                }
            }; 

        }
        private static List<OpenApiParameter> DecimalApiParams(string baseParamName)
        {
            return IntApiParams(baseParamName);
        }

        private static List<OpenApiParameter> StringApiParams(string baseParamName)
        {
            return new List<OpenApiParameter>()
            {
                new OpenApiParameter()
                {
                    Name = baseParamName + ".like",
                    In = ParameterLocation.Query,
                    Description = $"Similar strings (contains, prefix, suffix) for {baseParamName}"
                },
                new OpenApiParameter()
                {
                    Name = baseParamName + ".not",
                    In = ParameterLocation.Query,
                    Description = $"{baseParamName} not equal to"
                },
                new OpenApiParameter()
                {
                    Name = baseParamName + ".is",
                    In = ParameterLocation.Query,
                    Description = $"{baseParamName} equal to"
                }
            };
        }
    }

}
