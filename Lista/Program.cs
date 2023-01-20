using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Lista
{
    class Program
    {

        static void Main(string[] args)
        {
            var lista = new List<Shakira>();

            var casio = new Shakira();
            {

                casio.text = "Quizás no seamos un Rolex, pero al menos no nos han dejado por Clara Chía";
            };

            lista.Add(casio);

            foreach (var item in lista)
            {
                Console.WriteLine($"{item.text}");
            }

            Console.ReadKey();
        }
    }
}
