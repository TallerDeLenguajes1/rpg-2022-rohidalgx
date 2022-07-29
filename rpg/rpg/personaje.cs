using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

public class character
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Datum
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("alias")]
        public string Alias { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        [JsonPropertyName("residence")]
        public string Residence { get; set; }

        [JsonPropertyName("occupation")]
        public string Occupation { get; set; }

        [JsonPropertyName("kind")]
        public List<string> Kind { get; set; }

        [JsonPropertyName("image")]
        public List<string> Image { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("data")]
        public List<Datum> Data { get; set; }
    }

}
public class personaje
{

    //public List<string> listaTipos = new List<string>()
    //{
    //    "Alicornio",
    //    "Unicornio",
    //    "Pagaso",
    //    "Pony de Cristal",
    //    "Pony Terrestre"
    //};

    //public List<string> listaApodos = new List<string>()
    //{
    //    "Twilight Sparkle",
    //    "Fluttershy",
    //    "Pinkie Pie",
    //    "Rarity",
    //    "Rainbow Dash",
    //    "Applejack"
    //};

    public string? nombrePlayer { get; set; }
    public string? tipo { get; set; }
    public string? nombre { get; set; }
    public string? apodo { get; set; }
    public DateTime fechNac { get; set; }
    public int edad { get; set; }

    public int salud = 100;

    public void crearDatosAleatorios(List<character.Datum> data)
    {
        
        Random rnd = new Random();
        int id = rnd.Next(0, data.Count);
        nombre = data[id].Name;
        apodo = data[id].Alias;
        int cant = data[id].Kind.Count;
        tipo = data[id].Kind[rnd.Next(0, cant)];
        fechNac = CalcularNacimiento();
        edad = calcularEdad(fechNac);
    }
    public DateTime CalcularNacimiento()
    {
        Random rnd = new Random();
        DateTime FechaDeNacimiento = new DateTime(2010, 10, 10);//es la minima fecha posible (la fecha en la que se estreno la serie)                     
        int MaximoSumaFecha = (DateTime.Today - FechaDeNacimiento).Days;//dias entre hoy y esa fecha minima                   
        FechaDeNacimiento = FechaDeNacimiento.AddDays(rnd.Next(MaximoSumaFecha));//se le suma un random del total de dias          
        return (FechaDeNacimiento);
    }

    public int calcularEdad(DateTime FechaDeNacimiento)
    {
        DateTime FechaActual = DateTime.Now;
        if (FechaDeNacimiento.Month >= FechaActual.Month && FechaDeNacimiento.Day > FechaActual.Day)
        {
            return (FechaActual.Year - FechaDeNacimiento.Year - 1);
        }
        else
        {
            return (FechaActual.Year - FechaDeNacimiento.Year);
        }
    }
    public void mostrarDatos()
    {
        Console.WriteLine("\n ***Datos del personaje***");
        Console.WriteLine("\n Nombre:" + nombre);
        Console.WriteLine("\n Apodo:" + apodo);
        Console.WriteLine("\n Tipo:" + tipo);
        Console.WriteLine("\n Fecha de nacimiento:" + fechNac.ToShortDateString());
        Console.WriteLine("\n Edad:" + edad);
        Console.WriteLine("\n ******");

    }
    
    public int velocidad;
    public int destreza;
    public int fuerza;
    public int nivel;
    public int armadura;


    public void crearCaracteristicasAleatorias()
    {
        Random rnd = new Random();
        nivel = rnd.Next(1, 11);
        velocidad = nivel + rnd.Next(1, 11);
        destreza = nivel + rnd.Next(1, 6);
        fuerza = nivel + rnd.Next(1, 11);
        armadura = nivel + rnd.Next(1, 11);
    }

    public void mostrarCaracteristicas()
    {
        Console.WriteLine("\n ***Caracteristicas de: " +nombre +"***");
        Console.WriteLine("\n Nivel:" + nivel);
        Console.WriteLine("\n Velocidad:" + velocidad);
        Console.WriteLine("\n Destreza:" + destreza);
        Console.WriteLine("\n Fuerza:" + fuerza);
        Console.WriteLine("\n Armadura:" + armadura);
        Console.WriteLine("\n ******");
    }

    public float Combate(personaje personaje)//calculo de las estadisticas
    {
        Random rnd = new Random();
        float pDisparo = personaje.destreza * personaje.fuerza * personaje.nivel;
        float efectividadDisparo = rnd.Next(1, 101);
        float valorAtaque = pDisparo * efectividadDisparo;
        float pDefensa = personaje.armadura * personaje.velocidad;
        int maxDanio = 50000;
        float danioProvocado = (valorAtaque * efectividadDisparo - pDefensa) / maxDanio;
        return (danioProvocado);
    }

