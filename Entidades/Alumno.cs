using System;
using System.Collections.Generic;
using csharp.Entidades;

namespace CoreEscuela.Entidades
{
    public class Alumno: ObjetoEscuelaBase
    {
        public List<Evaluación> Evaluaciones {get; set;} =  new List<Evaluación>();
    }
}