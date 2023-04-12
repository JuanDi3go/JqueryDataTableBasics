using JqueryDataTableBasics.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace JqueryDataTableBasics.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _cadenaSQL;
        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListaEmpleado()
        {
            List<Empleado> lista = new List<Empleado>();

            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                var cmd = new SqlCommand("sp_lista_empleados", conexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Empleado
                        {
                            IdEmpleado = Convert.ToInt32(dr["IdEmpleado"]),
                            Nombre = dr["Nombre"].ToString(),
                            Cargo = dr["Cargo"].ToString(),
                            Oficina = dr["Oficina"].ToString(),
                            Salario = dr["Salario"].ToString(),
                            Telefono = Convert.ToInt32(dr["Telefono"]),
                            FechaIngreso = dr["FechaIngreso"].ToString()
                        });
                    }
                } 
            }
                return Json(new {data= lista});
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}