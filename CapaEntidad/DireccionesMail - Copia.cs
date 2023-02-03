//using System.Data;

//namespace CapaEntidad
//{
//    public class DireccionesMail
//    {
//        private int MiIdDireccion;
//        private string MiNombre;
//        private string MiDireccion;
//        private bool MiExiste;



//        public DireccionesMail()
//        {
//            this.MiIdDireccion = 0;
//            this.MiExiste = false;
//            this.LimpiarCampos();
//        }

//        public DireccionesMail(int Id)

//        {
//            this.MiIdDireccion = Id;
//            //this.LlenarCampos();
//        }

//        public DireccionesMail(DataRow Row)
//        {
//            this.MiExiste = true;
//            this.LimpiarCampos();
//            //this.RowAAtributos(Row);
//        }

//        public int IdDireccion

//        {
//            get { return MiIdDireccion; }
//            set { MiIdDireccion = value; }
//        }

//        public string Nombre
//        {
//            get { return MiNombre; }
//            set { MiNombre = value; }
//        }


//        public string Direccion

//        {
//            get { return MiDireccion; }
//            set { MiDireccion = value; }
//        }



//        //public string DireccionYNombre
//        //{
//        //    get
//        //    {
//        //        return this.Direccion + IIf(this.Nombre != "", " (" + this.Nombre + ")", "");
//        //    }
//        //}

//        public bool Existe
//        {
//            get
//            {
//                return this.MiExiste;
//            }
//        }



//        private void LimpiarCampos()
//        {
//            this.MiIdDireccion = 0;
//            this.MiNombre = "";
//            this.MiDireccion = "";
//        }
//    }
//}