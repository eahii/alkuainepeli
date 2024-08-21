using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

class AlkuaineTesti
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Haluatko pelata (p) vai tarkastella tuloksia (t)? (q lopettaa)");
            string valinta = Console.ReadLine().ToLower();

            if (valinta == "p")
            {
                PelaaTestia();
            }
            else if (valinta == "t")
            {
                TarkasteleTuloksia();
            }
            else if (valinta == "q")
            {
                break;
            }
            else
            {
                Console.WriteLine("Virheellinen valinta. Yritä uudelleen.");
            }
        }
    }

    static void PelaaTestia()
    {
        string[] alkuaineet = File.ReadAllLines("alkuaineet.txt");
        List<string> kysyttavatAlkuaineet = alkuaineet.Take(20).OrderBy(x => Guid.NewGuid()).Take(5).ToList();
        int oikeinMenneet = 0;

        foreach (string alkuaine in kysyttavatAlkuaineet)
        {
            Console.WriteLine($"Mikä on alkuaineen {alkuaine} kemiallinen merkki?");
            string vastaus = Console.ReadLine().Trim();

            if (vastaus.Equals(alkuaineet[Array.IndexOf(alkuaineet, alkuaine) + 1], StringComparison.OrdinalIgnoreCase))
            {
                oikeinMenneet++;
            }
        }

        Console.WriteLine($"Sait {oikeinMenneet} oikein ja {5 - oikeinMenneet} väärin.");
        TallennaTulos(oikeinMenneet);
    }

    static void TallennaTulos(int oikeinMenneet)
    {
        string hakemisto = DateTime.Now.ToString("ddMMyyyy");
        string tiedostoPolku = Path.Combine(hakemisto, "tulokset.json");

        Directory.CreateDirectory(hakemisto);

        List<int> tulokset = new List<int>();
        if (File.Exists(tiedostoPolku))
        {
            string json = File.ReadAllText(tiedostoPolku);
            tulokset = JsonConvert.DeserializeObject<List<int>>(json);
        }

        tulokset.Add(oikeinMenneet);

        string uusiJson = JsonConvert.SerializeObject(tulokset);
        File.WriteAllText(tiedostoPolku, uusiJson);
    }

    static void TarkasteleTuloksia()
    {
        List<int> kaikkiTulokset = new List<int>();

        foreach (string hakemisto in Directory.GetDirectories("."))
        {
            string tiedostoPolku = Path.Combine(hakemisto, "tulokset.json");
            if (File.Exists(tiedostoPolku))
            {
                string json = File.ReadAllText(tiedostoPolku);
                List<int> tulokset = JsonConvert.DeserializeObject<List<int>>(json);
                kaikkiTulokset.AddRange(tulokset);
            }
        }

        if (kaikkiTulokset.Count > 0)
        {
            double keskiarvo = kaikkiTulokset.Average();
            Console.WriteLine($"Tulosten keskiarvo: {keskiarvo:F2} / 5 ({keskiarvo / 5 * 100:F2}%)");
        }
        else
        {
            Console.WriteLine("Ei tuloksia saatavilla.");
        }
    }
}