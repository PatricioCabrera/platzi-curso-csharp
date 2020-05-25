using System;
using System.Collections.Generic;
using csharp.Entidades;

namespace CoreEscuela.Entidades
{
    public class Curso: ObjetoEscuelaBase
    {
        public TiposJornada Jornada { get; set; }
        public List<Asignatura> Asignaturas{ get; set; }
        public List<Alumno> Alumnos{ get; set; }        
    }
}