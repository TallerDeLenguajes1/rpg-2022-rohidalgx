using System;
using System.Text.Json;
using System.Net;


public class program
{
    public static void Main(String[] args)
    {
        int cantPersonajes = 0;
        int siOno = 0;
        int siOnoJSON = 0;
        FileStream fileStream;
        Console.WriteLine("\n BIENVENIDOS A EQUESTRIA \n");

        ///////////////
        character.Root API = new character.Root();
        API = conectarAPI();
        //////////////

        Console.WriteLine("\n Desea ver un listado de todos los ganadores? si =1, no=0");
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

        Console.WriteLine("\n Desea usar un personaje anterior? si =1, no=0");
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

                listaPersonajes = JsonSerializer.Deserialize<List<personaje>>(datoJson); //aca mete todos los jugadores anteriores en una lista
                List<personaje> listaAux = new List<personaje>(); //uso esta lista auxiliar para meter solo X cant de jugadores

                for(int i = 0; i < cantPersonajes; i++) //aca hago eso
                {
                    listaAux.Add(listaPersonajes[i]);
                }

                foreach (personaje jugador in listaAux) //aca les doy caracteristicas y eso y los paso a una fila
                {
                    jugador.crearCaracteristicasAleatorias();
                    jugador.crearDatosAleatorios(API.Data);
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
                personaje.crearDatosAleatorios(API.Data);
                personaje.nombrePlayer = nombre;
                filaPersonajes.Enqueue(personaje);
            }
            guardarParticipantesJSON(filaPersonajes, pathJSON);

        }

        personaje ganador = new personaje();
        while (filaPersonajes.Count > 1)
        {
            ganador = personaje.Pelea(filaPersonajes.Dequeue(), filaPersonajes.Dequeue());
            if (ganador != null)
            {
                filaPersonajes.Enqueue(ganador);
            }
        }

        if(ganador != null)
        {
            Console.WriteLine("\n******************************");
            Console.WriteLine("\n EL PONY GANADOR DE EQUESTRIA ES: " + ganador.nombre);
            ganador.mostrarDatos();
            ganador.agregarGanador(direccionArchivo, ganador);
            Console.ReadKey();

        }
        else
        {
            Console.WriteLine("\n NO GANO NADIE ");
        }

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

    public static character.Root conectarAPI()
    {

        var url = "https://ponyweb.ml/v1/character/all";
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        character.Root? api = new character.Root();
        try
        {
            using (WebResponse respuesta = request.GetResponse())
            {
                using (Stream streamReader = respuesta.GetResponseStream())
                {
                    if (streamReader != null)
                    {
                        using (StreamReader objReader = new StreamReader(streamReader))
                        {
                            string? responseBody = objReader.ReadToEnd();
                            api = JsonSerializer.Deserialize<character.Root>(responseBody);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se obtuvo respuesta del servicio web");
                        return (null);
                    }
                }
            }
        }
        catch (WebException e)
        {
            Console.WriteLine(e.ToString());
            return (null);
        }

        return (api);
    }





}