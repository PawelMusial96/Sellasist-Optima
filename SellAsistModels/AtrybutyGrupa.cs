namespace Sellasist_Optima.SellAsistModels
{
    public class AtrybutyGrupa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AtrybutyGrupy> Attributes { get; set; }
    }

    public class AtrybutyGrupy
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
