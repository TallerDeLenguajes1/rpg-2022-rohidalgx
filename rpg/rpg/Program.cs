﻿using System;
using System.Text.Json;


public class program
{
    public static void Main(String[] args)
    {
        int cantPersonajes = 0;
        int siOno = 0;
        int siOnoJSON = 0;
        FileStream fileStream;
        Console.WriteLine("\n BIENVENIDOS A MI JUEGUITO DE PONYS \n");


        Console.WriteLine("Desea ver un listado de todos los ganadores? si =1, no=0");
        siOno = Convert.ToInt32(Console.ReadLine());
        string? direccionArchivo = @"C:\taller_repo\rpg-2022-rohidalgx\rpg\ganadores.csv";
        if (!File.Exists(direccionArchivo))//SI NO EXISTE EL ARCHIVO LO CREA
        {
            fileStream = File.Create(direccionArchivo);
            fileStream.Close();
        }
        if (siOno == 1)
        {
            personaje.mostrarArchivoCSV(direccionArchivo);

        }

        string? pathJSON = @"C:\taller_repo\rpg-2022-rohidalgx\rpg\jugadores.json";

        Console.WriteLine("\n Ingrese su nombre");
        string? nombre = Console.ReadLine();
        Console.WriteLine("\n Con cuantos jugadores desea jugar? ");
        cantPersonajes = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Desea usar un personaje anterior? si =1, no=0");
        siOnoJSON = Convert.ToInt32(Console.ReadLine());
        Queue<personaje> filaPersonajes = new Queue<personaje>();
        List<personaje> listaPersonajes = new List<personaje>();

        if (siOnoJSON == 1)
        {
            if (!File.Exists(pathJSON))
            {
                Console.WriteLine("No existe un Json de participantes. Creando nuevos participantes...");
                siOnoJSON = 0;
            }
            else
            {
                StreamReader sr = new StreamReader(pathJSON);
                string? datoJson = sr.ReadLine();

                listaPersonajes = JsonSerializer.Deserialize<List<personaje>>(datoJson); //VER

                foreach (personaje jugador in listaPersonajes)
                {
                    jugador.crearCaracteristicasAleatorias();
                    jugador.crearDatosAleatorios();
                    filaPersonajes.Enqueue(jugador);
                }

                sr.Close();
            }

        }
        else
        {

            for (int i = 0; i < cantPersonajes; i++)//crea x cant de objetos personaje y los agrega a una fila
            {
                personaje personaje = new personaje();
                personaje.crearCaracteristicasAleatorias();
                personaje.crearDatosAleatorios();
                Console.WriteLine("\n Nombre del jugador [" + i+1 + "]? ");
                personaje.nombre = Console.ReadLine();
                filaPersonajes.Enqueue(personaje);
            }
            guardarParticipantesJSON(filaPersonajes, pathJSON);

        }



        personaje ganador = new personaje();
        while (filaPersonajes.Count > 1)
        {
            ganador = personaje.Pelea(filaPersonajes.Dequeue(), filaPersonajes.Dequeue());
            filaPersonajes.Enqueue(ganador);
        }

        Console.WriteLine("EL GANADOR SUPREMO ES:" + ganador.nombre);
        ganador.mostrarDatos();
        ganador.agregarGanador(direccionArchivo, ganador);

    }
    public static void guardarParticipantesJSON(Queue<personaje> personajes, string path)
    {


        if (!File.Exists(path))           
        {

            File.Create(path);

        }
        string? JSON; 


        FileStream FSJSON = new FileStream(path, FileMode.Open);
        StreamWriter SWJSON = new StreamWriter(FSJSON);

        JSON = JsonSerializer.Serialize(personajes);         

        SWJSON.WriteLine(JSON);                                    

        SWJSON.Close();                                                       
        FSJSON.Close();
    }

}