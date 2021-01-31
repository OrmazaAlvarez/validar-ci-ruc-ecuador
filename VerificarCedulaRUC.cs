using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections;
using System.Text;
using System;

namespace validar_ci_ruc_ecuador
{
    public enum TipoRUC{
        
            JURIDICO = 1,
            NATURAL = 2,
            PUBLICO = 3
        }
    public class VerificarCedulaRUC
    {
        public string Estado { get; set; }
        public TipoRUC TipoRUC { get; set; }
        
        public bool ValidarCedula(string Cedula){
            if (string.IsNullOrWhiteSpace(Cedula)) throw new Exception("La Cédula esta Vacia");
            else
            {
                if(Cedula.Length == 10) {
                    long s;
                    if (long.TryParse(Cedula, out s)){
                        if (ValidarProvincia(int.Parse(Cedula.Substring(0,2)))){
                            if (ValidarTercerDigito(int.Parse(Cedula.Substring(2,1)))){
                                var Generador = new CodigoDeControl(Cedula.Substring(0,9));
                                if (Generador.ValidarDigitoVerificador(int.Parse(Cedula.Substring(9)),Algoritmo.Modulo10)) {
                                    Estado = $"Cédula {Cedula} Correcta";
                                    return true;
                                } else {Estado = $"Útimo dígito de la Cédula {Cedula} Incorrecto";}
                            }else {Estado = $"Cédula {Cedula} Incorrecta";}
                        }else {Estado = $"El código de Provincia en la Cédula {Cedula} no es correcto";}
                    } else {throw new Exception($"La Cédula ({Cedula}) no es numérica en su totalidad.");}
                }else {Estado = $"La Cédula {Cedula} debe contener 10 digitos";}
                return false;
            }
        }

        private bool ValidarProvincia(int codigo){
            return (codigo > 0 && codigo < 25);
        }

        private bool ValidarTercerDigito(int tercerDigito){
            if( tercerDigito < 0 || tercerDigito>6 && tercerDigito != 9) return false;
            else{
                if (tercerDigito < 6) this.TipoRUC = TipoRUC.NATURAL;
                else if (tercerDigito == 6) this.TipoRUC = TipoRUC.PUBLICO;
                else if (tercerDigito == 9) this.TipoRUC = TipoRUC.JURIDICO;
                return true;
            }
        }

        private bool ValidarEstablecimiento(int Establecimiento){
            if (Establecimiento < 1){
                Estado = "El establecimiento debe esta entre 1 y ";
                Estado += (this.TipoRUC == TipoRUC.PUBLICO)? "9999" : "999";
                return false;
            }
            return true;
        }

        public bool ValidarRUC(string RUC){
            if (string.IsNullOrWhiteSpace(RUC)) throw new Exception("El RUC esta Vacio");
            else {
                long s;
                if (long.TryParse(RUC, out s))
                    if(RUC.Length == 13)
                        if (ValidarProvincia(int.Parse(RUC.Substring(0,2)))) 
                            if (ValidarTercerDigito(int.Parse(RUC.Substring(2,1)))){
                                int index = (this.TipoRUC == TipoRUC.PUBLICO)? 8 : 9;
                                if (ValidarEstablecimiento(int.Parse(RUC.Substring(index + 1)))){
                                    var Generador = new CodigoDeControl();
                                    if(Generador.ValidarDigitoVerificador(RUC.Substring(0,index), int.Parse(RUC.Substring(index,1)), (this.TipoRUC == TipoRUC.NATURAL? Algoritmo.Modulo10 : Algoritmo.Modulo11))){
                                        Estado = $"RUC {RUC} Válido";
                                        return true;
                                    } else Estado = $"El Dígito de control en el RUC {RUC} no corresponde {Generador.DigitoVerificador}";
                                }
                            }
                            else Estado = $"RUC {RUC} Inválido";
                        else Estado = $"El código de Provincia en el RUC {RUC} no es correcto";
                    else Estado = $"El RUC {RUC} debe contener 13 digitos";
                else throw new Exception($"El RUC ({RUC}) no es numérico en su totalidad.");
            }         
            return false;
        }
    }
}
