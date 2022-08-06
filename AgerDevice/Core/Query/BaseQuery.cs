namespace AgerDevice.Core.Query
{
    public abstract class BaseQuery
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}