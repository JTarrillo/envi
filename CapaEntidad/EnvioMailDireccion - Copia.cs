//using System.Collections.Generic;
//using System.Data;

//namespace CapaEntidad
//{

//    public class EnvioMailDireccion
//    {
//        private int MiIdEnvioMailDireccion;
//        private int MiIdMail;
//        private int MiIdDireccion;
//        private int MiTipo;
//        private bool MiExiste;
//        private EnvioMail MiEnvioMail;
//        private DireccionesMail MiDireccionMail;




//        public EnvioMailDireccion()
//        {
//            this.MiIdEnvioMailDireccion = 0;
//            this.MiExiste = false;
//            this.LimpiarCampos();
//        }

//        public EnvioMailDireccion(int Id)
//        {
//            this.MiIdEnvioMailDireccion = Id;
//            this.LlenarCampos();
//        }


//        public EnvioMailDireccion(DataRow Row)
//        {
//            this.MiExiste = true;
//            this.LimpiarCampos();
//            this.RowAAtributos(Row);
//        }


//        public int Id

//        {
//            get { return MiIdEnvioMailDireccion; }
//            set { MiIdEnvioMailDireccion = value; }
//        }
      

//        public int IdMail

//        {
//            get { return MiIdMail; }
//            set { MiIdMail = value; }
//        }
       

//        public int IdDireccion
//        {
//            get { return MiIdDireccion; }
//            set { MiIdDireccion = value; }
//        }
     
//        public int Tipo

//        {
//            get { return MiTipo; }
//            set { MiTipo = value; }
//        }
       

//        public bool Existe
//        {
//            get
//            {
//                return this.MiExiste;
//            }
//        }


//        public EnvioMail EnvioMail
//        {
//            get
//            {
//                if (this.MiEnvioMail == null)
//                    this.MiEnvioMail = new EnvioMail(this.MiIdMail);
//                else if (this.MiIdMail != this.MiEnvioMail.IdEnvioMail)
//                    this.MiEnvioMail = new EnvioMail(this.MiIdMail);
//                return this.MiEnvioMail;
//            }
//        }

//        public DireccionesMail DireccionMail
//        {
//            get
//            {
//                if (this.MiDireccionMail == null)
//                    this.MiDireccionMail = new DireccionesMail(this.MiIdDireccion);
//                else if (this.MiIdDireccion != this.MiDireccionMail.IdDireccion)
//                    this.MiDireccionMail = new DireccionesMail(this.MiIdDireccion);
//                return this.MiDireccionMail;
//            }
//        }




//        public void SetearEnvioMail(EnvioMail E)
//        {
//            this.MiEnvioMail = E;
//        }

//        private void LimpiarCampos()
//        {
//            this.MiIdEnvioMailDireccion = 0;
//            this.MiIdMail = 0;
//            this.MiIdDireccion = 0;
//            this.MiTipo = 0;
//        }

//        private void LlenarCampos()
//        {
//            if (this.Id == 0)
//            {
//                this.MiExiste = false;
//                this.LimpiarCampos();
//            }
//            else
//            {
//                string StrSql = "SELECT * FROM EnvioMailDireccion " + "WHERE EnvioMailDireccion.Id = " + System.Convert.ToString(this.Id);
//                DataTable MiDt = new DataTable();

//                using (System.Data.SqlClient.SqlConnection Cn = new System.Data.SqlClient.SqlConnection(EnvioDeNotificaciones.EnvioDeMail.CnStr))
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
//            this.MiIdEnvioMailDireccion = (int)Row.ItemArray[0];
//            this.MiIdMail = (int)Row.ItemArray[1];
//            this.MiIdDireccion = (int)Row.ItemArray[2];
//            this.MiTipo = (int)Row.ItemArray[3];
//        }


//        public static List<EnvioMailDireccion> Obtener(int IdMail)
//        {
//            List<EnvioMailDireccion> Lista = new List<EnvioMailDireccion>();
//            string StrSql = "SELECT EnvioMailDireccion.* FROM EnvioMailDireccion";
//            string StrWhere = "";
//            string StrJoin = "";
//            string StrOrder = "";
//            DataTable MiDt = new DataTable();

//            using (System.Data.SqlClient.SqlConnection Cn = new System.Data.SqlClient.SqlConnection(EnvioDeNotificaciones.EnvioDeMail.CnStr))
//            {
//                System.Data.SqlClient.SqlDataAdapter Da = new System.Data.SqlClient.SqlDataAdapter(StrSql, Cn);

//                StrWhere = StrWhere + "WHERE EnvioMailDireccion.IdMail = " + IdMail.ToString();

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
//                EnvioMailDireccion Objeto = new EnvioMailDireccion(MiDt.Rows[i]);
//                if (Objeto.Existe)
//                    Lista.Add(Objeto);
//                i = i + 1;
//            }
//            MiDt.Dispose();

//            return Lista;
//        }
//    }
//}



