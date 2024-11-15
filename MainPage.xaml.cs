using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Maui.Controls;


namespace ListView_zadanie 
{
    public partial class MainPage : ContentPage //główna klasa
    {
        public List<User> Users { get; set; }  //lista użytkowników

        public class User //klasa użytkownika
        {
                //właściwości od: 
            public string Imie { get; set; } //imienia i nazwiska
            public string Telefon { get; set; } //numeru telefonu
            public string Email { get; set; }  //emaila

            
            public User(string imie, string telefon, string email) //konstruktor klasy user przypisujący to co widać 
            {
                Imie = imie;
                Telefon = telefon;
                Email = email;
            }
        }

        public MainPage() 
        { 
            InitializeComponent();   //wczytanie xaml 
            Users = ReadFile(); //załadoweanie danych z pliku
            userListView.ItemsSource = Users; //przypisanie listy użytkowników do listview 
        }

        private List<User> ReadFile()  // ta metoda odczytuje plik i tworzy liste użytkowników
        {
            var users = new List<User>();   //tu tworzy się pusta lista użytkowników

            try  //by obsłużyć wyjątek
            {
                //wczytanie danych z pliku. Asynchronicznie bo inaczej może sie kod wywalać. Result blokuje wykonanie kodu czekając aż operacja sie zakończy.
                using var stream = FileSystem.OpenAppPackageFileAsync("uzytkownicy.txt").Result; 
                using var reader = new StreamReader(stream); //tworzenie obiektu Streamreader do oczytu pliku

                string line;  //ta zmienna przechowuje każdą linię z pliku
                while ((line = reader.ReadLine()) != null) //pętla odczytująca każdą linię pliku 
                {
                    if (!string.IsNullOrWhiteSpace(line))  //czy linia nie jest pusta
                    {
                        var parts = line.Split(' '); //to dzieli linie na części, dokładniej to 4 bo jest imie, nazwisko, telefon i email

                        if (parts.Length >= 4) //sprawdzenie czy linia ma te wyżej wspomniane 4 części 
                        {
                            string imie = $"{parts[0]} {parts[1]}"; //przypisanie imienia do pierwszych 2 części
                            string telefon = parts[2]; //do trzeciej telefon
                            string email = string.Join(" ", parts[3..]); // a tu email 

                            users.Add(new User(imie, telefon, email)); //tu dodaje sie nowy użytkownik do listy, użytkownik zawiera informacje z jednej linii
                        }
                    }
                }
            }
            catch (Exception ex) //obsługa błędów
            {
                Console.WriteLine($"Wystąpił błąd: {ex.Message}"); //wyświetla komunikat o błędzie jeśli coś jest nie tak 
            }

            return users; //zwraca liste
        }
    }
}


//sposób zaczytywania pliku został zmieniony, gdyż tamten podany w zadaniu nie działal. Ten którego użyłem znalazłem na https://stackoverflow.com/questions/7387085/how-to-read-an-entire-file-to-a-string-using-c