    static public personaje Pelea(personaje personaje1, personaje personaje2)
    {
        Console.ReadKey();
        Console.WriteLine("\n --------------------");
        Console.WriteLine("\n " + personaje1.nombre + " VS " + personaje2.nombre);
        Console.WriteLine("\n --------------------");
        Console.ReadKey();
        int numPelea = 0;
        personaje1.mostrarCaracteristicas();
        Console.ReadKey();
        personaje2.mostrarCaracteristicas();
        Console.ReadKey();
        while (numPelea < 3 && personaje1.salud > 0 && personaje2.salud > 0)
        {
            Console.WriteLine("\n Round: " + (numPelea + 1));
            personaje2.salud = Convert.ToInt32(personaje2.salud - personaje1.Combate(personaje1));
            if (personaje2.salud < 0)
            {
                personaje2.salud = 0;
                Console.WriteLine("\n Salud de " + personaje2.nombre + " despues del golpe: " + personaje2.salud);
                Console.WriteLine("\n Queda fuera de combate.");
                Console.WriteLine("\n EL GANADOR DE LA PELEA ES:" + personaje1.nombre);
                personaje1 = personaje1.Beneficio(personaje1);
                return (personaje1);
            }
            else
            {
                Console.WriteLine("\n Salud de " + personaje2.nombre + " despues del golpe: " + personaje2.salud);
            }
            personaje1.salud = Convert.ToInt32(personaje1.salud - personaje2.Combate(personaje2));

            if (personaje1.salud < 0)
            {
                personaje1.salud = 0;
                Console.WriteLine("\n Salud de " + personaje1.nombre + " despues del golpe:" + personaje1.salud);
                Console.WriteLine("\n Queda fuera de combate.");
                Console.WriteLine("\n EL GANADOR DE LA PELEA ES:" + personaje2.nombre);
                personaje2 = personaje2.Beneficio(personaje2);
                return (personaje2);
            }
            else
            {
                Console.WriteLine("\n Salud de " + personaje1.nombre + " despues del golpe:" + personaje1.salud);
            }
            numPelea++;
        }

        if (personaje1.salud >= personaje2.salud && personaje1.salud > 0)
        {
            Console.WriteLine("\n EL GANADOR DE LA PELEA ES:" + personaje1.nombre);
            personaje1 = personaje1.Beneficio(personaje1);
            Console.ReadKey();
            return (personaje1);//DEVUELVE EL GANADOR
        }
        else
        {
            if (personaje2.salud > 0)
            {
                Console.WriteLine("\n EL GANADOR DE LA PELEA ES:" + personaje2.nombre);
                personaje2 = personaje2.Beneficio(personaje2);
                Console.ReadKey();
                return (personaje2);

            }
            else
            {
                Console.WriteLine("\n No sobrevivio nadie");
                return (null);
            }
        }
    }

    public personaje Beneficio(personaje P)
    {
        Random rnd = new Random();
        P.salud = 100;
        switch (rnd.Next(1, 6))
        {
            case 1:
                P.nivel += 2;
                break;
            case 2:
                P.velocidad += P.velocidad * rnd.Next(5, 11) / 100;
                break;
            case 3:
                P.destreza += P.destreza * rnd.Next(5, 11) / 100;
                break;
            case 4:
                P.fuerza += P.fuerza * rnd.Next(5, 11) / 100;
                break;
            case 5:
                P.nivel += 1;
                break;
            case 6:
                P.armadura += P.armadura * rnd.Next(5, 11) / 100;
                break;

        }
        return (P);
    }


    public void agregarGanador(string path, personaje ganador)
    {
        using TextWriter streamWriter = File.AppendText(path);
        streamWriter.WriteLine(ganador.nombrePlayer + ";" + ganador.nombre + ";" + ganador.edad + ";" + ganador.tipo);

    }

    static public void mostrarArchivoCSV(string? path)
    {
        FileStream fileStream;
        if (!File.Exists(path))//SI NO EXISTE EL ARCHIVO LO CREA
        {
            fileStream = File.Create(path);
            fileStream.Close();
        }
        else
        {
            using TextReader streamReader = new StreamReader(path);
            var texto = streamReader.ReadToEnd();
            texto = texto.Replace(";", " ");
            Console.WriteLine("\n Nombre del Jugador | Nombre Pony | Edad | Tipo");
            Console.WriteLine(texto);

        }
    }
}
