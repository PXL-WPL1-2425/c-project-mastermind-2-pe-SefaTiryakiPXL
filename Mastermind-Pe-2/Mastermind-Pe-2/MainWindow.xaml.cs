using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mastermind_Pe_2
{
    public partial class MainWindow : Window
    {
        private string TitelAppears1;
        private string TitelAppears2;
        private string TitelAppears3;
        private string TitelAppears4;
        string Titel;
        int attempts = 0;
        int score = 100;
        List<List<string>> Historiek = new List<List<string>>();
        public MainWindow()
        {
            InitializeComponent();
            TitelAppearsAbove();
            // array 
            string[] colors = { "rood", "geel", "groen", "oranje", "wit", "blauw" };
            foreach (var comboBox in new[] { ComboBox1, ComboBox2, ComboBox3, ComboBox4 })
            {
                foreach (var color in colors)
                {
                    // voegt voor elke combobox de naam color
                    comboBox.Items.Add(color);
                }
            }
        }
        private void TitelAppearsAbove()
        {
            Random rnd = new Random();
            string[] TitelAppears = new string[] { "rood", "geel", "groen", "oranje", "wit", "blauw" };
            TitelAppears1 = TitelAppears[rnd.Next(0, TitelAppears.Length)];
            TitelAppears2 = TitelAppears[rnd.Next(0, TitelAppears.Length)];
            TitelAppears3 = TitelAppears[rnd.Next(0, TitelAppears.Length)];
            TitelAppears4 = TitelAppears[rnd.Next(0, TitelAppears.Length)];
            Titel = $"{TitelAppears1}, {TitelAppears2}, {TitelAppears3}, {TitelAppears4}";
            this.Title = $"Mastermind ({Titel})";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string kleur1 = ComboBox1.SelectedItem?.ToString();
            string kleur2 = ComboBox2.SelectedItem?.ToString();
            string kleur3 = ComboBox3.SelectedItem?.ToString();
            string kleur4 = ComboBox4.SelectedItem?.ToString();
            attempts++;
            this.Title = $"Mastermind - Poging {attempts}";

            if (kleur1 == TitelAppears1 && kleur2 == TitelAppears2 && kleur3 == TitelAppears3 && kleur4 == TitelAppears4)
            {
                MessageBox.Show($"juist!! In {attempts} pogingen");
                AskToReplay();
                return;
            }
            if (attempts >= 10)
            {
                MessageBox.Show($"Gefaald!! De code is: {Titel}");
                AskToReplay();
                return;
            }
            int scorePenalty = 0;
            string feedback = "";
            string[] correctCode = { TitelAppears1, TitelAppears2, TitelAppears3, TitelAppears4 };
            string[] gokken = { kleur1, kleur2, kleur3, kleur4 };
            for (int i = 0; i < 4; i++)
            {
                if (gokken[i] == correctCode[i])
                {
                    SetBorderColor(i, Brushes.DarkRed);
                    feedback += "J "; // Juiste
                }
                else if (correctCode.Contains(gokken[i]))
                {
                    SetBorderColor(i, Brushes.Wheat);
                    scorePenalty += 1;
                    feedback += "FP "; // verkeerde plaats
                }
                else
                {
                    feedback += "F "; // Fout
                    scorePenalty += 2;
                }
            }
            score -= scorePenalty;
            if (score < 0)
            {
                score = 0;
            }
            List<string> currentAttempt = new List<string>
            {
                kleur1,
                kleur2,
                kleur3,
                kleur4,
                feedback
            };
            Historiek.Add(currentAttempt);
            ListBoxHistoriek.Items.Clear();
            for (int i = 0; i < Historiek.Count; i++)
            {
                string feedbackString = $"{Historiek[i][0]} ,{Historiek[i][1]} ,{Historiek[i][2]} ,{Historiek[i][3]} -> {Historiek[i][4]}";
                ListBoxHistoriek.Items.Add(feedbackString);
            }
            Score.Content = $"Score: {score} strafpunten";
        }
        private void AskToReplay()
        {
            MessageBoxResult result = MessageBox.Show("Wil je opnieuw spelen?", "Spel afgelopen", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                reset();
            }
            else
            {
                MessageBox.Show("Je kunt het spel afsluiten wanneer je wilt.");
            }
        }
        private void reset()
        {
            attempts = 0;
            score = 100;
            ListBoxHistoriek.Items.Clear();
            TitelAppearsAbove();
            ResetBorder();
            ComboBox1.SelectedIndex = -1;
            ComboBox2.SelectedIndex = -1;
            ComboBox3.SelectedIndex = -1;
            ComboBox4.SelectedIndex = -1;
            this.Title = $"Mastermind - Nieuwe code: {Titel}";
        }
        private void ResetBorder()
        {
            kleur1Border.BorderBrush = Brushes.Gray;
            kleur2Border.BorderBrush = Brushes.Gray;
            kleur3Border.BorderBrush = Brushes.Gray;
            kleur4Border.BorderBrush = Brushes.Gray;
        }
        private void SetBorderColor(int index, Brush color)
        {
            switch (index)
            {
                case 0:
                    kleur1Border.BorderBrush = color;
                    break;
                case 1:
                    kleur2Border.BorderBrush = color;
                    break;
                case 2:
                    kleur3Border.BorderBrush = color;
                    break;
                case 3:
                    kleur4Border.BorderBrush = color;
                    break;
            }
        }
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is string kleur)
            {
                switch (comboBox.Name)
                {
                    case "ComboBox1":
                        Kleur1.Background = GetColor(kleur);
                        TextBlock1.Text = $"Gekozen kleur: {kleur}";
                        break;
                    case "ComboBox2":
                        Kleur2.Background = GetColor(kleur);
                        TextBlock2.Text = $"Gekozen kleur: {kleur}";
                        break;
                    case "ComboBox3":
                        Kleur3.Background = GetColor(kleur);
                        TextBlock3.Text = $"Gekozen kleur: {kleur}";
                        break;
                    case "ComboBox4":
                        Kleur4.Background = GetColor(kleur);
                        TextBlock4.Text = $"Gekozen kleur: {kleur}";
                        break;
                }
            }
        }
        private Brush GetColor(string kleur)
        {
            switch (kleur.ToLower())
            {
                case "rood":
                    return Brushes.Red;
                case "geel":
                    return Brushes.Yellow;
                case "groen":
                    return Brushes.Green;
                case "oranje":
                    return Brushes.Orange;
                case "wit":
                    return Brushes.White;
                case "blauw":
                    return Brushes.Blue;
                default:
                    return Brushes.Transparent;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Weet je zeker dat je wilt afsluiten?", "Afsluiten", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
        private void Afsluiten_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Wil je afsluiten", "Afsluiten", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();  // Sluit de applicatie af
            }
        }
        private void AantalPogingen_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Aantal pogingen: {attempts}", "Instellingen");
        }
        private void NieuwSpel_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }
        private void Highscores_Click(object sender, RoutedEventArgs e)
        {
            string highscores = "Ahmed -7 pogingen - 42/100\n" + "Piet -5 pogingen - 61/100\n"+
                                "Senne -8 pogingen - 17/100\n" + "Suyen -3 pogingen - 88/100\n";

            MessageBox.Show(highscores, "Mastermind Highscores:");
        }
    }
}