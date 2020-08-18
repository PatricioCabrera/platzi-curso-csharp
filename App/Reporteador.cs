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
        public Reporteador( Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObsEsc)
        {
            if (dicObsEsc is null)
            {
                throw new ArgumentNullException(nameof(dicObsEsc) );
            }
            _diccionario = dicObsEsc;
        }

        public IEnumerable<Evaluación> GetListaEvaluaciones()
        {
            IEnumerable<ObjetoEscuelaBase> lista = new List<Evaluación>();

            _diccionario.TryGetValue(LlaveDiccionario.Evaluación, out lista);

            return lista.Cast<Evaluación>();
        }
    }
}
