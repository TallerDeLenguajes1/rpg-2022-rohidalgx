using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class personaje
{
    
    public List<string> listaTipos = new List<string>()
    {
        "Ingeniero",
        "Medico",
        "Arquitecto",
        "Contador",
        "Fisico",
        "Astronauta",
        "Licenciado"
    };

    public List<string> listaApodos = new List<string>()
    {
        "Pepe",
        "Pili",
        "Negri",
        "Gorda" 
    };

    public string? tipo;
    public string? nombre;
    public string? apodo;
    public DateTime fechNac;
    public int edad;
    public int salud = 100;

    public void crearDatosAleatorios()
    {
        Random rnd = new Random();
        tipo = listaTipos[rnd.Next(0, listaTipos.Count)];
        apodo = listaApodos[rnd.Next(0, listaApodos.Count)];
        fechNac = CalcularNacimiento();
        edad = calcularEdad(fechNac);
    }
    public DateTime CalcularNacimiento()
    {
        Random rnd = new Random();
        DateTime FechaDeNacimiento = new DateTime(1723, 1, 1);//es la minima fecha posible (300 años)                     
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
        Console.WriteLine("\n Datos");
        Console.WriteLine("\n Nombre:" + nombre);
        Console.WriteLine("\n Fecha de nacimiento:" + fechNac.ToShortDateString());
        Console.WriteLine("\n Su personaje es de tipo:" + tipo);
        Console.WriteLine("\n Su personaje se apoda:" + apodo);
        Console.WriteLine("\n La edad de su personaje es:" + edad);
        Console.WriteLine("\n Salud del personaje:" + edad);

    }
    
    public int velocidad;
    public int destreza;
    public int fuerza;
    public int nivel;
    public int armadura;


    public void crearCaracteristicasAleatorias()
    {
        Random rnd = new Random();
        velocidad = rnd.Next(1, 11);
        destreza = rnd.Next(1, 6);
        fuerza = rnd.Next(1, 11);
        nivel = rnd.Next(1, 11);
        armadura = rnd.Next(1, 11);
    }

    public void mostrarCaracteristicas()
    {
        Console.WriteLine("\n ------Caracteristicas de :" +nombre);
        Console.WriteLine("\n Velocidad del personaje:" + velocidad);
        Console.WriteLine("\n Destreza del personaje:" + destreza);
        Console.WriteLine("\n Fuerza del personaje:" + fuerza);
        Console.WriteLine("\n Nivel del personaje:" + nivel);
        Console.WriteLine("\n Armadura del personaje:" + armadura);
    }

    public float Combate(personaje personaje)//calculo de las estadisticas
    {
        Random rnd = new Random();
        float pDisparo = personaje.destreza * personaje.fuerza * personaje.nivel;
        float efectividadDisparo = rnd.Next(1, 101);//COMO QUE PORCENTUAL
        float valorAtaque = pDisparo * efectividadDisparo;
        float pDefensa = personaje.armadura * personaje.velocidad;
        int maxDanio = 50000;
        float danioProvocado = (valorAtaque * efectividadDisparo - pDefensa) / maxDanio * 100;
        return (danioProvocado);
    }

    static public personaje Pelea(personaje personaje1, personaje personaje2)
    {
        int numPelea = 0;
        personaje1.mostrarCaracteristicas();
        personaje2.mostrarCaracteristicas();
        Console.WriteLine("\n Salud de " + personaje1.nombre+ ":"+personaje1.salud);
        Console.WriteLine("\n Salud de " + personaje2.nombre +":"+personaje2.salud);
        while(numPelea < 3 && personaje1.salud>0 && personaje2.salud > 0)
        {
            personaje2.salud = Convert.ToInt32(personaje2.salud - personaje1.Combate(personaje1));
            Console.WriteLine("\n Salud de " + personaje2.nombre + " despues del golpe:" + personaje2.salud);
            personaje1.salud = Convert.ToInt32(personaje1.salud - personaje2.Combate(personaje2));
            Console.WriteLine("\n Salud de " + personaje1.nombre + " despues del golpe:" + personaje1.salud);
            numPelea++;
        }

        if (personaje1.salud >= personaje2.salud)
        {
            Console.WriteLine("EL GANADOR DE LA PELEA ES:" + personaje1.nombre);
            personaje1 = personaje1.Beneficio(personaje1);
            return (personaje1);//DEVUELVE EL GANADOR
        }
        else
        {
            Console.WriteLine("EL GANADOR DE LA PELEA ES:" + personaje2.nombre);
            personaje2 = personaje2.Beneficio(personaje2);
            return (personaje2);
        }
    }

    public personaje Beneficio(personaje P)
    {
        Random rnd = new Random();
        switch (rnd.Next(1, 6))
        {
            case 1:
                P.salud += 10;
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
        streamWriter.WriteLine("Personajes Ganadores" +";"+ "Edad" +";"+ "Tipo\n");
        streamWriter.WriteLine(ganador.nombre + ";" + ganador.edad + ";" + ganador.tipo);

    }

    static public void mostrarArchivoCSV(string? path)
    {
        using TextReader streamReader = new StreamReader(path);
        var texto = streamReader.ReadToEnd();
        texto = texto.Replace(";", " ");
        Console.WriteLine(texto);
    }
}
