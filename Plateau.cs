using System;
using System.Collections.Generic;
using System.IO;

namespace Mots_glissés
{
    internal class Plateau
    {
        private string[,] lettres;

        // ==========================
        // CONSTRUCTEURS
        // ==========================

        // Plateau fourni (tests, fichiers)
        public Plateau(string[,] l)
        {
            lettres = l;
        }

        // Plateau vide (tests)
        public Plateau()
        {
        }

        // ✅ Plateau aléatoire (OBLIGATOIRE SELON LE SUJET)
        public Plateau(int lignes, int colonnes)
        {
            lettres = new string[lignes, colonnes];
            GenererPlateauAleatoire();
        }

        // ==========================
        // PROPRIÉTÉS
        // ==========================
        public string[,] Lettres
        {
            get { return lettres; }
        }

        public int NbLignes
        {
            get { return lettres.GetLength(0); }
        }

        public int NbColonnes
        {
            get { return lettres.GetLength(1); }
        }

        // ==========================
        // GÉNÉRATION ALÉATOIRE
        // ==========================
        private void GenererPlateauAleatoire()
        {
            List<string> sac = new List<string>();

            // Lecture du fichier Lettres.txt
            // Format : A,10,1
            string[] lignes = File.ReadAllLines("Lettres.txt");

            foreach (string ligne in lignes)
            {
                string[] parts = ligne.Split(',');
                string lettre = parts[0];
                int maxOcc = int.Parse(parts[1]);

                for (int i = 0; i < maxOcc; i++)
                    sac.Add(lettre);
            }

            Random rnd = new Random();

            for (int i = 0; i < NbLignes; i++)
            {
                for (int j = 0; j < NbColonnes; j++)
                {
                    int index = rnd.Next(sac.Count);
                    lettres[i, j] = sac[index];
                    sac.RemoveAt(index);
                }
            }
        }

        // ==========================
        // AFFICHAGE
        // ==========================
        public override string ToString()
        {
            string s = "";

            for (int i = 0; i < NbLignes; i++)
            {
                for (int j = 0; j < NbColonnes; j++)
                {
                    s += (string.IsNullOrEmpty(lettres[i, j]) ? "." : lettres[i, j]) + " ";
                }
                s += "\n";
            }
            return s;
        }

        // ==========================
        // FIN DE PARTIE
        // ==========================
        public bool EstVide()
        {
            for (int i = 0; i < NbLignes; i++)
                for (int j = 0; j < NbColonnes; j++)
                    if (!string.IsNullOrEmpty(lettres[i, j]))
                        return false;

            return true;
        }

        // ==========================
        // RECHERCHE DU MOT
        // ==========================
        public bool Recherche_Mot(string mot)
        {
            int ligneBase = NbLignes - 1;

            for (int j = 0; j < NbColonnes; j++)
            {
                if (lettres[ligneBase, j] == mot[0].ToString())
                {
                    if (RechercheRec(ligneBase, j, mot, 0))
                        return true;
                }
            }
            return false;
        }

        private bool RechercheRec(int i, int j, string mot, int index)
        {
            if (index == mot.Length)
                return true;

            if (i < 0 || j < 0 || j >= NbColonnes)
                return false;

            if (lettres[i, j] != mot[index].ToString())
                return false;

            // Vertical (haut)
            if (RechercheRec(i - 1, j, mot, index + 1))
                return true;

            // Gauche
            if (RechercheRec(i, j - 1, mot, index + 1))
                return true;

            // Droite
            if (RechercheRec(i, j + 1, mot, index + 1))
                return true;

            return false;
        }

        // ==========================
        // GLISSEMENT DES LETTRES
        // ==========================
        public void AppliquerGlissement(string mot)
        {
            // Suppression des lettres du mot
            for (int i = 0; i < NbLignes; i++)
            {
                for (int j = 0; j < NbColonnes; j++)
                {
                    if (!string.IsNullOrEmpty(lettres[i, j]) && mot.Contains(lettres[i, j]))
                        lettres[i, j] = "";
                }
            }

            // Glissement colonne par colonne
            for (int j = 0; j < NbColonnes; j++)
            {
                List<string> colonne = new List<string>();

                for (int i = 0; i < NbLignes; i++)
                {
                    if (!string.IsNullOrEmpty(lettres[i, j]))
                        colonne.Add(lettres[i, j]);
                }

                int index = NbLignes - 1;

                for (int k = colonne.Count - 1; k >= 0; k--)
                {
                    lettres[index, j] = colonne[k];
                    index--;
                }

                while (index >= 0)
                {
                    lettres[index, j] = "";
                    index--;
                }
            }
        }
    }
}
