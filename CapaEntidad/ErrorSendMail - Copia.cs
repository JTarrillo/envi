//using System;
//using System.Data;

//namespace CapaEntidad
//{

//    public class ErrorSendMail
//    {
//        private int MiIdErrorSendMail;
//        private DateTime MiFecha;
//        private string MiMotivo;
//        private int MiIntentos;
//        private DateTime MiFechaPrimerIntento;
//        private bool MiNoReintento;
//        private bool MiExiste;


//        public int IdErrorSendMail

//        {
//            get { return MiIdErrorSendMail; }
//            set { MiIdErrorSendMail = value; }
//        }

     
//        public DateTime Fecha

//        {
//            get { return MiFecha; }
//            set { MiFecha = value; }
//        }

    
//        public string Motivo

//        {
//            get { return MiMotivo; }
//            set { MiMotivo = value; }
//        }
       

//        public int Intentos

//        {
//            get { return MiIntentos; }
//            set { MiIntentos = value; }
//        }
       
      

//        public DateTime FechaPrimerIntento

//        {
//            get { return MiFechaPrimerIntento; }
//            set { MiFechaPrimerIntento = value; }
//        }
       

//        public bool NoReintento

//        {
//            get { return MiNoReintento; }
//            set { MiNoReintento = value; }
//        }
       
        

//        public bool Existe
//        {
//            get
//            {
//                return this.MiExiste;
//            }
//        }


//        public ErrorSendMail()
//        {
//            this.MiIdErrorSendMail = 0;
//            this.MiExiste = false;
//            this.LimpiarCampos();
//        }

//        public ErrorSendMail(int Id)
//        {
//            this.MiIdErrorSendMail = Id;
//            //this.LlenarCampos();
//        }

//        public ErrorSendMail(DataRow Row)
//        {
//            this.MiExiste = true;
//            this.LimpiarCampos();
//            //this.RowAAtributos(Row);
//        }

//        private void LimpiarCampos()
//        {
//            this.MiIdErrorSendMail = 0;
//            this.MiFecha = default(DateTime);
//            this.MiMotivo = "";
//            this.MiIntentos = 0;
//            this.MiFechaPrimerIntento = default(DateTime);
//            this.NoReintento = false;
//        }
//    }

//}
