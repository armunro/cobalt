using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Cobalt.Guidance.Descriptions
{
    public class StageDescription : Description<Stage.Stage>
    {
        public List<PropertyInfo> RequiredParameters { get; } = new List<PropertyInfo>();
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
    }
}