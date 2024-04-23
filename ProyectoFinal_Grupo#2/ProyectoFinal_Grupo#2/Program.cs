using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticoEnfermedades
{
    class Program
    {
        static void Main(string[] args)
        {
            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("Seleccione una opción:");
                Console.WriteLine("1. Realizar diagnóstico");
                Console.WriteLine("2. Salir");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        // Construir el árbol de decisiones
                        DecisionTree arbolDecisiones = ConstruirArbolDecisiones();

                        // Realizar diagnóstico
                        RealizarDiagnostico(arbolDecisiones);
                        break;
                    case "2":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Por favor, seleccione una opción válida.");
                        break;
                }
            }
        }

        static DecisionTree ConstruirArbolDecisiones()
        {
            // Construir el árbol de decisiones
            DecisionTree nodoRaiz = new DecisionTree("fiebre");
            DecisionTree nodoDolorDeCabeza = nodoRaiz.AgregarHijo("si", "¿Tiene dolor de cabeza?");
            DecisionTree nodoCongestionNasal = nodoRaiz.AgregarHijo("no", "¿Tiene congestión nasal?");
            DecisionTree nododolorfacial = nodoCongestionNasal.AgregarHijo("si", "¿Tiene dolor facial?");
            DecisionTree nodoAlergia = nodoCongestionNasal.AgregarHijo("no", "¿Tiene estornudos?");
            DecisionTree nodoEstornudos = nodoAlergia.AgregarHijo("si", "¿Tiene picazón en los ojos?");
            DecisionTree nodoDolorDeGarganta = nodoAlergia.AgregarHijo("no", "¿Tiene dolor de garganta?");
            DecisionTree nodoBronquitis = nodoDolorDeGarganta.AgregarHijo("si", "¿Tiene dificultad para respirar?");
            DecisionTree nodoSinositis = nododolorfacial.AgregarHijo("si", "Sinositis", "Tratamiento para la sinositis: antihistamínicos, descongestionantes, evitación del alérgeno.");
            DecisionTree nodoGripe = nodoDolorDeCabeza.AgregarHijo("si", "Gripe", "Tratamiento para la gripe: Descanso, líquidos, medicamentos para reducir la fiebre y aliviar los síntomas");
            DecisionTree nodoResfriadoComun = nodoDolorDeCabeza.AgregarHijo("no", "Resfriado común", "Tratamiento para el Resfriado común:  descanso, líquidos, medicamentos para aliviar la congestión y la tos.\r\n");
            DecisionTree nodoAlergiaFinal = nodoEstornudos.AgregarHijo("si", "Alergia", "Tratamiento para la Alergia: antihistamínicos, descongestionantes, evitación del alérgeno.");
            DecisionTree nodoBronquitisFinal = nodoBronquitis.AgregarHijo("si", "Bronquitis", "Tratamiento para la Bronquitis: descanso, líquidos, inhaladores broncodilatadores, medicamentos para aliviar la tos.");

            // Retornar el nodo raíz del árbol
            return nodoRaiz;
        }

        static void RealizarDiagnostico(DecisionTree arbolDecision)
        {
            Console.WriteLine("Bienvenido al sistema de diagnóstico médico.");
            Console.WriteLine("Por favor, responda las siguientes preguntas con 'si' o 'no'.");
            Console.WriteLine();

            // Empezar con el nodo raíz
            DecisionTree nodoActual = arbolDecision;

            while (nodoActual != null)
            {
                // Mostrar la pregunta actual
                Console.WriteLine(nodoActual.Pregunta);

                // Obtener la respuesta del usuario
                string respuesta = Console.ReadLine().ToLower();

                // Avanzar al siguiente nodo según la respuesta
                DecisionTree siguienteNodo = nodoActual.ObtenerSiguienteNodo(respuesta);

                // Si el siguiente nodo es nulo, hemos llegado a un nodo hoja
                if (siguienteNodo == null)
                {
                    // Mostrar el diagnóstico y salir del bucle
                    Console.WriteLine("El diagnóstico es: " + nodoActual.Diagnostico);
                    break;
                }
                else
                {
                    // Avanzar al siguiente nodo
                    nodoActual = siguienteNodo;
                }
            }
        }
    }

    class DecisionTree
    {
        public string Pregunta { get; }
        public string Diagnostico { get; }
        public bool EsHoja => hijos.Count == 0;

        private readonly Dictionary<string, DecisionTree> hijos = new Dictionary<string, DecisionTree>();

        public DecisionTree(string pregunta, string diagnostico = null)
        {
            Pregunta = pregunta;
            Diagnostico = diagnostico;
        }

        public DecisionTree AgregarHijo(string respuesta, string pregunta, string diagnostico = null)
        {
            DecisionTree nuevoNodo = new DecisionTree(pregunta, diagnostico);
            hijos.Add(respuesta.ToLower(), nuevoNodo);
            return nuevoNodo;
        }

        public DecisionTree ObtenerSiguienteNodo(string respuesta)
        {
            if (hijos.ContainsKey(respuesta.ToLower()) || (respuesta.ToLower() == "si" && hijos.ContainsKey("sí")))
            {
                return hijos[respuesta.ToLower()];
            }
            else
            {
                Console.WriteLine("Respuesta inválida. Por favor, responda con 'si' o 'no'.");
                return null;
            }
        }
    }
}

