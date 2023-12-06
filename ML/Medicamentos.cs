namespace ML
{
    public class Medicamentos
    {
        public int IdMedicamentos {  get; set; }
        public string? SKU { get; set; }
        public string? Nombre { get; set; }
        public decimal? Precio { get; set;}
        public decimal? PrecioTotal { get; set; }
        public string? Imagen { get; set; }
        public int Cantidad { get; set; }
        public List<ML.Medicamentos>? medicamentos { get; set; }
    }
}