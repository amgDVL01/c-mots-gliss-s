using System.Diagnostics.Tracing;
using System.IO;

namespace Mots_glissés
{
    internal class Dictionnaire //validé
    {
        List<string> dico;
        public Dictionnaire() //validé
        {
            this.dico = new List<string>();
            string[] tabMots;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader("Mots_Français.txt");
                string line;
                while ((line= sr.ReadLine()) != null)
                {
                    tabMots = line.Split(' ');
                    foreach (string mot in tabMots)
                    {
                        dico.Add(mot);
                    }
                }
            }
            catch (IOException e)
            { 
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (sr!= null) sr.Close();
            }
            Tri_Fusion();
        }
        public List<string> Dico { get { return this.dico; } }
        public void Tri_Fusion()
        {
           Tri_Fusion(0, dico.Count - 1);
        }
        public void Tri_Fusion(int gauche, int droite)
        {
            if (gauche < droite)
            {
                int pivot = Partition(gauche, droite);
                Tri_Fusion(gauche, pivot - 1);
                Tri_Fusion(pivot + 1, droite);
            }
        }
        public int Partition(int gauche, int droite)
        {
            string pivot = dico[droite];
            int i = gauche - 1;

            for (int j = gauche; j < droite; j++)
            {
                if (ComparerMots(dico[j], pivot) < 0)
                {
                    i++;
                    (dico[i], dico[j]) = (dico[j], dico[i]);
                }
            }

            (dico[i + 1], dico[droite]) = (dico[droite], dico[i + 1]);
            return i + 1;
        }
        public bool RechercheDicho(string mot)
        {
            return RechercheDichoRecursif(dico, mot, 0, dico.Count()-1);
        }

        public bool RechercheDichoRecursif(List<string> mots, string mot, int debut, int fin)
        {
            if (debut > fin)
                return false;

            int milieu = (debut + fin) / 2;

            int comparaison = ComparerMots(mot, mots[milieu]);

            if (comparaison == 0)
                return true;
            else if (comparaison < 0)
                return RechercheDichoRecursif(mots, mot, debut, milieu - 1);
            else
                return RechercheDichoRecursif(mots, mot, milieu + 1, fin);
        }
        public int ComparerMots(string mot1, string mot2)
        {
            int res=0;
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (mot1.Length < mot2.Length) res=-1;
            if (mot1.Length > mot2.Length) res= 1;
            else
            {
                int i = 0, l=mot1.Length;
                while (res==0 && i<l)
                {
                    if (alphabet.IndexOf(mot1[i]) < alphabet.IndexOf(mot2[i])) res = -1;
                    if (alphabet.IndexOf(mot1[i]) > alphabet.IndexOf(mot2[i])) res = 1;
                    i++;
                }
            }
            return res;
        }
    }
}
