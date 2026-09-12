using System.Threading;

namespace Ej01;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("*** Práctico 1 ***");

        //Ej1();
        //Ej2();
        Ej3();

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
}