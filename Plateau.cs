namespace Mots_glissés
{
    internal class Plateau
    {
        string[,] lettres;

        public Plateau(string[,] l)
        {
            this.lettres = l;
        }
        public Plateau ()
        {

        }
        public string[,] Lettres { get { return lettres; } }

    }
}
