using UndirLaFlota.Juego;

namespace UndirLaFlota
{

    public partial class MainPage : ContentPage
    {
        private Tablero jugadorTablero;
        private Tablero maquinaTablero;
        private HashSet<(int, int)> jugadasMaquina;
        private int aciertos = 0;
        private int fallos = 0;
        private int Disparos => aciertos + fallos;

        public MainPage()
        {
            InitializeComponent();

            this.SizeChanged += (s, e) =>
            {
                if (Width > Height)
                    VisualStateManager.GoToState(this, "Landscape");
                else
                    VisualStateManager.GoToState(this, "Portrait");
            };

            NuevaPartida();
        }

        private void NuevaPartida()
        {
            jugadorTablero = new Tablero();
            maquinaTablero = new Tablero();
            jugadasMaquina = new HashSet<(int, int)>();
            aciertos = 0;
            fallos = 0;
            InicializarTableros();
            ActualizarContadores();
        }

        private void InicializarTableros()
        {
            TableroJugadorGrid.Children.Clear();
            TableroMaquinaGrid.Children.Clear();

            TableroJugadorGrid.RowDefinitions.Clear();
            TableroJugadorGrid.ColumnDefinitions.Clear();
            TableroMaquinaGrid.RowDefinitions.Clear();
            TableroMaquinaGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < 10; i++)
            {
                TableroJugadorGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                TableroJugadorGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                TableroMaquinaGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                TableroMaquinaGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var jugadorBtn = new Button
                    {
                        BackgroundColor = jugadorTablero.TableroList[i][j] == 2 ? Colors.Gray : Colors.LightGray,
                        HeightRequest = 40,
                        WidthRequest = 40,
                        IsEnabled = false
                    };
                    TableroJugadorGrid.Add(jugadorBtn, j, i);

                    var maquinaBtn = new Button
                    {
                        BackgroundColor = Colors.LightGray,
                        HeightRequest = 40,
                        WidthRequest = 40,
                        CommandParameter = new Tuple<int, int>(i, j)
                    };

                    maquinaBtn.Clicked += (s, e) =>
                    {
                        var btn = (Button)s;
                        var pos = (Tuple<int, int>)btn.CommandParameter;
                        JugadaJugador(btn, pos.Item1, pos.Item2);
                    };

                    TableroMaquinaGrid.Add(maquinaBtn, j, i);
                }
            }
        }

        private void JugadaJugador(Button btn, int x, int y)
        {
            var resultado = maquinaTablero.Jugada(x, y);
            if (resultado == null)
                return;

            if (resultado == "Agua")
            {
                fallos++;
                btn.BackgroundColor = Colors.LightBlue;
            }
            else if (resultado == "Tocado")
            {
                aciertos++;
                btn.BackgroundColor = Colors.Red;
            }
            else if (resultado == "Partida finalizada")
            {
                aciertos++;
                btn.BackgroundColor = Colors.DarkRed;
                DisplayAlert("¡Victoria!", "¡Has hundido todos los barcos enemigos!", "OK");
            }

            btn.IsEnabled = false;
            ActualizarContadores();

            JugadaMaquina();
        }

        private void JugadaMaquina()
        {
            Random rnd = new Random();
            int x, y;
            do
            {
                x = rnd.Next(0, 10);
                y = rnd.Next(0, 10);
            } while (jugadasMaquina.Contains((x, y)));

            jugadasMaquina.Add((x, y));
            string resultado = jugadorTablero.Jugada(x, y);

            foreach (var view in TableroJugadorGrid.Children)
            {
                if (view is Button btn && Grid.GetRow(btn) == x && Grid.GetColumn(btn) == y)
                {
                    if (resultado == "Agua")
                        btn.BackgroundColor = Colors.LightBlue;
                    else if (resultado == "Tocado")
                        btn.BackgroundColor = Colors.Red;
                    else if (resultado == "Partida finalizada")
                    {
                        btn.BackgroundColor = Colors.Black;
                        DisplayAlert("¡Derrota!", "La máquina ha hundido todos tus barcos.", "OK");
                    }
                    break;
                }
            }
        }

        private void ActualizarContadores()
        {
            AciertosLabel.Text = $"Aciertos: {aciertos}";
            FallosLabel.Text = $"Fallos: {fallos}";
            DisparosLabel.Text = $"Disparos: {Disparos}";
        }

        private void NuevaPartida_Clicked(object sender, EventArgs e)
        {
            NuevaPartida();
        }
    }
}