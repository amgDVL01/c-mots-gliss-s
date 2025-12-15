
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

        // Gestion du temps
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
            Console.Clear();
            Console.WriteLine("=== DÉBUT DE LA PARTIE ===");

            Stopwatch chronoPartie = Stopwatch.StartNew();

            while (!FinDePartie(chronoPartie))
            {
                Joueur j = joueurs[joueurCourant];
                TourDeJeu(j);

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

            Stopwatch chronoTour = Stopwatch.StartNew();

            while (chronoTour.Elapsed < tempsParTour)
            {
                Console.Write("Propose un mot (ENTER pour passer) : ");
                string mot = Console.ReadLine();

                if (string.IsNullOrEmpty(mot))
                    return;

                mot = mot.ToUpper();

                if (mot.Length < 2)
                {
                    Console.WriteLine("Mot trop court.");
                    continue;
                }

                if (joueur.Contient(mot))
                {
                    Console.WriteLine("Mot déjà trouvé.");
                    continue;
                }

                if (!dictionnaire.RechercheDicho(mot))
                {
                    Console.WriteLine("Mot absent du dictionnaire.");
                    continue;
                }

                if (!plateau.Recherche_Mot(mot))
                {
                    Console.WriteLine("Mot non présent sur le plateau.");
                    continue;
                }

                // Mot valide
                joueur.AddMot(mot);

                int score = CalculerScore(mot);
                joueur.AddScore(score);

                plateau.AppliquerGlissement(mot);

                Console.WriteLine($"Mot accepté ! +{score} points");
                Console.ReadKey();
                return;
            }
        }

        // ==========================
        // FIN DE PARTIE
        // ==========================
        private bool FinDePartie(Stopwatch chrono)
        {
            if (chrono.Elapsed >= tempsTotal)
                return true;

            if (plateau.EstVide())
                return true;

            return false;
        }

        // ==========================
        // SCORE
        // ==========================
        private int CalculerScore(string mot)
        {
            // Formellement explicable à l’oral
            return mot.Length * mot.Length;
        }

        // ==========================
        // AFFICHAGE FINAL
        // ==========================
        private void AfficherResultats()
        {
            Console.Clear();
            Console.WriteLine("=== FIN DE LA PARTIE ===\n");

            foreach (Joueur j in joueurs)
            {
                Console.WriteLine(j.toString());
                Console.WriteLine();
            }
        }
    }
}

