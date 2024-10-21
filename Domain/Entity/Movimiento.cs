using Domain.Enumerator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    [Table("Movimientos")]
    public class Movimiento
    {       
        [Key]
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public Movimientos TipoMovimiento { get; set; }
        public double Valor { get; set; }
        public double Saldo { get; set; }
        public bool Estado { get; set; }
        public string NumeroCuenta { get; set; }
        public string CodMovimiento { get; set; }
        public Cuenta Cuentas { get; set; }

        public Movimiento(DateTime? Fecha, Movimientos TipoMovimiento, double Valor, double Saldo, bool Estado, string NumeroCuenta, string CodMovimiento)
        {
            this.Fecha = Fecha;
            this.TipoMovimiento = TipoMovimiento;
            this.Valor = Valor;
            this.Saldo = Saldo;
            this.Estado = Estado;
            this.NumeroCuenta = NumeroCuenta;
            this.CodMovimiento = CodMovimiento;
        }
    }
}
