using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Mots_glissés
{
    internal class Plateau
    {
        private string[,] lettres;
        private int lignes;
        private int colonnes;


        /// Constructeur du plateau à partir d'une grille existante.
        public Plateau(string[,] grille)
        {
            lettres = grille;
            lignes = grille.GetLength(0);
            colonnes = grille.GetLength(1);
        }

        /// Constructeur aléatoire
        public Plateau(int l, int c)
        {
            Random rnd = new Random();

            /// Initialisation matrice
            lignes = l;
            colonnes = c;
            lettres = new string[lignes, colonnes];

            List<string> sacDeLettres = new List<string>();
            StreamReader sr = null;

            ///Lecture de Lettres.txt
            try
            {

                sr = new StreamReader("Lettre.txt");
                string ligne;
                while ((ligne = sr.ReadLine()) != null)
                {
                    string[] parts = ligne.Split(',');

                    string lettre = parts[0];
                    int nbMax = int.Parse(parts[1]);

                    /// ajout de chaque lettre (2xnombre) dans le sac de lettres
                    for (int i = 0; i < nbMax; i++)
                    {
                        sacDeLettres.Add(lettre);
                        sacDeLettres.Add(lettre);
                    }
                }

            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (sr != null) sr.Close();
            }

            /// Remplissage de la matrice en piochant dans le sac de lettres
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    if (sacDeLettres.Count == 0)
                    {
                        lettres[i, j] = " ";
                    }
                    else
                    {
                        int index = rnd.Next(sacDeLettres.Count);
                        lettres[i, j] = sacDeLettres[index];
                        sacDeLettres.RemoveAt(index);
                    }
                }
            }
        }


        /// Constructeur a partir d'un fichier
        public Plateau (string nomFile)
        {
            ToRead(nomFile);
        }

        /// Accès à la matrice.
        public string[,] Lettres { get { return lettres; } }


        /// Affichage du plateau.
        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                    res += lettres[i, j] + " ";
                res += "\n";
            }
            return res;
        }


        /// Sauvegarde CSV (séparateur ';').
        public void ToFile(string nomFile)
        {
            StreamWriter sw = new StreamWriter(nomFile);
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    sw.Write(lettres[i, j]);
                    if (j < colonnes - 1) sw.Write(";");
                }
                sw.WriteLine();
            }
            sw.Close();
        }


        /// Lecture Fichier .txt ou .csv (séparateur ';' ou ' ').
        public void ToRead(string nomFile)
        {
            // 1) Compter lignes/colonnes
            StreamReader sr = new StreamReader(nomFile);
            string line = sr.ReadLine();
            if (line == null) { sr.Close(); return; }

            colonnes = 1;
            for (int k = 0; k < line.Length; k++)
                if (line[k] == ';'|| line[k]==' ') colonnes++;

            lignes = 1;
            while ((line = sr.ReadLine()) != null) lignes++;
            sr.Close();

            lettres = new string[lignes, colonnes];

            // 2) Remplir
            sr = new StreamReader(nomFile);
            int i = 0;
            while ((line = sr.ReadLine()) != null)
            {
                int j = 0;
                string cell = "";

                for (int k = 0; k < line.Length; k++)
                {
                    if (line[k] == ';'|| line[k]==' ')
                    {
                        lettres[i, j] = cell;
                        cell = "";
                        j++;
                    }
                    else cell += line[k];
                }
                lettres[i, j] = cell;
                i++;
            }
            sr.Close();
        }

        /// Résultat d'une recherche : positions (ligne,colonne) de chaque lettre du mot.
        public class ResultatMot
        {
            public int[] Ligs;
            public int[] Cols;

            public ResultatMot(int n)
            {
                Ligs = new int[n];
                Cols = new int[n];
            }
        }


        /// Recherche un mot en partant OBLIGATOIREMENT de la dernière ligne.
        /// Déplacements autorisés : gauche, droite, haut (pas diagonale).
        /// Lettres collées, sans réutiliser une case.

        public object Recherche_Mot(string mot)
        {
            if (mot == null) return null;
            mot = mot.Trim().ToUpper();
            if (mot.Length < 2) return null;

            int baseLig = lignes - 1;

            // On essaie toutes les colonnes de la base comme point de départ
            for (int startCol = 0; startCol < colonnes; startCol++)
            {
                if (lettres[baseLig, startCol].ToUpper() != mot[0].ToString()) continue;

                bool[,] visite = new bool[lignes, colonnes];
                ResultatMot res = new ResultatMot(mot.Length);

                if (RechRec(mot, 0, baseLig, startCol, visite, res))
                    return res;
            }

            return null;
        }


        /// Recherche récursive simple

        private bool RechRec(string mot, int idx, int lig, int col, bool[,] visite, ResultatMot res)
        {
            // Vérif lettre
            if (lettres[lig, col].ToUpper() != mot[idx].ToString())
                return false;

            // Prendre la case
            visite[lig, col] = true;
            res.Ligs[idx] = lig;
            res.Cols[idx] = col;

            // Fin ?
            if (idx == mot.Length - 1)
                return true;

            int nextIdx = idx + 1;

            // 1) Haut
            if (lig - 1 >= 0 && !visite[lig - 1, col])
                if (RechRec(mot, nextIdx, lig - 1, col, visite, res)) return true;

            // 2) Gauche
            if (col - 1 >= 0 && !visite[lig, col - 1])
                if (RechRec(mot, nextIdx, lig, col - 1, visite, res)) return true;

            // 3) Droite
            if (col + 1 < colonnes && !visite[lig, col + 1])
                if (RechRec(mot, nextIdx, lig, col + 1, visite, res)) return true;

            // Backtrack
            visite[lig, col] = false;
            return false;
        }

        /// Supprime toutes les lettres du mot trouvé puis applique la gravité
        /// colonne par colonne sur les colonnes impactées.
        public void Maj_Plateau(object objet)
        {
            ResultatMot res = objet as ResultatMot;
            if (res == null) return;

            // Marquer les cases utilisées comme vides
            bool[] colonneImpactee = new bool[colonnes];

            for (int k = 0; k < res.Ligs.Length; k++)
            {
                int i = res.Ligs[k];
                int j = res.Cols[k];
                lettres[i, j] = " ";
                colonneImpactee[j] = true;
            }

            // Gravité colonne par colonne
            for (int col = 0; col < colonnes; col++)
            {
                if (!colonneImpactee[col]) continue;

                string[] temp = new string[lignes];
                int write = lignes - 1;

                // On récupère les lettres non vides du bas vers le haut
                for (int lig = lignes - 1; lig >= 0; lig--)
                {
                    string v = lettres[lig, col];
                    if (v != " " && v != "")
                    {
                        temp[write] = v;
                        write--;
                    }
                }

                // Compléter le reste avec des vides
                for (int t = write; t >= 0; t--)
                    temp[t] = " ";

                // Réécrire la colonne
                for (int lig = 0; lig < lignes; lig++)
                    lettres[lig, col] = temp[lig];
            }
        }
    }
}

