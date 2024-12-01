namespace Sellasist_Optima.WebApiModels
{
    public class DocumentAttributes
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Format { get; set; }
        //public List<string> Values { get; set; }
        public List<ComplexType> Values { get; set; }
    }

    public class ComplexType
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
