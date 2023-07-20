using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Fargoline.Models;


namespace Fargoline.Controllers
{
    public class ContenedorController : Controller
    {
        // GET: Contenedor
        //Recuperamos la cadena de conexión del Web.config
private string Cadena = ConfigurationManager.ConnectionStrings["Cadena"].ConnectionString;
        public List<Contenedor> Listar()
        {
            string mensaje = string.Empty;
            List<Contenedor> listaContenedor = new List<Contenedor>();
            try { 
            SqlConnection cnn = new SqlConnection(Cadena);
            SqlCommand cmd = new SqlCommand("usp_Contenedor_Listar", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Contenedor oContenedor = new Contenedor()
                {

                    NumeroContenedor = dr.GetInt32(0),
                    TipoContenedor = dr.GetString(1),
                    TamañoContenedor = dr.GetInt32(2),
                    PesoContenedor = dr.GetDecimal(3),
                    TaraContenedor = dr.GetDecimal(4)
                };

                listaContenedor.Add(oContenedor);
            }
            cnn.Close();
            return listaContenedor;
        }
            catch (Exception ex)
            {
                mensaje = "Ocurrió un error en la operación. " + ex.Message;
                return null;
            }
        }

        public int GenerarNumeroContenedor()
        {
            int NumeroContenedor = 0;
            try
            {
                SqlConnection cnn = new SqlConnection(Cadena);
                SqlCommand cmd = new SqlCommand("usp_Generar_NumeroContenedor", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cnn.Open();
                NumeroContenedor = Convert.ToInt32(cmd.ExecuteScalar());
                cnn.Close();
            }
            catch (Exception ex)
            {
                NumeroContenedor = -1;
            }

            return NumeroContenedor;
        }


        public bool Insertar(Contenedor oContenedor)
        {
            SqlConnection cnn = null;
            try
            {
                cnn = new SqlConnection(Cadena);
                SqlCommand cmd = new SqlCommand("usp_Insertar_Contenedor", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TipoContenedor", oContenedor.TipoContenedor);
                cmd.Parameters.AddWithValue("@TamañoContenedor", oContenedor.TamañoContenedor);
                cmd.Parameters.AddWithValue("@PesoContenedor", oContenedor.PesoContenedor);
                cmd.Parameters.AddWithValue("@TaraContenedor", oContenedor.TaraContenedor);

                cnn.Open();
                cmd.ExecuteNonQuery(); // Ejecutar la inserción en la base de datos
                cnn.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (cnn != null)
                    cnn.Close();
            }
        }


        public bool Actualizar(Contenedor oContenedor)
        {
            bool Rpta = false;
            SqlConnection cnn = null;
            try
            {
                cnn = new SqlConnection(Cadena);
                SqlCommand cmd = new SqlCommand("usp_Contenedor_Actualizar", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NumeroContenedor", oContenedor.NumeroContenedor);
                cmd.Parameters.AddWithValue("@TipoContenedor", oContenedor.TipoContenedor);
                cmd.Parameters.AddWithValue("@TamañoContenedor", oContenedor.TamañoContenedor);
                cmd.Parameters.AddWithValue("@PesoContenedor", oContenedor.PesoContenedor);
                cmd.Parameters.AddWithValue("@TaraContenedor", oContenedor.TaraContenedor);
                cnn.Open();
                cmd.ExecuteNonQuery();
                Rpta = true;
            }
            catch (Exception)
            {
                Rpta = false;
            }
            finally
            {
                cnn.Close();
            }
            return Rpta;
        }
     
        public bool Eliminar(int NumeroContenedor)
        {
            bool Rpta = false;
            SqlConnection cnn = null;
            try
            {
                cnn = new SqlConnection(Cadena);
                SqlCommand cmd = new SqlCommand("usp_Contenedor_Eliminar", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NumeroContenedor", NumeroContenedor);
                cnn.Open();
                cmd.ExecuteNonQuery();
                Rpta = true;
            }
            catch (Exception)
            {
                Rpta = false;
            }
            finally
            {
                cnn.Close();
            }
            return Rpta;
        }
        public Contenedor Seleccionar(int NumeroContenedor)
        {
            Contenedor oContenedor = new Contenedor();

            SqlConnection cnn = new SqlConnection(Cadena);
            SqlCommand cmd = new SqlCommand("usp_Contenedor_Seleccionar", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumeroContenedor", NumeroContenedor);
            cnn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                oContenedor.NumeroContenedor = Convert.ToInt32(dr["NumeroContenedor"]);
                oContenedor.TipoContenedor = dr["TipoContenedor"].ToString();
                oContenedor.TamañoContenedor = Convert.ToInt32(dr["TamañoContenedor"]);
                oContenedor.PesoContenedor = Convert.ToDecimal(dr["PesoContenedor"]);
                oContenedor.TaraContenedor = Convert.ToDecimal(dr["TaraContenedor"]);
            }
            cnn.Close();

            return oContenedor;
        }

        public ActionResult Index()
        {
   
            return View(Listar());
        }

        public ActionResult Create()
        {
            Contenedor oContenedor = new Contenedor();
            oContenedor.NumeroContenedor = GenerarNumeroContenedor();
            return View(oContenedor);
        }

        [HttpPost]
        public ActionResult Create(Contenedor oContenedor)
        {
            if (Insertar(oContenedor))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(oContenedor);
            }
        }

        public ActionResult Edit(int NumeroContenedor)
        {
            return View(Seleccionar(NumeroContenedor));
        }


        [HttpPost]
        public ActionResult Edit(Contenedor oContenedor)
        {
            if (Actualizar(oContenedor))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(oContenedor);
            }
        }

        public ActionResult Details(int NumeroContenedor)
        {
            return View(Seleccionar(NumeroContenedor));
        }

        public ActionResult Delete(int NumeroContenedor)
        {
           
                return View(Seleccionar(NumeroContenedor));
            }
        

        [HttpPost]
        public ActionResult Delete(Contenedor oContenedor)
        {
            if (Eliminar(oContenedor.NumeroContenedor))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(oContenedor);
            }
        }
    }
}