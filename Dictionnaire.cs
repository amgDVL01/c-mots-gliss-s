using System.Diagnostics.Tracing;
using System.IO;

namespace Mots_glissés
{
    internal class Dictionnaire
    {
        string [][] dico;
        public Dictionnaire() //validé
        {
            string [] dico1 = File.ReadAllLines("Mots_Français.txt");
            this.dico=new string[dico1.Length][];
            foreach (string mots in dico1) this.dico[Array.IndexOf(dico1, mots)] = mots.Split(' ');
        }
        public string[] DicoLettre(string mot) //validé
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return dico[alphabet.IndexOf(mot[0])];
        }
        //public string[] Tri_Fusion(string[] mots) //validé
        //{
        //    if (mots.Length <= 1) return mots;
        //    int milieu = mots.Length / 2;
        //    string[] gauche = new string[milieu];
        //    string[] droite = new string[mots.Length - milieu];
        //    Array.Copy(mots, 0, gauche, 0, milieu);
        //    Array.Copy(mots, milieu, droite, 0, mots.Length - milieu);
        //    gauche = Tri_Fusion(gauche);
        //    droite = Tri_Fusion(droite);
        //    return Fusion(gauche, droite);
        //}
        //static string[] Fusion(string[] gauche, string[] droite) //validé
        //{
        //    int i = 0;
        //    int j = 0;
        //    int k = 0;
        //    string[] resultat = new string[gauche.Length + droite.Length];

        //    while (i < gauche.Length && j < droite.Length)
        //    {
        //        if (gauche[i].Length <= droite[j].Length)
        //        {
        //            resultat[k] = gauche[i];
        //            i++;
        //        }
        //        else
        //        {
        //            resultat[k] = droite[j];
        //            j++;
        //        }
        //        k++;
        //    }
        //    while (i < gauche.Length)
        //    {
        //        resultat[k] = gauche[i];
        //        i++;
        //        k++;
        //    }

        //    while (j < droite.Length)
        //    {
        //        resultat[k] = droite[j];
        //        j++;
        //        k++;
        //    }

        //    return resultat;
        //}

        public string[] Tri_Fusion(string[] tableau)
        {
            if (tableau.Length <= 1)
                return tableau;

            int milieu = tableau.Length / 2;

            string[] gauche = tableau.Take(milieu).ToArray();
            string[] droite = tableau.Skip(milieu).ToArray();

            gauche = Tri_Fusion(gauche);
            droite = Tri_Fusion(droite);

            return Fusion(gauche, droite);
        }
        public string[] Fusion(string[] gauche, string[] droite)
        {
            string[] resultat = new string[gauche.Length + droite.Length];
            int i = 0, j = 0, k = 0;

            while (i < gauche.Length && j < droite.Length)
            {
                if (string.Compare(gauche[i], droite[j], StringComparison.Ordinal) <= 0)
                {
                    resultat[k++] = gauche[i++];
                }
                else
                {
                    resultat[k++] = droite[j++];
                }
            }

            while (i < gauche.Length)
                resultat[k++] = gauche[i++];

            while (j < droite.Length)
                resultat[k++] = droite[j++];

            return resultat;
        }
        //public bool RechercheDicho(string mot) // a verifier
        //{
        //    string[] MotsLettre = Tri_Fusion(DicoLettre(mot));
        //    return RechercheDichoRecursif(mot, 0, MotsLettre.Length-1, MotsLettre);
        //}
        //public bool RechercheDichoRecursif(string mot, int debut, int fin, string[] mots) //a verifier
        //{
        //    if (debut > fin) return false;
        //    int milieu = (debut + fin) / 2;
        //    if (mot == mots[milieu]) return true;
        //    if (mot.Length < mots[milieu].Length) return RechercheDichoRecursif(mot, debut, milieu-1, mots);
        //    else return RechercheDichoRecursif(mot, milieu+1, fin, mots);
        //}
        public bool RechercheDicho(string mot)
        {
            string[] MotsLettre = Tri_Fusion(DicoLettre(mot));
            return RechercheDichoRecursif(MotsLettre, mot, 0, MotsLettre.Length - 1);
        }

        public bool RechercheDichoRecursif(string[] mots, string mot, int debut, int fin)
        {
            if (debut > fin)
                return false;

            int milieu = (debut + fin) / 2;

            int comparaison = string.Compare(mot, mots[milieu], StringComparison.Ordinal);

            if (comparaison == 0)
                return true;
            else if (comparaison < 0)
                return RechercheDichoRecursif(mots, mot, debut, milieu - 1);
            else
                return RechercheDichoRecursif(mots, mot, milieu + 1, fin);
        }
    }
}
