using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Cobalt.Guidance.Text.Descriptions;

namespace Cobalt.Pipeline.Stage
{
    public class StageDescription : Description<Stage>
    {
        public List<PropertyInfo> RequiredParameters { get; set; } = new List<PropertyInfo>();
        public StageDescription Named(string name)
        {
            Name = name;
            return this;
        }
        
        public StageDescription That(string purpose)
        {
            Purpose = purpose;
            return this;
        }

        public StageDescription Require<T>(params Expression<Func<T>>[] requiredParams)
        {
            foreach (var expression in requiredParams)
            {
                var body = (MemberExpression)expression.Body;
                RequiredParameters.Add((PropertyInfo)body.Member);
            }

            return this;
        }
        
        public new StageDescription Describe() => this;
        
        
    }
}