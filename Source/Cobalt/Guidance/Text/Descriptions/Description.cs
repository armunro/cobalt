using System;
using System.Reflection;

namespace Cobalt.Guidance.Text.Descriptions
{
    public class Description<T>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public string Author { get; set; } = "Valence Systems";
        
        public Description()
        {
            Id = Guid.NewGuid();
        }
       
        
    }
}