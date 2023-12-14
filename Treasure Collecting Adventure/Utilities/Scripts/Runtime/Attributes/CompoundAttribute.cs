using UnityEngine;

namespace LupinrangerPatranger
{
    public class CompoundAttribute : PropertyAttribute
    {
        public readonly string propertyPath;

        public CompoundAttribute(string propertyPath)
        {
            this.propertyPath = propertyPath;
        }
    }
}