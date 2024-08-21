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
            Console.WriteLine();
            Console.WriteLine("***** Alkuaine Peli - Final boss edition *****");
            Console.WriteLine("Pelaa Alkuaine peliä painamalla (p)");
            Console.WriteLine("Tarkastele tuloksia painamalla (t)");
            Console.WriteLine("Lopeta ohjelma painamalla (q)");
            Console.WriteLine("----------------------------------------------");

            string? valinta = Console.ReadLine()?.ToLower();

            if (valinta == null)
            {
                Console.WriteLine("Virheellinen valinta. Yritä uudelleen.");
                continue;
            }

            if (valinta == "p")
            {
                PelaaPelia();
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

    static void PelaaPelia()
    {
        var alkuaineTiedot = LueAlkuaineTiedot("alkuaineet.txt");

        // Satunnaisesti valitut 5 kysymystä kaikista alkuaineista
        List<string> kysyttavatAlkuaineet = alkuaineTiedot.Keys
                                        .OrderBy(x => Guid.NewGuid())
                                        .Take(5)
                                        .ToList();

        int oikeinMenneet = 0;
        List<(string alkuaine, string oikeaLyhenne)> oikeatVastaukset = new List<(string, string)>();
        List<(string alkuaine, string oikeaLyhenne)> vaaratVastaukset = new List<(string, string)>();

        foreach (string alkuaineNimi in kysyttavatAlkuaineet)
        {
            Console.WriteLine($"Mikä on alkuaineen {alkuaineNimi} kemiallinen merkki?");
            string vastaus = Console.ReadLine()?.Trim() ?? string.Empty;

            if (alkuaineTiedot != null && alkuaineTiedot.TryGetValue(alkuaineNimi, out string? alkuaineLyhenne))
            {
                if (alkuaineLyhenne != null && StringComparer.OrdinalIgnoreCase.Compare(vastaus, alkuaineLyhenne) == 0)
                {
                    oikeinMenneet++;
                    oikeatVastaukset.Add((alkuaineNimi, alkuaineLyhenne)); // Tallenna oikea vastaus
                }
                else
                {
                    // Tarkistetaan, että alkuaineLyhenne ei ole null ennen lisäämistä listalle
                    if (alkuaineLyhenne != null)
                    {
                        vaaratVastaukset.Add((alkuaineNimi, alkuaineLyhenne));
                    }
                }
            }
            else
            {
                Console.WriteLine($"Alkuainetta {alkuaineNimi} ei löytynyt tiedoista.");
            }
        }

        Console.WriteLine($"Sait {oikeinMenneet} oikein ja {5 - oikeinMenneet} väärin.");

        if (oikeatVastaukset.Count > 0)
        {
            Console.WriteLine("Oikeat vastaukset:");
            foreach (var (alkuaine, oikeaLyhenne) in oikeatVastaukset)
            {
                Console.WriteLine($"- {alkuaine} ({oikeaLyhenne})");
            }
        }

        if (vaaratVastaukset.Count > 0)
        {
            Console.WriteLine("Väärät vastaukset:");
            foreach (var (alkuaine, oikeaLyhenne) in vaaratVastaukset)
            {
                Console.WriteLine($"- {alkuaine} ({oikeaLyhenne})");
            }
        }

        TallennaTulos(oikeinMenneet);
    }

    static Dictionary<string, string> LueAlkuaineTiedot(string tiedostoPolku)
    {
        var alkuaineTiedot = new Dictionary<string, string>();

        if (!File.Exists(tiedostoPolku))
        {
            Console.WriteLine("Virhe: Tiedostoa ei löydy.");
            return new Dictionary<string, string>(); // Palauttaa tyhjän sanakirjan.
        }

        var rivit = File.ReadLines(tiedostoPolku).ToList();

        if (rivit.Count % 2 != 0)
        {
            Console.WriteLine("Virhe: Tiedostossa ei ole parillista määrää rivejä.");
            Environment.Exit(1);
        }

        for (int i = 0; i < rivit.Count; i += 2)
        {
            alkuaineTiedot[rivit[i]] = rivit[i + 1];
        }

        return alkuaineTiedot;
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
            tulokset = JsonConvert.DeserializeObject<List<int>>(json) ?? new List<int>(); // Lisätty null-coalescing operaatio
        }

        tulokset.Add(oikeinMenneet);

        string uusiJson = JsonConvert.SerializeObject(tulokset, Formatting.Indented);
        File.WriteAllText(tiedostoPolku, uusiJson);
    }

    static void TarkasteleTuloksia()
    {
        List<int> kaikkiTulokset = new List<int>();
        int? viimeisinTulos = null;

        foreach (string hakemisto in Directory.GetDirectories("."))
        {
            string tiedostoPolku = Path.Combine(hakemisto, "tulokset.json");
            if (File.Exists(tiedostoPolku))
            {
                string json = File.ReadAllText(tiedostoPolku);
                List<int>? tulokset = JsonConvert.DeserializeObject<List<int>>(json);
                if (tulokset != null && tulokset.Count > 0)
                {
                    kaikkiTulokset.AddRange(tulokset);
                    viimeisinTulos = tulokset.Last(); // Viimeisin tulos
                }
            }
        }

        if (kaikkiTulokset.Count > 0)
        {
            double keskiarvo = kaikkiTulokset.Average();
            Console.WriteLine($"Tulosten keskiarvo: {keskiarvo:F2} / 5 ({keskiarvo / 5 * 100:F2}%)");
            Console.WriteLine($"Paras tulos: {kaikkiTulokset.Max()} / 5");
            Console.WriteLine($"Huonoin tulos: {kaikkiTulokset.Min()} / 5");
            Console.WriteLine($"Viimeisin tulos: {viimeisinTulos} / 5");
        }
        else
        {
            Console.WriteLine("Ei tuloksia saatavilla.");
        }
    }
}
