using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Mots_glissés
{
    internal class Joueur
    {
        string nom;
        int score;
        List<string> mots;
        
        public Joueur(string n)
        {
            this.nom = n;
            this.score=0;
            this.mots = new List<string>();
        }

        public string Nom { get { return nom; } }
        public int Score { get { return score; } }
        public List<string> Mots { get { return mots; } }

        public void AddMot(string mot)
        {
            Mots.Add(mot);
        }
        public void AddScore(int val)
        {
            score += val;
        }
        public string toString()
        {
            string s = nom+"\nscore : "+score;
            foreach (string mot in mots) s += "\n" + mot;
            return s;

        }
        public bool Contient(string n)
        {
            return mots.Contains(n);
        }
    }
}
