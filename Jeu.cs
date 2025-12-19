using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mots_glissés
{
    internal class Jeu
    {
        private Plateau plateau;
        private Dictionnaire dictionnaire;
        private List<Joueur> joueurs;

        private int joueurCourant;

        private TimeSpan tempsTotal;
        private TimeSpan tempsParTour;

        public Jeu(Plateau p, Dictionnaire d, List<Joueur> j,
                   int tempsTotalMinutes = 2, int tempsTourSecondes = 30)
        {
            plateau = p;
            dictionnaire = d;
            joueurs = j;
            joueurCourant = 0;

            tempsTotal = TimeSpan.FromMinutes(tempsTotalMinutes);
            tempsParTour = TimeSpan.FromSeconds(tempsTourSecondes);
        }

        // ==========================
        // LANCEMENT DU JEU
        // ==========================
        public void Lancer()
        {
            Stopwatch chronoPartie = Stopwatch.StartNew();

            while (!FinDePartie(chronoPartie))
            {
                Joueur j = joueurs[joueurCourant];
                TourDeJeu(j);

                // joueur suivant QUOI QU’IL ARRIVE
                joueurCourant = (joueurCourant + 1) % joueurs.Count;
            }

            chronoPartie.Stop();
            AfficherResultats();
        }

        // ==========================
        // TOUR D’UN JOUEUR
        // ==========================
       private void TourDeJeu(Joueur joueur)
{
    Console.Clear();
    Console.WriteLine(plateau.ToString());
    Console.WriteLine($"Tour de {joueur.Nom}");
    Console.WriteLine($"Temps restant : {tempsParTour.TotalSeconds} secondes");

    Stopwatch chronoTour = Stopwatch.StartNew();
    string mot = "";

    while (true)
    {
        // ⏱ Temps écoulé → zappe DIRECTEMENT
        if (chronoTour.Elapsed >= tempsParTour)
        {
            Console.WriteLine("\nTemps écoulé !");
            Thread.Sleep(1000);
            return;
        }

        // Lecture NON BLOQUANTE
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            // ENTER → valider le mot
            if (key.Key == ConsoleKey.Enter)
                break;

            // BACKSPACE
            if (key.Key == ConsoleKey.Backspace && mot.Length > 0)
            {
                mot = mot.Substring(0, mot.Length - 1);
                Console.Write("\b \b");
            }
            // Lettre
            else if (char.IsLetter(key.KeyChar))
            {
                mot += char.ToUpper(key.KeyChar);
                Console.Write(key.KeyChar);
            }
        }
    }

    Console.WriteLine();

    if (string.IsNullOrEmpty(mot))
        return;

    if (mot.Length < 2)
    {
        Console.WriteLine("Mot trop court.");
        Thread.Sleep(1000);
        return;
    }

    if (joueur.Contient(mot))
    {
        Console.WriteLine("Mot déjà trouvé.");
        Thread.Sleep(1000);
        return;
    }

    if (!dictionnaire.RechercheDicho(mot))
    {
        Console.WriteLine("Mot absent du dictionnaire.");
        Thread.Sleep(1000);
        return;
    }

    object res = plateau.Recherche_Mot(mot);
    if (res == null)
    {
        Console.WriteLine("Mot non présent sur le plateau.");
        Thread.Sleep(1000);
        return;
    }


    joueur.AddMot(mot);
    int score = CalculerScore(mot);
    joueur.AddScore(score);
    plateau.Maj_Plateau(res);

    Console.WriteLine($"Mot accepté ! +{score} points");
    Console.ReadKey();
}

        // ==========================
        // FIN DE PARTIE
        // ==========================
        private bool FinDePartie(Stopwatch chrono)
        {
            if (chrono.Elapsed >= tempsTotal)
                return true;

            return PlateauVide();
        }

        private bool PlateauVide()
        {
            string[,] g = plateau.Lettres;

            for (int i = 0; i < g.GetLength(0); i++)
                for (int j = 0; j < g.GetLength(1); j++)
                    if (g[i, j] != " " && g[i, j] != "")
                        return false;

            return true;
        }

        private void AfficherResultats()
        {
            Console.Clear();
            Console.WriteLine("=== FIN DE PARTIE ===\n");

            foreach (Joueur j in joueurs)
            {
                Console.WriteLine(j.toString());
                Console.WriteLine();
            }
        }

        private void MessageErreur()
        {
            Console.WriteLine("Mot incorrect → joueur suivant");
            Console.ReadKey();
        }
    }
}




