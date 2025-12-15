using System;
using System.Collections.Generic;
using System.IO;

namespace Mots_glissés
{
     internal class Plateau
 {
     private string[,] lettres;
     private int lignes;
     private int colonnes;

     /// <summary>
     /// Constructeur
     /// </summary>
     /// <param name="grille"></param>
     public Plateau(string[,] grille)
     {
         lettres = grille;
         lignes = grille.GetLength(0);
         colonnes = grille.GetLength(1);
     }

     /// <summary>
     /// Accesseur
     /// </summary>
     public string[,] Lettres
     {
         get { return lettres; }
     }

     
     /// <summary>
     /// Méthode ToString() d'affichage du plateau
     /// </summary>
     /// <returns></returns>
     
     public override string ToString()
     {
         string res = "";
         for (int i = 0; i < lignes; i++)
         {
             for (int j = 0; j < colonnes; j++)
             {
                 res += lettres[i, j] + " ";
             }
             res += "\n";
         }
         return res;
     }

     /// <summary>
     /// Sauvegarde SIMPLE (pas de contrôle)
     /// </summary>
     /// <param name="nomFile"></param>
     public void ToFile(string nomFile)
     {
         System.IO.StreamWriter sw = new System.IO.StreamWriter(nomFile);

         for (int i = 0; i < lignes; i++)
         {
             for (int j = 0; j < colonnes; j++)
             {
                 sw.Write(lettres[i, j]);
                 if (j < colonnes - 1)
                     sw.Write(";");
             }
             sw.WriteLine();
         }

         sw.Close();
     }

     /// <summary>
     /// Lecture SIMPLE
     /// </summary>
     /// <param name="grille"></param>
     public void ToRead(string[,] grille)
     {
         lettres = grille;
         lignes = grille.GetLength(0);
         colonnes = grille.GetLength(1);
     }

     /// <summary>
     /// Recherche d’un mot (vertical uniquement, depuis le bas)
     /// </summary>
     /// <param name="mot"></param>
     /// <returns></returns>
     public object Recherche_Mot(string mot)
     {
         mot = mot.ToUpper();

         for (int col = 0; col < colonnes; col++)
         {
             int indexMot = 0;

             for (int lig = lignes - 1; lig >= 0; lig--)
             {
                 if (lettres[lig, col] == mot[indexMot].ToString())
                 {
                     indexMot++;

                     if (indexMot == mot.Length)
                     {
                         // Mot trouvé → on retourne la colonne
                         return col;
                     }
                 }
                 else
                 {
                     indexMot = 0;
                 }
             }
         }

         return null; /// mot non trouvé
     }

     /// <summary>
     /// Mise à jour du plateau (efface une colonne trouvée)
     /// </summary>
     /// <param name="resultat"></param>
     public void Maj_Plateau(object resultat)
     {
         if (resultat == null)
             return;

         int col = (int)resultat;

         /// Décalage vers le bas
         for (int i = lignes - 1; i > 0; i--)
         {
             lettres[i, col] = lettres[i - 1, col];
         }

         /// Case du haut vide
         lettres[0, col] = " ";
     }
 }
    
    
}

