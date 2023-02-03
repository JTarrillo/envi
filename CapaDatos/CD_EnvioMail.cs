using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_EnvioMail
    {
        public List<EnvioMail> Listar()
        {
            List<EnvioMail> list = new List<EnvioMail>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.CnStr))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT  em.Id,em.Fecha,em.Mensaje,");
                    sb.AppendLine("em.Asunto,em.BodyHTML,em.PrioridadAlta,");
                    sb.AppendLine("em.NotificaFallaEntrega,em.EnvioInmediato,");
                    sb.AppendLine("em.Test, em.FechaEnvio,dm.Direccion[Direccion], ");
                    sb.AppendLine("dm.Nombre[Nombre],adj.NombreArchivo[NombreArchivo],");
                    sb.AppendLine("esm.Motivo[Motivo], ca.Descripcion[Descripcion]");
                    sb.AppendLine("FROM EnvioMail em");
                    sb.AppendLine("INNER JOIN DireccionesMail dm");
                    sb.AppendLine("ON em.IdRemitente = dm.IdDireccion");
                    sb.AppendLine("INNER JOIN AdjuntosMail adj");
                    sb.AppendLine("ON adj.Id = em.Id ");
                    sb.AppendLine("LEFT JOIN Categoria ca");
                    sb.AppendLine("ON ca.Identificador = em.CategoriaMail");
                    sb.AppendLine("INNER JOIN ErrorSendMail esm ");
                    sb.AppendLine("ON em.Id = esm.Id");
                    sb.AppendLine("WHERE esm.Id IS NOT NULL");
                    sb.AppendLine("AND adj.NombreArchivo is not null");


                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(
                                new EnvioMail()
                                {
                                    oAdjuntosMail = new AdjuntosMail() { NombreArchivo = reader["NombreArchivo"].ToString() },
                                    Fecha = Convert.ToDateTime(reader["Fecha"].ToString()),
                                    Mensaje = reader["Mensaje"].ToString(),
                                    Asunto = reader["Asunto"].ToString(),
                                    BodyHTML = Convert.ToBoolean(reader["BodyHTML"]),
                                    PrioridadAlta = Convert.ToBoolean(reader["PrioridadAlta"]),
                                    NotificaFallaEntrega = Convert.ToBoolean(reader["NotificaFallaEntrega"]),
                                    EnvioInmediato = Convert.ToBoolean(reader["EnvioInmediato"]),
                                    Test = Convert.ToBoolean(reader["Test"]),
                                    oCategoria = new Categoria() { Descripcion = reader["Descripcion"].ToString() },
                                    oDireccionesMail = new DireccionesMail() { Nombre = reader["Nombre"].ToString(), Direccion = reader["Direccion"].ToString() },
                                    FechaEnvio = Convert.ToDateTime(reader["FechaEnvio"].ToString()),
                                    oErrorSendMail = new ErrorSendMail() { Motivo = reader["Motivo"].ToString() },

                                });

                        }
                    }
                }

            }

            catch
            {

                list = new List<EnvioMail>();
            }

            return list;
        }
    
    public bool VerMensaje(int Id)
    {
        bool resultado = false;
      
        try
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.CnStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT Mensaje FROM EnvioMail WHERE Id = @Id", oconexion);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();
                resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
            }
        }
        catch (Exception ex)
        {
            resultado = false;
         
        }
        return resultado;
    }

}
}
