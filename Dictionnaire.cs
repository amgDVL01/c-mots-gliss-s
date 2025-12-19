using System.Diagnostics.Tracing;
using System.IO;

namespace Mots_glissés
{
    internal class Dictionnaire
    {
        List<string> dico;
        public Dictionnaire()
        {
            this.dico = new List<string>();
            string[] tabMots;

            
            StreamReader sr = null;
            try
            {
                ///Lecture de Mots_Francais.txt
                sr = new StreamReader("Mots_Français.txt");
                string line;
                ///Remplissage de la liste
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
        
        /// Initialisation Tri Fusion
        public void Tri_Fusion()
        {
           dico=Tri_Fusion(dico);
        }

        public List<string> Tri_Fusion(List<string> liste)
        {
            if (liste.Count <= 1)
                return liste;

            int milieu = liste.Count / 2;

            ///Séparation des 2 parties de la liste (en excluant le milieu)
            List<string> gauche = liste.GetRange(0, milieu);
            List<string> droite = liste.GetRange(milieu, liste.Count - milieu);

            /// Réitération avec les 2 moitiés
            gauche = Tri_Fusion(gauche);
            droite = Tri_Fusion(droite);

            ///Fusion des 2 moitiés (voir méthode)
            return Fusion(gauche, droite);
        }
        public List<string> Fusion(List<string> gauche, List<string> droite)
        {
            List<string> resultat = new List<string>();
            int i = 0, j = 0;

            while (i < gauche.Count && j < droite.Count)
            {
                /// Comparaison des mots courants des listes selon l'ordre alphabétique
                if (ComparerMots(gauche[i], droite[j]) <= 0)
                {
                    /// ajout du mot courant de la liste de gauche à la suite de la liste de retour et passage au mot suivant
                    resultat.Add(gauche[i]);
                    i++;
                }
                else
                {
                    /// ajout du mot courant de la liste de droite à la suite de la liste de retour et passage au mot suivant
                    resultat.Add(droite[j]);
                    j++;
                }
            }

            /// Ajout des mots restants si une des 2 listes et vidée
            while (i < gauche.Count)
                resultat.Add(gauche[i++]);

            while (j < droite.Count)
                resultat.Add(droite[j++]);

            return resultat;
        }

        /// Initialisation Recherche Dicho
        public bool RechercheDicho(string mot)
        {
            return RechercheDichoRecursif(dico, mot, 0, dico.Count()-1);
        }

        public bool RechercheDichoRecursif(List<string> mots, string mot, int debut, int fin)
        {
            /// cas négatif : le mot recherché n'est pas dans la liste
            if (debut > fin)
                return false;

            int milieu = (debut + fin) / 2;

            ///Comparaison du mot et du milieu de la liste, voir méthode
            int comparaison = ComparerMots(mot, mots[milieu]);

            /// cas positif: le mot recherché est au milieu de la liste
            if (comparaison == 0)
                return true;
            /// recherche dans la première moitié de la liste
            else if (comparaison < 0)
                return RechercheDichoRecursif(mots, mot, debut, milieu - 1);
            /// recherche dans la deuxième moitié de la liste
            else
                return RechercheDichoRecursif(mots, mot, milieu + 1, fin);
        }

        /// Comparaison de 2 mots selon l'ordre alphabétique
        /// -1 => mot1 avant mot2
        /// 0 => mot1=mot2
        /// 1 => mot2 après mot2
        public int ComparerMots(string mot1, string mot2)
        {
            int res=0;
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            /// détermination du résultat si les mots ne sont pas de même longueur
            if (mot1.Length < mot2.Length) res=-1;
            if (mot1.Length > mot2.Length) res= 1;
            ///détermination du résultat si les mots sont de même longueur lettre par lettre
            else
            {
                int i = 0, l=mot1.Length;
                while (res==0 && i<l)
                {
                    /// recherche et comparaison de la position des lettres courantes de mot1 et mot2 dans l'alphabet
                    if (alphabet.IndexOf(mot1[i]) < alphabet.IndexOf(mot2[i])) res = -1;
                    if (alphabet.IndexOf(mot1[i]) > alphabet.IndexOf(mot2[i])) res = 1;
                    i++;
                }
            }
            return res;
        }
    }
}
