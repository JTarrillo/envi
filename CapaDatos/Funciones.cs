//using System;
//using System.Data;
//using System.Data.SqlClient;
//namespace CapaDatos
//{
//    public class Funciones
//    {

//        public DataTable ObtenerMensaje(int Id = 0)
//        {
//            DataTable dt = new DataTable();
//            try
//            {
//                string StrSql = "SELECT Mensaje FROM EnvioMail WHERE Id = @Id ";



//                SqlParameter[] Parametros = new SqlParameter[] {
//            new SqlParameter("@Id", SqlDbType.Int) { Value = Id }
//        };

//                dt = SqlHelper.ExecuteDataset(Conexion.CnStr, CommandType.Text, StrSql, Parametros).Tables(0);

//                return dt;
//            }
//            catch (Exception)
//            {
//                dt.Clear();
//                return dt;
//            }
//        }

//    }
//}
