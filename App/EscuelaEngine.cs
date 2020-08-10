using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;
using csharp.Entidades;
using Etapa1.Entidades;

namespace CoreEscuela
{
    public sealed class EscuelaEngine
    {
        public Escuela Escuela { get; set; }

        public EscuelaEngine()
        {

        }

        public void Inicializar()
        {
            Escuela = new Escuela("Platzi Academy", 2012, TiposEscuela.Primaria,
            ciudad: "Bogotá", pais: "Colombia"
            );

            CargarCursos();
            CargarAsignaturas();
            CargarEvaluaciones();
        }

        public Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> GetDiccionarioObjetos()
        {
            Dictionary<LlaveDiccionario, IEnumerable <ObjetoEscuelaBase>> diccionario = new Dictionary<LlaveDiccionario, IEnumerable <ObjetoEscuelaBase>>();

            diccionario.Add(LlaveDiccionario.Escuela, new List<ObjetoEscuelaBase> {Escuela});
            diccionario.Add(LlaveDiccionario.Curso, Escuela.Cursos);

            var listatmp = new List<Evaluación>();
            var listatmpAs = new List<Asignatura>();
            var listatmpAl = new List<Alumno>();

            foreach (var cur in Escuela.Cursos)
            {
                listatmpAs.AddRange(cur.Asignaturas);
                listatmpAl.AddRange(cur.Alumnos);

                foreach (var alum in cur.Alumnos)
                {
                    listatmp.AddRange(alum.Evaluaciones);
                }
            }

            diccionario.Add(LlaveDiccionario.Evaluación, listatmp);
            diccionario.Add(LlaveDiccionario.Asignatura, listatmpAs);
            diccionario.Add(LlaveDiccionario.Alumno, listatmpAl);
            
            return diccionario;
        }

        private List<Alumno> GenerarAlumnosAlAzar(int cantidad)
        {
            string[] nombre1 = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };
            string[] apellido1 = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trump", "Toledo", "Herrera" };
            string[] nombre2 = { "Freddy", "Anabel", "Rick", "Murty", "Silvana", "Diomedes", "Nicomedes", "Teodoro" };

            var listaAlumnos = from n1 in nombre1
                               from n2 in nombre2
                               from a1 in apellido1
                               select new Alumno { Nombre = $"{n1} {n2} {a1}" };

            return listaAlumnos.OrderBy((al) => al.UniqueId).Take(cantidad).ToList();
        }

        #region Obtener objetos de escuela
        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
            out int conteoEvaluaciones,
            out int conteoAlumnos,
            out int conteoAsignaturas,
            out int conteoCursos,
            bool traeEvaluaciones = true, 
            bool traeAlumnos = true, 
            bool traeAsignaturas = true, 
            bool traeCursos = true)
        {
            conteoEvaluaciones = conteoAlumnos = conteoAsignaturas = conteoCursos = 0;

            List<ObjetoEscuelaBase> listaObj = new List<ObjetoEscuelaBase>();
            listaObj.Add(Escuela);

            if (traeCursos)
            {
                listaObj.AddRange(Escuela.Cursos);
                conteoCursos += Escuela.Cursos.Count();
            }

            foreach (var curso in Escuela.Cursos)
            {
                if (traeAsignaturas)
                {
                    listaObj.AddRange(curso.Asignaturas);
                    conteoAsignaturas += curso.Asignaturas.Count();
                }

                if (traeAlumnos)
                {
                    listaObj.AddRange(curso.Alumnos);
                    conteoAlumnos += curso.Alumnos.Count();
                }

                if (traeEvaluaciones)
                {
                    foreach (var alumno in curso.Alumnos)
                    {
                        listaObj.AddRange(alumno.Evaluaciones);
                        conteoEvaluaciones += alumno.Evaluaciones.Count();
                    }
                }
            }


            return listaObj.AsReadOnly();
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
            bool traeEvaluaciones = true,
            bool traeAlumnos = true,
            bool traeAsignaturas = true,
            bool traeCursos = true)
        {
            return GetObjetoEscuela(out _, out _, out _, out _, traeEvaluaciones, traeAlumnos, traeAsignaturas, traeCursos);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
            out int conteoEvaluaciones,
            bool traeEvaluaciones = true,
            bool traeAlumnos = true,
            bool traeAsignaturas = true,
            bool traeCursos = true)
        {
            return GetObjetoEscuela(out conteoEvaluaciones, out _, out _, out _, traeEvaluaciones, traeAlumnos, traeAsignaturas, traeCursos);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
            out int conteoEvaluaciones,
            out int conteoCursos,
            bool traeEvaluaciones = true,
            bool traeAlumnos = true,
            bool traeAsignaturas = true,
            bool traeCursos = true)
        {
            return GetObjetoEscuela(out conteoEvaluaciones, out conteoCursos, out _, out _, traeEvaluaciones, traeAlumnos, traeAsignaturas, traeCursos);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
            out int conteoEvaluaciones,
            out int conteoCursos,
            out int conteoAsignaturas,
            bool traeEvaluaciones = true,
            bool traeAlumnos = true,
            bool traeAsignaturas = true,
            bool traeCursos = true)
        {
            return GetObjetoEscuela(out conteoEvaluaciones, out conteoCursos, out conteoAsignaturas, out _, traeEvaluaciones, traeAlumnos, traeAsignaturas, traeCursos);
        }
        #endregion
        #region Métodos de carga
        private void CargarEvaluaciones()
        {
            foreach (Curso curso in Escuela.Cursos)
            {
                foreach (Asignatura asignatura in curso.Asignaturas)
                {
                    foreach (Alumno alumno in curso.Alumnos)
                    {
                        var rnd = new Random(System.Environment.TickCount);

                        Evaluación[] evArray = new Evaluación[5];

                        for (int i = 0; i < 5; i++)
                        {
                            Evaluación ev = new Evaluación
                            {
                                Asignatura = asignatura,
                                Nombre = $"{asignatura.Nombre} Ev#{i + 1}",
                                Nota = (float)(5 * rnd.NextDouble()),
                                Alumno = alumno
                            };
                            evArray[i] = ev;
                        }
                        alumno.Evaluaciones.AddRange(evArray);
                    }
                }
            }

        }


        private void CargarAsignaturas()
        {
            foreach (var curso in Escuela.Cursos)
            {
                var listaAsignaturas = new List<Asignatura>(){
                            new Asignatura{Nombre="Matemáticas"} ,
                            new Asignatura{Nombre="Educación Física"},
                            new Asignatura{Nombre="Castellano"},
                            new Asignatura{Nombre="Ciencias Naturales"}
                };
                curso.Asignaturas = listaAsignaturas;
            }
        }


        private void CargarCursos()
        {
            Escuela.Cursos = new List<Curso>(){
                        new Curso(){ Nombre = "101", Jornada = TiposJornada.Mañana },
                        new Curso() {Nombre = "201", Jornada = TiposJornada.Mañana},
                        new Curso{Nombre = "301", Jornada = TiposJornada.Mañana},
                        new Curso(){ Nombre = "401", Jornada = TiposJornada.Tarde },
                        new Curso() {Nombre = "501", Jornada = TiposJornada.Tarde},
            };

            Random rnd = new Random();
            foreach (var c in Escuela.Cursos)
            {
                int cantRandom = rnd.Next(5, 20);
                c.Alumnos = GenerarAlumnosAlAzar(cantRandom);
            }
        }
    }
    #endregion
}