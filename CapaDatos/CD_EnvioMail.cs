using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_EnvioMail
    {
        public List<EnvioMail> Listar()
        {
            List<EnvioMail> list = new List<EnvioMail>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select Id,Fecha,Mensaje,Asunto,BodyHTML,PrioridadAlta,NotificaFallaEntrega,EnvioInmediato,Test,CategoriaMail,IdRemitente,FechaEnvio,ErrorId from EnvioMail";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(
                                new EnvioMail()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Fecha = Convert.ToDateTime(reader["Fecha"].ToString()),
                                    Mensaje = reader["Mensaje"].ToString(),
                                    Asunto = reader["Asunto"].ToString(),
                                    BodyHTML = Convert.ToBoolean(reader["BodyHTML"]),
                                    PrioridadAlta = Convert.ToBoolean(reader["PrioridadAlta"]),
                                    NotificaFallaEntrega = Convert.ToBoolean(reader["NotificaFallaEntrega"]),
                                    EnvioInmediato = Convert.ToBoolean(reader["EnvioInmediato"]),
                                    Test = Convert.ToBoolean(reader["Test"]),
                                    CategoriaMail = Convert.ToInt32(reader["CategoriaMail"]),
                                    IdRemitente = Convert.ToInt32(reader["IdRemitente"]),
                                    FechaEnvio = Convert.ToDateTime(reader["FechaEnvio"].ToString()),
                                    ErrorId = Convert.ToInt32(reader["ErrorId"]),
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
    }
}
