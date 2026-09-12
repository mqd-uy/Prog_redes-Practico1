using System.Threading;

namespace Ej01;

class Program
{
    static void Main(string[] args)
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
    }

    static void Imprime(string message, int cant = 20)
    {
        for (int i = 0; i < cant; i++)
        {
            Console.WriteLine(message + " nro " + (i + 1));
            Thread.Sleep(100);
        }
    }
}