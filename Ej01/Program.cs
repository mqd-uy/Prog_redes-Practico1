using System.Runtime.Serialization.Formatters;
using System.Threading;

namespace Ej01;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("*** Práctico 1 ***");

        //Ej1();
        //Ej2();
        //Ej3();
        //Ej4();
        Ej5();

    }

    static void Ej1()
    {
        Console.WriteLine("*** Ejercicio 1 ***");
        // en realidad Thread siempre espera un delegado
        ThreadStart delegado1 = new ThreadStart(() => Imprime("X"));
        Thread t1 = new Thread(delegado1);
        // pero se puede hacer el atajo y pasar el metodo o una funcion anonima con la invocacion
        Thread t2 = new Thread(() => Imprime("Y"));
        t1.Start();
        // forma 1 con join
        t1.Join();
        t2.Start();

        // forma 2 chequeando estado a que haya finalizado hilo 1
        // while (t1.ThreadState != ThreadState.Stopped)
        // {
        //     Thread.Sleep(50);
        // }
        // t2.Start();

        Console.WriteLine("Finalizado programa");

        void Imprime(string message, int cant = 20)
        {
            for (int i = 0; i < cant; i++)
            {
                Console.WriteLine(message + " nro " + (i + 1));
                Thread.Sleep(100);
            }
        }

    }

    static void Ej2()
    {
        Console.WriteLine("*** Ejercicio 2 ***");

        Thread t1 = new Thread(CienCeros);
        t1.Start();
        t1.Join();

        Console.WriteLine("100 ceros finalizados en otro hilo");


        void CienCeros()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.Write("0");
            }
        }
    }

    static void Ej3()
    {
        Console.WriteLine("*** Ejercicio 3 ***");
        int suma1 = 0;
        int suma2 = 0;

        Thread t1 = new Thread(() => suma1 = Sumar2(1, 2));
        Thread t2 = new Thread(() => suma2 = Sumar3(1, 2, 3));
        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();


        Console.WriteLine("La suma es: " + (suma1 + suma2));


        int Sumar2(int a, int b)
        {
            return a + b;
        }

        int Sumar3(int a, int b, int c)
        {
            return a + b + c;
        }
    }

    static void Ej4()
    {
        Console.WriteLine("*** Ejercicio 4 ***");
        Console.WriteLine("""
            
            Un programa es el código, el proceso es el programa en ejecución en un sistema operativo
             con un espacio de memoria asignado. Un hilo es la unidad minima de ejecucion, un proceso tiene 
             como minimo un hilo.
             Tener varios hilos tiene la ventaja de poder realizar tareas concurrentemente, sin que un hilo
             solo bloquee la ejecucion del proceso, por ejemplo al esperar por entrada o salida
            
            """);
    }

    static void Ej5()
    {
        Console.WriteLine("*** Ejercicio 5 ***");

        bool start = false;
        
        for (int i = 0; i < 10; i++)
        {
            int num = i + 1;
            new Thread(() =>
            {
                HiloSaluda(num);
            }).Start();
        }

        while (!start)
        {
            Console.Write("Ingrese start: ");
            string input = Console.ReadLine();
            if (input == "start")
                start = true;
        }

        Console.WriteLine("Hilo principal");

        void HiloSaluda(int i)
        {
            while (true)
            {
                if (start)
                {
                    Console.WriteLine("Hola, soy el hilo " + i);
                    break;
                }
            }
        }

    }
}