using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;


namespace YawnMassage.Api
{
    public class SwaggerCustomHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "X-GroupId",
                In = "header",
                Type = "string",
                Required = false // set to false if this is optional
            });
        }
    }
}
