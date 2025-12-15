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

            Stopwatch chronoTour = Stopwatch.StartNew();

            Console.WriteLine($"Temps restant : {tempsParTour.Seconds} secondes");
            Console.Write("Propose un mot (ENTER pour passer) : ");

            while (chronoTour.Elapsed < tempsParTour)
            {
                if (!Console.KeyAvailable)
                    continue;

                string mot = Console.ReadLine();

                // ➜ Passe volontairement
                if (string.IsNullOrEmpty(mot))
                    return;

                mot = mot.Trim().ToUpper();

                //  ERREUR → joueur suivant
                if (mot.Length < 2)
                {
                    MessageErreur();
                    return;
                }

                if (joueur.Contient(mot))
                {
                    MessageErreur();
                    return;
                }

                if (!dictionnaire.RechercheDicho(mot))
                {
                    MessageErreur();
                    return;
                }

                object res = plateau.Recherche_Mot(mot);
                if (res == null)
                {
                    MessageErreur();
                    return;
                }

                //  MOT VALIDE
                joueur.AddMot(mot);
                int score = mot.Length * mot.Length;
                joueur.AddScore(score);

                plateau.Maj_Plateau(res);

                Console.WriteLine($"Mot accepté ! +{score} points");
                Console.ReadKey();
                return;
            }

            //  Temps écoulé
            Console.WriteLine("Temps écoulé !");
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


