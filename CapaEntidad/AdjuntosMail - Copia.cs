//using System.Collections.Generic;
//using System.Data;
//using EnvioDeNotificaciones;

//namespace CapaEntidad
//{
//    public class AdjuntosMail
//    {
//        private int MiIdAdjuntosMail;
//        private int MiIdMail;
//        private string MiNombreArchivo;
//        private bool MiBorrado;
//        private bool MiExiste;
//        private EnvioMail MiEnvioMail;


//        public int IdAdjuntosMail
//        {
//            get { return MiIdAdjuntosMail; }
//            set { MiIdAdjuntosMail = value; }
//        }

//        public int IdMail

//        {
//            get { return MiIdMail; }
//            set { MiIdMail = value; }
//        }


//        public string NombreArchivo


//        {
//            get { return MiNombreArchivo; }
//            set { MiNombreArchivo = value; }
//        }


//        public bool Borrado

//        {
//            get { return MiBorrado; }
//            set { MiBorrado = value; }
//        }


//        public bool Existe
//        {
//            get
//            {
//                return this.MiExiste;
//            }
//        }


//        public AdjuntosMail()
//        {
//            this.MiIdAdjuntosMail = 0;
//            this.MiExiste = false;
//            this.LimpiarCampos();
//        }

//        public AdjuntosMail(int Id)
//        {
//            this.MiIdAdjuntosMail = Id;
//            //this.LlenarCampos();
//        }

//        public AdjuntosMail(DataRow Row)
//        {
//            this.MiExiste = true;
//            this.LimpiarCampos();
//            //this.RowAAtributos(Row);
//        }

//        private void LimpiarCampos()
//        {
//            this.MiIdAdjuntosMail = 0;
//            this.MiIdMail = 0;
//            this.MiNombreArchivo = "";
//            this.MiBorrado = false;
//        }

//        private void LlenarCampos()
//        {
//            if (this.IdAdjuntosMail == 0)
//            {
//                this.MiExiste = false;
//                this.LimpiarCampos();
//            }
//            else
//            {
//                string StrSql = "SELECT * FROM AdjuntosMail " + "WHERE  AdjuntosMail.Id = " + System.Convert.ToString(this.IdAdjuntosMail).Length;
//                DataTable MiDt = new DataTable();

//                using (System.Data.SqlClient.SqlConnection Cn = new System.Data.SqlClient.SqlConnection(EnvioDeMail.CnStr))
//                {
//                    Cn.Open();
//                    System.Data.SqlClient.SqlTransaction Tr;
//                    Tr = Cn.BeginTransaction(IsolationLevel.ReadUncommitted);
//                    System.Data.SqlClient.SqlDataAdapter Da = new System.Data.SqlClient.SqlDataAdapter(StrSql, Cn);
//                    Da.SelectCommand.Transaction = Tr;
//                    Da.SelectCommand.CommandTimeout = EnvioDeNotificaciones.EnvioDeMail.TiempoDeEsperaSQL;

//                    Da.Fill(MiDt);
//                    Tr.Commit();
//                    Tr.Dispose();
//                    Da.Dispose();
//                    Cn.Close();
//                    Cn.Dispose();
//                }

//                this.LimpiarCampos();
//                if (MiDt.Rows.Count == 0)
//                    this.MiExiste = false;
//                else
//                {
//                    this.RowAAtributos(MiDt.Rows[0]);
//                    this.MiExiste = true;
//                }
//                MiDt.Dispose();
//            }
//        }

//        private void RowAAtributos(DataRow Row)
//        {
//            this.MiIdAdjuntosMail = (int)Row.ItemArray[0];
//            this.MiIdMail = (int)Row.ItemArray[1];
//            this.MiNombreArchivo = (string)Row.ItemArray[2];
//            this.MiBorrado = (bool)Row.ItemArray[3];
//        }

//        public static List<AdjuntosMail> Obtener(int IdMail)
//        {
//            List<AdjuntosMail> Lista = new List<AdjuntosMail>();
//            string StrSql = "SELECT AdjuntosMail.* FROM AdjuntosMail";
//            string StrWhere = "";
//            string StrJoin = "";
//            string StrOrder = "";
//            DataTable MiDt = new DataTable();

//            using (System.Data.SqlClient.SqlConnection Cn = new System.Data.SqlClient.SqlConnection(EnvioDeMail.CnStr))
//            {
//                System.Data.SqlClient.SqlDataAdapter Da = new System.Data.SqlClient.SqlDataAdapter(StrSql, Cn);

//                StrWhere = StrWhere + "WHERE AdjuntosMail.IdMail = " + IdMail.ToString();

//                StrSql = StrSql + " " + StrJoin + " " + StrWhere + " " + StrOrder;

//                Da.SelectCommand.CommandText = StrSql;
//                Da.SelectCommand.CommandTimeout = EnvioDeNotificaciones.EnvioDeMail.TiempoDeEsperaSQL;

//                Cn.Open();
//                System.Data.SqlClient.SqlTransaction Tr;
//                Tr = Cn.BeginTransaction(IsolationLevel.ReadUncommitted);
//                Da.SelectCommand.Transaction = Tr;
//                Da.Fill(MiDt);
//                Tr.Commit();
//                Tr.Dispose();
//                Da.Dispose();
//                Cn.Close();
//                Cn.Dispose();
//            }

//            int i = 0;
//            while (i < MiDt.Rows.Count)
//            {
//                AdjuntosMail Objeto = new AdjuntosMail(MiDt.Rows[i]);
//                if (Objeto.Existe)
//                    Lista.Add(Objeto);
//                i = i + 1;
//            }
//            MiDt.Dispose();

//            return Lista;
//        }
//    }
//}

