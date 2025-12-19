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

        public Jeu(Plateau p, Dictionnaire d, List<Joueur> j, int tempsTotalMinutes = 2, int tempsTourSecondes = 30)
        {
            plateau = p;
            dictionnaire = d;
            joueurs = j;

            joueurCourant = 0;

            tempsTotal = TimeSpan.FromMinutes(tempsTotalMinutes);
            tempsParTour = TimeSpan.FromSeconds(tempsTourSecondes);
        }


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


        private void TourDeJeu(Joueur joueur)
        {
            Console.Clear();
            Console.WriteLine(plateau.ToString());
            Console.WriteLine("Tour de " + joueur.Nom);
            Console.WriteLine("Temps : "+tempsParTour.TotalSeconds+ " secondes");

            Stopwatch chronoTour = Stopwatch.StartNew();
            string mot = "";

            while (true)
            {
                // Temps écoulé: passe au tour suivant
                if (chronoTour.Elapsed >= tempsParTour)
                {
                    Console.WriteLine("\nTemps écoulé !");
                    Console.ReadKey();
                    return;
                }

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();

                    // enter validation du mot
                    if (key.Key == ConsoleKey.Enter)
                        break;

                    // supprimer une lettre
                    if (key.Key == ConsoleKey.Backspace && mot.Length > 0)
                    {
                        mot = mot.Substring(0, mot.Length - 1);
                        Console.Write("\b \b");
                    }
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
                Console.ReadKey();
                return;
            }

            if (joueur.Contient(mot))
            {
                Console.WriteLine("Mot déjà trouvé.");
                Console.ReadKey();
                return;
            }

            if (!dictionnaire.RechercheDicho(mot))
            {
                Console.WriteLine("Mot absent du dictionnaire.");
                Console.ReadKey();
                return;
            }

            object res = plateau.Recherche_Mot(mot);
            if (res == null)
            {
                Console.WriteLine("Mot non présent sur le plateau.");
                Console.ReadKey();
                return;
            }


            joueur.AddMot(mot);
            int score = CalculerScore(mot);
            joueur.AddScore(score);
            plateau.Maj_Plateau(res);

            Console.WriteLine("Mot accepté ! " + score +"points");
            Console.ReadKey();
        }



        private bool FinDePartie(Stopwatch chrono)
        {
            if (chrono.Elapsed >= tempsTotal)
                return true;

            // Fin si plateau vide (plus aucune lettre)
            if (PlateauVide())
                return true;

            return false;
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


        private int CalculerScore(string mot)
        {
            return mot.Length * mot.Length;
        }

        // Affichage final
        private void AfficherResultats()
        {
            Console.Clear();
            Console.WriteLine("FIN DE LA PARTIE\n");

            foreach (Joueur j in joueurs)
            {
                Console.WriteLine(j.toString());
                Console.WriteLine();
            }
        }
    }

}
