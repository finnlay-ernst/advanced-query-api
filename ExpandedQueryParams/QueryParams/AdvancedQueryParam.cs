
namespace ExpandedQueryParams
{
    public abstract class AdvancedQueryParam
    {
        // Name of the field in our model object this query param is filtering
        public string Name { get; set; }

        public abstract object this[string propertyName] { set; }
        public abstract bool Passes(object subject);
    }
}
