

namespace UndirLaFlota.Juego;

public class Tablero
{
    public List<List<int>> TableroList { get; set; }
    private List<List<(int x, int y)>> posicionesBarcos;
    private List<int> Barcos;
    private int aciertos = 0;
    private int TotalPuntos;
    public int Dim;
    

    public Tablero()
    {
        Dim = 10;
        TableroList = new List<List<int>>();
        posicionesBarcos = new List<List<(int x, int y)>>();
        Barcos = new List<int>();
        Barcos.Add(6);
        Barcos.Add(5);
        Barcos.Add(3);
        Barcos.Add(3);
        Barcos.Add(2);
        foreach (int i in Barcos)
        {
            TotalPuntos += i;
        }
        
        TableroLimpio();
        GenerarTablero();
    }

    public void GenerarTablero()
    {
        foreach (int i in Barcos)
        {
            GenerarBarco(i);
        }
    }

    private void GenerarBarco(int tamano)
    {
        Random random = new Random();
        int x = 0;
        int y = 0;
        int dir = 0;
        int P_x = 0;
        int P_y = 0;
        bool entra = false;
        int pos;

        while (!entra)
        {
            entra = true;
            x = random.Next(0, Dim);
            y = random.Next(0, Dim);
            dir = random.Next(0, 4);
            P_x = x;
            P_y = y;
            pos = 0;

            while (pos < tamano)
            {
                if (P_x < 0 || P_x >= 10 || P_y < 0 || P_y >= 10)
                {
                    entra = false;
                    break;
                }
                else if (TableroList[P_x][P_y] != 0)
                {
                    entra = false;
                    break;
                }

                switch (dir)
                {
                    case 0:
                        P_x++;
                        break;
                    case 1:
                        P_x--;
                        break;
                    case 2:
                        P_y++;
                        break;
                    case 3:
                        P_y--;
                        break;
                }
                pos++;
            }
        }

        if (entra)
        {
            pos = 0;
            List<(int, int)> posiciones = new List<(int, int)>();
            P_x = x;
            P_y = y;

            while (pos < tamano)
            {
                TableroList[P_x][P_y] = 2;
                posiciones.Add((P_x, P_y));

                switch (dir)
                {
                    case 0:
                        P_x++;
                        break;
                    case 1:
                        P_x--;
                        break;
                    case 2:
                        P_y++;
                        break;
                    case 3:
                        P_y--;
                        break;
                }

                pos++;
            }

            posicionesBarcos.Add(posiciones);
        }
    }
    private void TableroLimpio()
    {
        aciertos = 0;
        for (int i = 0; i < Dim; i++)
        {
            TableroList.Add(new List<int>());
            for (int j = 0; j < Dim; j++)
            {
                TableroList[i].Add(0);
            }
        }
    }

    public String Jugada(int x, int y)
    {
        if (TableroList[x][y] == 0)
        {
            TableroList[x][y] = 1;
            return "Agua";
        }
        else if (TableroList[x][y] == 1 || TableroList[x][y] == 3)
        {
            return null;
        }
        else if (TableroList[x][y] == 2)
        {
            aciertos++;
            TableroList[x][y] = 3;

            // Verificar si el barco está hundido
            if (EstaHundido(x, y))
            {
                if (aciertos >= TotalPuntos)
                {
                    return "Partida finalizada";
                }
                return "Hundido";
            }

            if (aciertos >= TotalPuntos)
            {
                return "Partida finalizada";
            }

            return "Tocado";
        }

        return null;
    }

    private bool EstaHundido(int x, int y)
    {
        foreach (var barco in posicionesBarcos)
        {
            // Si el barco contiene esta posición
            if (barco.Contains((x, y)))
            {
                // Comprobar si todas las posiciones están tocadas
                foreach (var pos in barco)
                {
                    if (TableroList[pos.Item1][pos.Item2] != 3)
                        return false;
                }
                return true;
            }
        }
        return false;
    }
}