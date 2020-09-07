using CoreEscuela.Entidades;
using csharp.Entidades;
using Etapa1.Entidades;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CoreEscuela.App
{
    public class Reporteador
    {
        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;
        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObsEsc)
        {
            if (dicObsEsc is null)
            {
                throw new ArgumentNullException(nameof(dicObsEsc));
            }
            _diccionario = dicObsEsc;
        }

        public IEnumerable<Evaluación> GetListaEvaluaciones()
        {
            IEnumerable<ObjetoEscuelaBase> lista = new List<Evaluación>();

            _diccionario.TryGetValue(LlaveDiccionario.Evaluación, out lista);

            return lista.Cast<Evaluación>();
        }

        public IEnumerable<string> GetListaAsignaturas(out IEnumerable<Evaluación> listaEvaluaciones)
        {
            listaEvaluaciones = GetListaEvaluaciones();

            return (from Evaluación ev in listaEvaluaciones
                    select ev.Asignatura.Nombre).Distinct();
        }

        public IEnumerable<string> GetListaAsignaturas()
        {

            return GetListaAsignaturas(out var dummy);
        }

        public Dictionary<string, IEnumerable<Evaluación>> GetEvalXAsig()
        {
            Dictionary<string, IEnumerable<Evaluación>> dicRta = new Dictionary<string, IEnumerable<Evaluación>>();
            IEnumerable<string> listaAsig = GetListaAsignaturas(out IEnumerable<Evaluación> listaEval);

            foreach (var asig in listaAsig)
            {
                var evalsAsig = from eval in listaEval
                                where eval.Asignatura.Nombre == asig
                                select eval;
                dicRta.Add(asig, evalsAsig);
            }

            return dicRta;
        }

        public Dictionary<string, IEnumerable<AlumnoPromedio>> GetPromeAlumnPorAsignatura()
        {
            var rta = new Dictionary<string, IEnumerable<AlumnoPromedio>>();
            var dicEvalXAsig = GetEvalXAsig();

            foreach (var asigConEval in dicEvalXAsig)
            {
                var promsAlumn = from eval in asigConEval.Value
                            group eval by new
                            {
                                eval.Alumno.UniqueId,
                                eval.Alumno.Nombre
                            }
                            into grupoEvalsAlumno
                            select new AlumnoPromedio
                            {   alumnoID = grupoEvalsAlumno.Key.UniqueId,
                                alumnoNombre = grupoEvalsAlumno.Key.Nombre,
                                promedio = grupoEvalsAlumno.Average( evaluación=> evaluación.Nota )
                            };
                rta.Add(asigConEval.Key, promsAlumn);
            }

            return rta;
        }

        public Dictionary<string, IEnumerable<AlumnoPromedio>> GetMejorPromeAlumnPorAsignatura(int top = 10)
        {
            var rta = new Dictionary<string, IEnumerable<AlumnoPromedio>>();
            var promediosXAsignatura = GetPromeAlumnPorAsignatura();

            foreach (var item in promediosXAsignatura)
            {
                var query = (from asign in item.Value
                             orderby asign.promedio descending
                             select asign).Take(top);
                rta.Add(item.Key, query);
            }

            return rta;
        }
    }
}
