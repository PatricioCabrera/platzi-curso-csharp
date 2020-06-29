using System;
using System.Collections.Generic;
using System.Text;

namespace Etapa1.Entidades
{
    interface ILugar
    {
        string Dirección { get; set; }

        void LimpiarLugar();
    }
}
