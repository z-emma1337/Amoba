using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Amoba
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static Label label_amoba = new Label
        {
            Content = "Amőba",
            FontSize = 50,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(10)
        };
        public static Label label_tablamerete = new Label
        {
            Content = "Tábla mérete: 5",
            FontSize = 35,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(10)
        };
        public static Slider slider_tablameret = new Slider
        {
            Margin = new Thickness(0, 10, 0, 0),
            Width = 300,
            Height = 50,
            Minimum = 5,
            Maximum = 10,
            Value = 5,
            TickFrequency = 1,
            IsSnapToTickEnabled = true,


        };
        public static Button btn_2jatekos = new Button
        {
            Content = "2 játékos",
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Right,
            Padding = new Thickness(20, 10, 20, 10),
            Margin = new Thickness(40, 15, 15, 15),
        };
        public static Button btn_gepellen = new Button
        {
            Content = "1 játékos",
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Left,
            Padding = new Thickness(20, 10, 20, 10),
            Margin = new Thickness(15, 15, 40, 15),
        };
        public static Button btn_Betolt = new Button
        {
            Content = "Mentés betöltése",
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Center,
            Padding = new Thickness(20, 10, 20, 10),
            Margin = new Thickness(15),
        };
        public static bool x = true;
        public static bool o = false;
        public static Button[,] gameButtons;
        private Grid kezdolap = new Grid();
        private Grid parentGrid = new Grid();
        private Grid mentesekGrid = new Grid
        {
            Margin = new Thickness(20),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        private Grid parentMentesek = new Grid();
        public static Button UjJatek = new Button

        {
            Content = "Új játék",
            FontSize = 20,
            Name = "btn_ujjatek",
            Visibility = Visibility.Visible,
            HorizontalAlignment = HorizontalAlignment.Left,
            Padding = new Thickness(60, 10, 60, 10),
            Margin = new Thickness(15),


        };
        public static Button JatekMentese = new Button
        {
            Content = "Játék mentése",
            FontSize = 20,
            Name = "btn_jatekmentese",
            Visibility = Visibility.Visible,
            HorizontalAlignment = HorizontalAlignment.Right,
            Padding = new Thickness(30, 10, 30, 10),
            Margin = new Thickness(15),

        };
        public static Label kovetkezo = new Label
        {
            Content = "Következik: X",
            FontSize = 50,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(10)
        };
        public static MySqlConnection Connection = new MySqlConnection("SERVER=localhost;DATABASE=amoba_db;UID=root;PASSWORD=");
        private void Mentes()
        {
            Connection.Open();
            DateTime currentDate = DateTime.Now;
            string helyek = "";
            foreach (var Button in gameButtons)
            {
                if (Button.Content == "X")
                {
                    helyek = helyek + "X;";
                }
                else if (Button.Content == "O")
                {
                    helyek = helyek + "O;";
                }
                else
                {
                    helyek = helyek + "-;";
                }
            }
            MySqlCommand cmd_Mentes = new MySqlCommand($"insert into mentesek(datum,helyezes) values('{currentDate.ToString("yyyy-MM-dd H:mm:ss")}','{helyek}')", Connection);
            cmd_Mentes.ExecuteNonQuery();
            JatekMentese.IsEnabled = false;
            Connection.Close();
        }
        private void buttonsEnable()
        {
            foreach (Button button in gameButtons)
            {
                button.IsEnabled = true;
            }
        }

       private void buttonsDisable()
        {
            foreach (Button button in gameButtons)
            {
                button.IsEnabled = false;
            }
        }

        private  void CheckWin()
        {
            int rowCount = gameButtons.GetLength(0);
            int colCount = gameButtons.GetLength(1);

            // Horizontális ellenőrzés X-re
            if (!x)
            {
                for (int sor = 0; sor < rowCount; sor++)
                {
                    int db = 0;
                    for (int osz = 0; osz < colCount; osz++)
                    {
                        if (gameButtons[sor, osz].Content.ToString() == "X")
                        {
                            db++;
                            if (db == 5)
                            {
                                kovetkezo.Content = "X nyert!";
                                buttonsDisable();

                            }
                        }
                        else
                        {
                            db = 0;
                        }
                    }
                }
            }

            //Vertikális ellenőrzés X-re
            if (!x)
            {
                for (int osz = 0; osz < colCount; osz++)
                {
                    int db = 0;
                    for (int sor = 0; sor < rowCount; sor++)
                    {
                        if (gameButtons[sor, osz].Content.ToString() == "X")
                        {
                            db++;
                            if (db == 5)
                            {
                                kovetkezo.Content = "X nyert!";
                                buttonsDisable();
                            }
                        }
                        else
                        {
                            db = 0;
                        }
                    }
                }
            }

            //Átlós ellenőrzés X-re
            if (!x)
            {
                for (int sor = 0; sor <= rowCount - 5; sor++)
                {
                    for (int osz = 0; osz <= colCount - 5; osz++)
                    {
                        int db = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            if (gameButtons[sor + i, osz + i].Content.ToString() == "X")
                            {
                                db++;
                                if (db == 5)
                                {
                                    kovetkezo.Content = "X nyert!";
                                    buttonsDisable();
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            if (!x)
            {
                for (int sor = 4; sor < rowCount; sor++)
                {
                    for (int osz = 0; osz <= colCount - 5; osz++)
                    {
                        int db = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            if (gameButtons[sor - i, osz + i].Content.ToString() == "X")
                            {
                                db++;
                                if (db == 5)
                                {
                                    kovetkezo.Content = "X nyert!";
                                    buttonsDisable();
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            //-----------------------------------------------------------
            // Horizontális ellenőrzés O-ra
            if (!o)
            {
                for (int sor = 0; sor < rowCount; sor++)
                {
                    int db = 0;
                    for (int osz = 0; osz < colCount; osz++)
                    {
                        if (gameButtons[sor, osz].Content.ToString() == "O")
                        {
                            db++;
                            if (db == 5)
                            {
                                kovetkezo.Content = "O nyert!";
                                buttonsDisable();

                            }
                        }
                        else
                        {
                            db = 0;
                        }
                    }
                }
            }

            //Vertikális ellenőrzés O-ra
            if (!o)
            {
                for (int osz = 0; osz < colCount; osz++)
                {
                    int db = 0;
                    for (int sor = 0; sor < rowCount; sor++)
                    {
                        if (gameButtons[sor, osz].Content.ToString() == "O")
                        {
                            db++;
                            if (db == 5)
                            {
                                kovetkezo.Content = "O nyert!";
                                buttonsDisable();
                            }
                        }
                        else
                        {
                            db = 0;
                        }
                    }
                }
            }

            //Átlós ellenőrzés O-re
            if (!o)
            {
                for (int sor = 0; sor <= rowCount - 5; sor++)
                {
                    for (int osz = 0; osz <= colCount - 5; osz++)
                    {
                        int db = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            if (gameButtons[sor + i, osz + i].Content.ToString() == "O")
                            {
                                db++;
                                if (db == 5)
                                {
                                    kovetkezo.Content = "O nyert!";
                                    buttonsDisable();
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            if (!o)
            {
                for (int sor = 4; sor < rowCount; sor++)
                {
                    for (int osz = 0; osz <= colCount - 5; osz++)
                    {
                        int db = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            if (gameButtons[sor - i, osz + i].Content.ToString() == "O")
                            {
                                db++;
                                if (db == 5)
                                {
                                    kovetkezo.Content = "O nyert!";
                                    buttonsDisable();
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            else
            {
                bool temp = true;
                foreach (Button button in gameButtons)
                {

                    if (button.Content == "")
                    {
                        temp = false;
                    }
                }
                if (temp)
                {
                    kovetkezo.Content = "Döntetlen!";
                    buttonsDisable();
                }
            }
        }

        private void Kezdolap()
        {
            kezdolap.RowDefinitions.Clear();
            kezdolap.Children.Clear();
            kezdolap.ColumnDefinitions.Clear();


            kezdolap.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            kezdolap.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            kezdolap.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            kezdolap.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            Grid.SetRow(label_amoba, 0);
            kezdolap.Children.Add(label_amoba);

            Grid.SetRow(label_tablamerete, 1);
            kezdolap.Children.Add(label_tablamerete);

            Grid.SetRow(slider_tablameret, 2);
            kezdolap.Children.Add(slider_tablameret);

            Grid.SetRow(btn_2jatekos, 3);
            kezdolap.Children.Add(btn_2jatekos);

            Grid.SetRow(btn_gepellen, 3);
            kezdolap.Children.Add(btn_gepellen);            
            
            Grid.SetRow(btn_Betolt, 3);
            kezdolap.Children.Add(btn_Betolt);

            label_amoba.Visibility = Visibility.Visible;
            label_tablamerete.Visibility = Visibility.Visible;
            slider_tablameret.Visibility = Visibility.Visible;
            btn_gepellen.Visibility = Visibility.Visible;
            btn_2jatekos.Visibility = Visibility.Visible;

            Application.Current.MainWindow.Content = kezdolap;
            Application.Current.MainWindow.Width = 600;
            Application.Current.MainWindow.Height = 400;
            Application.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
        }
        public MainWindow()
        {
            InitializeComponent();
            Kezdolap();

            slider_tablameret.ValueChanged += slider_tablameret_Changed;
            btn_2jatekos.Click += btn_2jatekos_Click;
            btn_gepellen.Click += Btn_gepellen_Click;
            btn_Betolt.Click += Btn_Betolt_Click;
        }

        private void valasszMentest()
        {
            // Rejtett kezdőlap elemek
            label_amoba.Visibility = Visibility.Hidden;
            label_tablamerete.Visibility = Visibility.Hidden;
            slider_tablameret.Visibility = Visibility.Hidden;
            btn_gepellen.Visibility = Visibility.Hidden;
            btn_2jatekos.Visibility = Visibility.Hidden;
            btn_Betolt.Visibility = Visibility.Hidden;

            // Új listához és elrendezéshez előkészület
            List<string> datumok = new List<string>();
            Grid mentesekGrid = new Grid();
            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = mentesekGrid // ScrollViewer tartalma a Grid lesz
            };

            // Adatbázis kapcsolat és adatok lekérdezése
            Connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT datum FROM mentesek", Connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                datumok.Add(reader["datum"].ToString());
            }
            Connection.Close();

            // Grid sorainak és gombjainak létrehozása
            for (int i = 0; i < datumok.Count; i++)
            {
                Button mentesbtn = new Button
                {
                    Content = datumok[i],
                    FontSize = 35,
                    Padding = new Thickness(100,5,100,5),
                    HorizontalAlignment = HorizontalAlignment.Center
                };



                mentesekGrid.RowDefinitions.Add(new RowDefinition());
                Grid.SetRow(mentesbtn, i);
                mentesekGrid.Children.Add(mentesbtn);
            }

            // Szülő Grid megtisztítása és új elrendezés hozzáadása
            parentMentesek.Children.Clear();
            parentMentesek.Children.Add(scrollViewer);

            // Fő ablak frissítése
            Application.Current.MainWindow.Content = parentMentesek;
        }

        private void Btn_Betolt_Click(object sender, RoutedEventArgs e)
        {
            valasszMentest();
        }

        private void Btn_gepellen_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btn_2jatekos_Click(object sender, RoutedEventArgs e)
        {
            parentGrid.Children.Clear();
            parentGrid.RowDefinitions.Clear();
            parentGrid.ColumnDefinitions.Clear();


            label_amoba.Visibility = Visibility.Hidden;
            slider_tablameret.Visibility = Visibility.Hidden;
            btn_2jatekos.Visibility = Visibility.Hidden;
            btn_gepellen.Visibility = Visibility.Hidden;
            label_tablamerete.Visibility = Visibility.Hidden;

            this.Width = 100 * slider_tablameret.Value;
            this.Height = 120 * slider_tablameret.Value;



            parentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Label helye
            parentGrid.RowDefinitions.Add(new RowDefinition()); // Játék helye
            parentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });


            Grid.SetRow(kovetkezo, 0);
            parentGrid.Children.Add(kovetkezo);

            Grid.SetRow(UjJatek, 2);
            UjJatek.Click += UjJatek_Click;
            parentGrid.Children.Add(UjJatek);

            Grid.SetRow(JatekMentese, 2);
            JatekMentese.Click += JatekMentese_Click;
            parentGrid.Children.Add(JatekMentese);

            Grid gameGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 70 * slider_tablameret.Value,
                Height = 70 * slider_tablameret.Value
            };

            for (int i = 0; i < slider_tablameret.Value; i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition());
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            gameButtons = new Button[(int)slider_tablameret.Value, (int)slider_tablameret.Value];

            for (int sor = 0; sor < slider_tablameret.Value; sor++)
            {
                for (int oszlop = 0; oszlop < slider_tablameret.Value; oszlop++)
                {
                    Button gameButton = new Button
                    {
                        Content = "",
                        FontSize = 50
                    };
                    gameButton.Click += gameButton_Click;

                    gameButtons[sor, oszlop] = gameButton;

                    Grid.SetRow(gameButton, sor);
                    Grid.SetColumn(gameButton, oszlop);

                    gameGrid.Children.Add(gameButton);
                }
            }


            Grid.SetRow(gameGrid, 1);
            parentGrid.Children.Add(gameGrid);

            Application.Current.MainWindow.ResizeMode = ResizeMode.CanResize;
            this.Content = parentGrid;
            JatekMentese.IsEnabled = true;

        }

        private void JatekMentese_Click(object sender, RoutedEventArgs e)
        {
            Mentes();
        }

        private void UjJatek_Click(object sender, RoutedEventArgs e)
        {
            Kezdolap();
        }

        private void slider_tablameret_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            label_tablamerete.Content = "Tábla mérete: " + slider_tablameret.Value;
        }

        private void gameButton_Click(object sender, RoutedEventArgs e)
        {

            Button clickedButton = sender as Button;
            if (clickedButton.Content.ToString() == "")
            {
                if (x && !o)
                {
                    clickedButton.Content = "X";
                    kovetkezo.Content = "Következik: O";
                    x = false;
                    o = true;
                }
                else if (o && !x)
                {
                    clickedButton.Content = "O";
                    kovetkezo.Content = "Következik: X";
                    o = false;
                    x = true;
                }
                CheckWin();
                JatekMentese.IsEnabled = true;

            }
        }
    }
}