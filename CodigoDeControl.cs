using System;

namespace validar_ci_ruc_ecuador
{
    public enum Algoritmo{ Modulo10, Modulo11 }
    public class CodigoDeControl
    {
        private string _Serie;
        private  int _DigitoVerificador;
        private readonly string Vacia = "La Serie esta vacia.";

        public CodigoDeControl()
        {
            this._Serie = "";
        }

        public CodigoDeControl(string serie)
        {
            if (string.IsNullOrWhiteSpace(serie)) throw new Exception(Vacia);
            else this._Serie = serie.Trim();
        }

        public int DigitoVerificador
        {
            get { return _DigitoVerificador; }
        }

        
        public string Serie
        {
            get { return _Serie; }
        }

        private void GenerarDigitoVerificadorM11() 
        {            
            
            int factor=1, suma = 0;
            bool numero = true;
            for (int c = Serie.ToString().Length - 1; c >= 0 && numero; c--) {//El for recorrera la cadena de derecha a izquierda
                if (char.IsDigit(Serie.Substring(c, 1), 0)){
                    if (factor == 7) factor = 2; else factor++; //controlo que el factor obtenga los numeros 2 hasta el 7 de forma secuencial 
                    suma += int.Parse(Serie.Substring(c, 1)) * factor;        
                } else numero = false;
            }
            if (numero){
                int residuo =  (suma % 11);
                _DigitoVerificador = (residuo > 1)? (11 - residuo) : residuo;
            } else throw new Exception($"La serie ({Serie}) no es numérica en su totalidad.");
        }

        private void GenerarDigitoVerificadorM10()
        {            
            int factor = 1, suma = 0,aux;
            bool numero = true;
            for (int c = Serie.Length - 1; c >= 0 && numero; c--) { //El for recorrera la cadena de derecha a izquierda
                if (char.IsDigit(Serie.Substring(c, 1), 0)){
                    if (factor == 2) factor = 1; else factor++; //Aqui se controla que el factor obtenga los numeros 2 y 1 de forma secuencial 
                    aux = int.Parse(Serie.Substring(c, 1)) * factor;
                    if (aux > 9)  suma += (aux / 10) + (aux % 10);
                    else suma += aux;
                } else numero = false;
            }
            if (numero){
                int residuo = (suma % 10);
                _DigitoVerificador = (residuo > 0)? (10 - residuo): residuo ;
            } else throw new Exception($"La serie ({Serie}) no es numérica en su totalidad.");
        }
        public bool ValidarDigitoVerificador(string serie, int codigoverificador, Algoritmo algoritmo)
        {
            if (string.IsNullOrWhiteSpace(serie)) throw new Exception(Vacia);
            else {
                this._Serie = serie;
                if (algoritmo == Algoritmo.Modulo10) GenerarDigitoVerificadorM10();
                else GenerarDigitoVerificadorM11();
                return int.Equals(this.DigitoVerificador, codigoverificador);
            }
        }

        public bool ValidarDigitoVerificador(int codigoverificador, Algoritmo algoritmo)
        {
            if (string.IsNullOrWhiteSpace(_Serie))  throw new Exception(Vacia);
            else {
                if (algoritmo == Algoritmo.Modulo10) 
                    GenerarDigitoVerificadorM10();
                else
                    GenerarDigitoVerificadorM11();
                return int.Equals(this.DigitoVerificador, codigoverificador);
            }
        }

        public void GenerarDigitoVerificador(Algoritmo algoritmo) {
            if (string.IsNullOrWhiteSpace(Serie)) throw new Exception(Vacia);
            else if (algoritmo == Algoritmo.Modulo10) GenerarDigitoVerificadorM10();
                  else GenerarDigitoVerificadorM11();
        }
        public void GenerarDigitoVerificador(string Serie, Algoritmo algoritmo){
            if (string.IsNullOrWhiteSpace(Serie)) throw new Exception(Vacia);
            this._Serie = Serie;
            GenerarDigitoVerificador(algoritmo);
        }
    }
}