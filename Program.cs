using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

class AlkuaineTesti
{
    static void Main()
    {
        // Pääsilmukka, joka toistaa ohjelman päävalikkoa
        while (true)
        {
            // Näytetään päävalikon vaihtoehdot
            Console.WriteLine();
            Console.WriteLine("***** Alkuaine Peli - Final boss edition *****");
            Console.WriteLine("Pelaa Alkuaine peliä painamalla (p)");
            Console.WriteLine("Tarkastele tuloksia painamalla (t)");
            Console.WriteLine("Lopeta ohjelma painamalla (q)");
            Console.WriteLine("----------------------------------------------");

            // Luetaan käyttäjän valinta ja muutetaan se pieniksi kirjaimiksi
            string? valinta = Console.ReadLine()?.ToLower();

            // Tarkistetaan, että valinta ei ole null
            if (valinta == null)
            {
                Console.WriteLine("Virheellinen valinta. Yritä uudelleen.");
                continue;
            }

            // Suoritetaan käyttäjän valinnan mukaan
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
                break; // Lopetetaan ohjelma
            }
            else
            {
                Console.WriteLine("Virheellinen valinta. Yritä uudelleen.");
            }
        }
    }

    // Metodi pelin pelaamiseen
    static void PelaaPelia()
    {
        // Luetaan alkuaineiden tiedot tiedostosta
        var alkuaineTiedot = LueAlkuaineTiedot("alkuaineet.txt");

        // Satunnaisesti valitut 5 kysymystä kaikista alkuaineista
        List<string> kysyttavatAlkuaineet = alkuaineTiedot.Keys
                                        .OrderBy(x => Guid.NewGuid()) // Satunnaistetaan kysymykset
                                        .Take(5) // Otetaan 5 kysymystä
                                        .ToList();

        int oikeinMenneet = 0; // Lasketaan oikein menneet vastaukset
        // Listat oikeista ja vääristä vastauksista
        List<(string alkuaine, string oikeaLyhenne)> oikeatVastaukset = new List<(string, string)>();
        List<(string alkuaine, string oikeaLyhenne)> vaaratVastaukset = new List<(string, string)>();

        // Käydään läpi kysyttävät alkuaineet
        foreach (string alkuaineNimi in kysyttavatAlkuaineet)
        {
            // Kysytään kemiallista merkkiä
            Console.WriteLine($"Mikä on alkuaineen {alkuaineNimi} kemiallinen merkki?");
            string vastaus = Console.ReadLine()?.Trim() ?? string.Empty;

            // Tarkistetaan vastaus
            if (alkuaineTiedot != null && alkuaineTiedot.TryGetValue(alkuaineNimi, out string? alkuaineLyhenne))
            {
                if (alkuaineLyhenne != null && StringComparer.OrdinalIgnoreCase.Compare(vastaus, alkuaineLyhenne) == 0)
                {
                    oikeinMenneet++; // Lisää oikeiden vastausten laskuriin
                    oikeatVastaukset.Add((alkuaineNimi, alkuaineLyhenne)); // Tallenna oikea vastaus
                }
                else
                {
                    // Tarkistetaan, että alkuaineLyhenne ei ole null ennen lisäämistä listalle
                    if (alkuaineLyhenne != null)
                    {
                        vaaratVastaukset.Add((alkuaineNimi, alkuaineLyhenne)); // Tallenna väärä vastaus
                    }
                }
            }
            else
            {
                Console.WriteLine($"Alkuaainetta {alkuaineNimi} ei löytynyt tiedoista.");
            }
        }

        // Tulostetaan tulokset
        Console.WriteLine($"Sait {oikeinMenneet} oikein ja {5 - oikeinMenneet} väärin.");

        // Näytetään oikeat vastaukset
        if (oikeatVastaukset.Count > 0)
        {
            Console.WriteLine("Oikeat vastaukset:");
            foreach (var (alkuaine, oikeaLyhenne) in oikeatVastaukset)
            {
                Console.WriteLine($"- {alkuaine} ({oikeaLyhenne})");
            }
        }

        // Näytetään väärät vastaukset
        if (vaaratVastaukset.Count > 0)
        {
            Console.WriteLine("Väärät vastaukset:");
            foreach (var (alkuaine, oikeaLyhenne) in vaaratVastaukset)
            {
                Console.WriteLine($"- {alkuaine} (oikea lyhenne: {oikeaLyhenne})");
            }
        }

        // Tallennetaan tulos JSON-tiedostoon
        TallennaTulos(oikeinMenneet);
    }

    // Metodi alkuaineiden tietojen lukemiseen tiedostosta
    static Dictionary<string, string> LueAlkuaineTiedot(string tiedostoPolku)
    {
        var alkuaineTiedot = new Dictionary<string, string>();

        // Tarkistetaan tiedoston olemassaolo
        if (!File.Exists(tiedostoPolku))
        {
            Console.WriteLine("Virhe: Tiedostoa ei löydy.");
            return new Dictionary<string, string>(); // Palautetaan tyhjän sanakirjan
        }

        // Luetaan tiedoston rivit
        var rivit = File.ReadLines(tiedostoPolku).ToList();

        // Tarkistetaan, että rivimäärä on parillinen
        if (rivit.Count % 2 != 0)
        {
            Console.WriteLine("Virhe: Tiedostossa ei ole parillista määrää rivejä.");
            Environment.Exit(1); // Lopetetaan ohjelma virhetilanteessa
        }

        // Täytetään sanakirja alkuaineiden nimillä ja lyhenteillä
        for (int i = 0; i < rivit.Count; i += 2)
        {
            alkuaineTiedot[rivit[i]] = rivit[i + 1];
        }

        return alkuaineTiedot;
    }

    // Metodi tulosten tallentamiseen
    static void TallennaTulos(int oikeinMenneet)
    {
        // Määritetään hakemiston nimi päivämäärän mukaan
        string hakemisto = DateTime.Now.ToString("ddMMyyyy");
        string tiedostoPolku = Path.Combine(hakemisto, "tulokset.json");

        // Luodaan hakemisto tarvittaessa
        Directory.CreateDirectory(hakemisto);

        List<int> tulokset = new List<int>();
        if (File.Exists(tiedostoPolku))
        {
            // Luetaan olemassa olevat tulokset JSON-tiedostosta
            string json = File.ReadAllText(tiedostoPolku);
            tulokset = JsonConvert.DeserializeObject<List<int>>(json) ?? new List<int>(); // Lisätty null-coalescing operaatio
        }

        // Lisätään uusi tulos
        tulokset.Add(oikeinMenneet);

        // Kirjoitetaan tulokset takaisin JSON-tiedostoon
        string uusiJson = JsonConvert.SerializeObject(tulokset, Formatting.Indented);
        File.WriteAllText(tiedostoPolku, uusiJson);
    }

    // Metodi tulosten tarkasteluun
    static void TarkasteleTuloksia()
    {
        List<int> kaikkiTulokset = new List<int>();
        int? viimeisinTulos = null;

        // Käydään läpi kaikki hakemistot ja etsitään tulokset
        foreach (string hakemisto in Directory.GetDirectories("."))
        {
            string tiedostoPolku = Path.Combine(hakemisto, "tulokset.json");
            if (File.Exists(tiedostoPolku))
            {
                // Luetaan JSON-tiedosto ja deserialisoidaan tulokset
                string json = File.ReadAllText(tiedostoPolku);
                List<int>? tulokset = JsonConvert.DeserializeObject<List<int>>(json);
                if (tulokset != null && tulokset.Count > 0)
                {
                    kaikkiTulokset.AddRange(tulokset); // Lisätään tulokset listalle
                    viimeisinTulos = tulokset.Last(); // Päivitetään viimeisin tulos
                }
            }
        }

        // Näytetään tulokset
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
