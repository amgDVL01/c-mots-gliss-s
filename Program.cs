using System.ComponentModel;

namespace Mots_glissés
{
    internal class Program
    {
        static void Test_Joueur()
        {
            Console.WriteLine("Test_Joueur");
            Joueur arbi = new Joueur("arbi");
            arbi.AddScore(45456465);
            arbi.AddMot("AAAHDHHHABB");
            arbi.AddMot("CRACHE");
            Console.WriteLine(arbi.toString());
            Console.WriteLine(arbi.Nom + " possède le mot \"AAAHDHHHABB\" : " + arbi.Contient("AAAHDHHHABB"));
            Console.WriteLine(arbi.Nom + " possède le mot \"ENGRAINE\" : " + arbi.Contient("ENGRAINE"));
            Console.WriteLine();
        }
        static void Test_Dictionnaire()
        {
            Console.WriteLine("Test_Dictionnaire");
            Dictionnaire dico = new Dictionnaire();
            Console.WriteLine("appartenance du mot \"AFFECTIONNEE\" : " + dico.RechercheDicho("AFFECTIONNEE"));
            Console.WriteLine("appartenance du mot \"AAABBDDEABB\" : " + dico.RechercheDicho("AAABBDDEABB"));
            Console.WriteLine("appartenance du mot \"MELANGER\" : " + dico.RechercheDicho("MELANGER"));
            Console.WriteLine("Affichage des premiers mots du dictionnaire trié : ");
            foreach (string s in dico.Dico)
            {
                if (s[0] == 'A'&& s.Length<5) Console.Write(s + " ");
            }
            Console.WriteLine();
        }
        static void Test_Plateau()
        {
            Plateau p = new Plateau(10,10);

            ///Plateau aléatoire
            Console.WriteLine(p.ToString());
            Console.WriteLine();

            ///Plateau depuis fichier (séparateur ';')
            p = new Plateau("Test1.txt");
            Console.WriteLine(p.ToString());
            Console.WriteLine();

            ///Plateau depuis fichier (séparateur ' ')
            p = new Plateau("Test2.txt");
            Console.WriteLine(p.ToString());
            Console.WriteLine();
        }
        static void Test_Jeu()
        {
            List<Joueur> joueurs = new List<Joueur>();
            joueurs.Add(new Joueur("j1"));
            joueurs.Add(new Joueur("j2"));
            Jeu j = new Jeu(new Plateau("Test3.txt"), new Dictionnaire(), joueurs);
            j.Lancer();

        }
        static void InitierJeu()
        {
            Plateau p=new Plateau(0,0);
            Dictionnaire d=new Dictionnaire();

            string NomJoueur = "";
            int nbJoueurs = 0;
            List<string>ListeNoms=new List<string>();
            List<Joueur> joueurs = new List<Joueur>();

            
            ConsoleKeyInfo cki;
            bool endGame = false;
            
            while (!endGame)
            {
                Console.Clear();
                while (nbJoueurs == 0)
                {
                    Console.Write("=== MOTS GLISSÉS ===\n\nsaisir le nombre de joueurs : ");
                    try
                    {
                        nbJoueurs = Convert.ToInt32(Console.ReadLine());
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("Entrée invalide, veuillez réessayer");
                        Console.ReadKey();
                    }
                    Console.Clear();
                    Console.WriteLine("=== SAISIE DES JOUEURS ===\n\n" + nbJoueurs + " Joueurs");
                    for (int i = 0; i < nbJoueurs; i++)
                    {
                        Console.WriteLine("\nJoueur " + (i + 1));
                        bool NomValide = false;
                        while (!NomValide)
                        {
                            Console.Write("saisir le nom du joueur : ");
                            NomJoueur = Console.ReadLine();
                            if (!ListeNoms.Contains(NomJoueur))
                            {
                                ListeNoms.Add(NomJoueur);
                                joueurs.Add(new Joueur(NomJoueur));
                                NomValide = true;
                            }
                            else Console.WriteLine("nom deja utilisé, veuillez saisir un autre nom");
                        }
                    }
                }
                bool PlateauValide = false;
                int taille;
                string FileName;
                while (!PlateauValide)
                {
                    Console.Clear();
                    Console.WriteLine("=== CHOIX DU PLATEAU ===\n\nChoisissez un plateau :\n1 - Plateau aléatoire\n2 - Petit Plateau\n3 - Grand Plateau\n4 - Plateau personnalisé (depuis un fichier)");
                    cki = Console.ReadKey();
                    switch (cki.Key)
                    {
                        case ConsoleKey.D1:
                            Console.Write("\nla taille du plateau : ");
                            taille = Convert.ToInt32(Console.ReadLine());
                            p = new Plateau(taille,taille);
                            PlateauValide = true;
                            break;
                        case ConsoleKey.NumPad1:
                            Console.Write("\nsaisir la taille du plateau : ");
                            taille = Convert.ToInt32(Console.ReadLine());
                            p = new Plateau(taille, taille);
                            PlateauValide = true;
                            break;
                        case ConsoleKey.D2:
                            p = new Plateau("PetitPlateau.txt");
                            PlateauValide = true;
                            break;
                        case ConsoleKey.NumPad2:
                            p = new Plateau("PetitPlateau.txt");
                            PlateauValide = true;
                            break;
                        case ConsoleKey.D3:
                            p = new Plateau("GrandPlateau.txt");
                            PlateauValide = true;
                            break;
                        case ConsoleKey.NumPad3:
                            p = new Plateau("GrandPlateau.txt");
                            PlateauValide = true;
                            break;
                        case ConsoleKey.D4:
                            Console.Write("\nsaisir le nom du fichier : ");
                            FileName = "" + Console.ReadLine();
                            p = new Plateau(FileName);
                            PlateauValide = true;
                            break;
                        case ConsoleKey.NumPad4:
                            Console.Write("\nsaisir le nom du fichier : ");
                            FileName = "" + Console.ReadLine();
                            p = new Plateau(FileName);
                            PlateauValide = true;
                            break;
                        default:
                            Console.WriteLine("entrée invalide, veuillez réessayer");
                            Console.ReadKey();
                            break;
                    }
                }
                Console.Clear();
                Console.WriteLine("=== DUREE DE LA PARTIE ===");
                Console.Write("\nsaisir la durée de la partie (en minutes) : ");
                int tPartie = Convert.ToInt32(Console.ReadLine());
                int tTour;
                if (joueurs.Count > 1)
                {
                    Console.Write("\nsaisir la durée d'un tour (en secondes) : ");
                    tTour = Convert.ToInt32(Console.ReadLine());
                }
                else tTour = 30;
                Jeu j = new Jeu(p, d, joueurs, tPartie, tTour);
                j.Lancer();

                Console.WriteLine("Voulez-vous rejouer ?");
                Console.WriteLine("<Entree> pour rejouer, <Echap> pour quitter");
                bool actionValide = false;
                bool choixJoueurs = false;
                while (!actionValide)
                {
                    cki = Console.ReadKey();
                    if (cki.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine("\nrejouer avec les mêmes joueurs (y/n) ?");
                        while (!choixJoueurs)
                        {
                            cki = Console.ReadKey();
                            if (cki.Key==ConsoleKey.Y) choixJoueurs = true;
                            if(cki.Key==ConsoleKey.N)
                            {
                                nbJoueurs = 0;
                                choixJoueurs = true;
                            }
                        }
                        actionValide = true;
                    }  
                    if (cki.Key == ConsoleKey.Escape)
                    {
                        endGame = true;
                        actionValide = true;
                    }
                }
            }
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            //Test_Joueur();
            //Test_Dictionnaire();
            //Test_Plateau();
            //Test_Jeu();
            //Console.ReadKey();

            InitierJeu();
        }
    }
}
