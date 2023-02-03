//using System.Data;

//namespace CapaEntidad
//{
//    public class Categoria
//    {
//        private int MiIdIdentificador;
//        private string MiDescripcion;
//        private string MiProyecto;
//        private string MiClaseYMetodo;

//        private bool MiExiste;

//        public class Filtro : Categoria
//        {
//        }

//        public int IdIdentificador

//        {
//            get { return MiIdIdentificador; }
//            set { MiIdIdentificador = value; }
//        }
        
//        public string Descripcion

//        {
//            get { return MiDescripcion; }
//            set { MiDescripcion = value; }
//        }
       
//        public string Proyecto


//        {
//            get { return MiProyecto; }
//            set { MiProyecto = value; }
//        }
       

//        public string ClaseYMetodo


//        {
//            get { return MiClaseYMetodo; }
//            set { MiClaseYMetodo = value; }
//        }

//        public bool Existe
//        {
//            get
//            {
//                return this.MiExiste;
//            }
//        }


//        public Categoria()
//        {
//            this.MiIdIdentificador = 0;
//            this.MiExiste = false;
//            this.LimpiarCampos();
//        }

//        public Categoria(int Id)
//        {
//            this.MiIdIdentificador = Id;
//            //this.LlenarCampos();
//        }

//        public Categoria(DataRow Row)
//        {
//            this.MiExiste = true;
//            this.LimpiarCampos();
//            //this.RowAAtributos(Row);
//        }

//        private void LimpiarCampos()
//        {
//            this.MiIdIdentificador = 0;
//            this.MiDescripcion = "";
//            this.MiClaseYMetodo = "";
//            this.MiProyecto = "";
//        }
//    }
//}
