using System;
int cantPersonajes = 0;

Console.WriteLine("\n BIENVENIDOS A MI JUEGUITO RE PIOLA \n");

Console.WriteLine("\n Ingrese su nombre");
string? nombre = Console.ReadLine();

Console.WriteLine("\n Con cuantos jugadores desea jugar? ");
cantPersonajes = Convert.ToInt32(Console.ReadLine());
Queue<personaje> filaPersonajes = new Queue<personaje>();

for (int i = 0; i < cantPersonajes; i++)//crea x cant de objetos personaje y los agrega a una fila
{
    personaje personaje = new personaje();
    personaje.crearCaracteristicasAleatorias();
    personaje.crearDatosAleatorios();
    Console.WriteLine("\n Nombre del jugador [" +i + "]? ");
    personaje.nombre = Console.ReadLine();
    filaPersonajes.Enqueue(personaje);
}

personaje ganador = new personaje();
while (filaPersonajes.Count>1)
{
    ganador = personaje.Pelea(filaPersonajes.Dequeue(), filaPersonajes.Dequeue());
    filaPersonajes.Enqueue(ganador);
}

Console.WriteLine("EL GANADOR SUPREMO ES:" + ganador.nombre);
ganador.mostrarDatos();



