namespace Veterinaria_API.Common
{
    public class PagedResult<t>
    {
        public List<t> Datos { get; set; }
        public int PaginaActual { get; set; }
        public int TamanioPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
    }
}
