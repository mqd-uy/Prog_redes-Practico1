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
        //Ej5();
        //Ej6();
        //Ej7();
        Ej8();

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

        object candado = new Object();

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
            {
                start = true;
                lock (candado)
                {
                    Monitor.Pulse(candado);
                }
            }
        }

        Console.WriteLine("Hilo principal");

        void HiloSaluda(int i)
        {
            Console.WriteLine("Empezó hilo " + i);
            lock (candado)
            {
                Monitor.Wait(candado);
                Console.WriteLine("Hola, soy el hilo " + i);
                Monitor.Pulse(candado);
            }
        }
    }

    static void Ej6()
    {
        Console.WriteLine("*** Ejercicio 6 ***");

        object candado = new object();

        int cantHilos;
        bool seImprimio = false;

        Console.Write("Cuantos hilos?: ");
        cantHilos = int.Parse(Console.ReadLine()!);

        for (int i = 0; i < cantHilos; i++)
        {
            int num = i + 1;
            Thread t = new Thread(() => Hilo(num));
            t.IsBackground = true;
            t.Start();
        }

        lock (candado)
        {
            while (!seImprimio)
            {
                Monitor.Wait(candado);
            }
        }

        Console.WriteLine("Finalizando programa, hilos en background por abortarse");

        void Hilo(int num)
        {
            lock (candado)
            {
                if (!seImprimio)
                {
                    seImprimio = true;
                    Console.WriteLine("Bienvenidos a Programación de Redes 2026 desde el hilo {0}", num);
                    // le avisa al hilo principal que quedó en wait
                    Monitor.Pulse(candado);
                }
            }
        }
    }

    static void Ej7()
    {
        Console.WriteLine("*** Ejercicio 7 ***");

        object candado = new object();

        // trato a las personas como un string, el nombre es lo que me interesa
        List<string> personas = new List<string>();

        for (int i = 0; i < 5; i++)
        {
            int num = i + 1;
            new Thread(() =>
            {
                try
                {
                    PedirDatos(num);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }).Start();
        }

        void PedirDatos(int num)
        {
            // el lock lo hago para todo el I/O porque estoy en una sola ventana, si cada hilo fuera su propia ventana
            // el lock seria solo para el leer/escribir lista de nombres
            lock (candado)
            {
                Console.WriteLine($"Pidiendo datos desde hilo: {num}");
                Console.Write("Nombre: ");
                string nombre = Console.ReadLine();
                if (personas.Contains(nombre))
                    throw new Exception("Ya existe una persona con ese nombre, no se agregará");
                personas.Add(nombre);
                Console.WriteLine($"{nombre} agregado, ahora hay {personas.Count} personas");
            }
        }

    }

    static void Ej8()
    {
        Console.WriteLine("*** Ejercicio 8 ***");

        object candado = new object();
        int hilosTerminados = 0;

        const int largoListas = 10;

        List<int> listaA = GenerarListaEnteros(largoListas);
        List<int> listaB = GenerarListaEnteros(largoListas);

        List<double> cocientes = new List<double>();


        for (int i = 0; i < largoListas; i++)
        {
            int num = i;
            new Thread(() =>
            {
                try
                {
                    Cocientes(num);
                }
                catch (DivideByZeroException ex)
                {
                    Console.WriteLine("No se puede dividir por 0. Cociente no tenido en cuanta para el promedio");
                }
                finally
                {
                    lock (candado)
                    {
                        hilosTerminados++;
                        Monitor.Pulse(candado);
                    }
                }
            }).Start();
        }

        lock (candado)
        {
            while (hilosTerminados < largoListas)
            {
                Monitor.Wait(candado);
            }
        }
        double sumaCocientes = 0;
        cocientes.ForEach((n) => sumaCocientes += n);
        Console.WriteLine("El promedio de los cocientes es: " + (sumaCocientes / cocientes.Count));

        void Cocientes(int num)
        {
            int a = listaA[num];
            int b = listaB[num];
            // si hay division por 0 no se agrega
            if (b == 0)
                throw new DivideByZeroException();
            double cociente = (double)a / b;
            cocientes.Add(cociente);
            Console.WriteLine($"Hilo {num + 1} cociente: {cociente}");
        }

        List<int> GenerarListaEnteros(int cant)
        {
            Random genarador = new Random();
            List<int> lista = new List<int>();
            for (int i = 0; i < cant; i++)
            {
                lista.Add(genarador.Next(10));
            }
            return lista;
        }
    }
}
