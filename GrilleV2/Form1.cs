using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace GrilleV2
{
    
    public partial class Form1 : Form
    {

        /***********************************************************************
        * Nom du projet   : Jeu du Cavalier
        * Langage         : C# (WinForms)
        * Auteur          : DJOMO Elisabeth
        * Date            : 08/03/2025
        * Description     :
        *     Cette application implémente le parcours du cavalier sur un échiquier.
        *     L'objectif est que le cavalier visite chaque case sans jamais repasser
        *     deux fois sur la même.
        * 
        * Fonctionnalités :
        *     - Affichage d'un échiquier sous forme de grille (8x8).
        *     - Indication des déplacements du cavalier.
        *     - Menu permettant de personnaliser la grille.
        ************************************************************************/


        private Button[,] grille = new Button[8, 8]; // creation de la grille
        private bool CouleurBtn;
        private int randomPosition = 1;

        private Button dernierBouton; // Stocke le dernier bouton cliqué
                                      //sert a effacer l'image dans le bouton précédent
                                      //sert a verifier  si on click sur le premier bouton

        
        private Button boutonCourant;
        private static int N = 64; //nombre de bouton sur une grille 8x8

        private Point positionDernierBtn; //utile pour l'option de retour

        private bool cavalierBlocked = false; //verifie si le cavalier ne peut plus se déplacer

        private static int[,] deplacementsCavalier = {
        { 2,  1}, { 2, -1}, {-2,  1}, {-2, -1},
        { 1,  2}, { 1, -2}, {-1,  2}, {-1, -2}};


        private static int numeroClick = 0; //affiche le numero de deplacement du cavalier
        Image cavalier;
        Image smiley;
        Image smiley_T;


        /************************** INITIALISATION *******************************************/
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            CouleurBtn = true;

            cavalier = Image.FromFile("./image/cavalier-min.png"); //l'Image doit etre de taille 100x100
            smiley = Image.FromFile("./image/smiley.jpg");
            smiley_T = Image.FromFile("./image/smiley_triste.jpg");
            label2.Text = N.ToString(); //affiche le nombre de boutons restant sur la grille

            button68.Enabled = false;  //bouton  d'annulation "back"

            dernierBouton= null;  //on a appuyer sur aucun bouton de la grille
            boutonCourant = null;
            button67.Enabled = false; //bouton "restart"

            pictureBox1.Image = smiley;

            Init_Grille();

        }

/************************** INITIALISATION DE LA GRILLE ******************************/
        private void Init_Grille()
        {
            int cpt = 0;

            for (int i = 0; i < 8; i++)
            {
                CouleurBtn = !CouleurBtn; //un changement de ligne la couleur du carreau doit changer

                for (int j = 0; j < 8; j++)
                {
                    cpt++;

                    Button btn = this.Controls.Find("button" + cpt, true).FirstOrDefault() as Button;
                    if (btn != null)
                    {
                        CouleurBtn = !CouleurBtn; //un changement de colonne la couleur du carreau doit changer
                       
                        btn.Text = " ";
                        grille[i, j] = btn;

                        btn.Tag = new Point(i, j); // Stocke les coordonnées du bouton crée

                        if (CouleurBtn)
                        {
                            grille[i, j].BackColor = Color.DarkGoldenrod;
                        }

                    }
                    grille[i, j].Enabled = false;
                }

            }

        }


 /************************* GESTION DES CLICKS ******************************************/
        private void button1_Click(object sender, EventArgs e)
        {
     
            button67.Enabled = true;  // activation du bouton restart
            button68.Enabled = true;  //activation du bouton retour

            Button btn = new Button();
            btn = (Button)sender;
           

            //si on appuie sur un bouton qui n'est pas dans les deplacement
            //d'un cavalier dans un jeu d'echec alors on ne prend pas en compte
            //le dernierBouton != null sert a verifier uniquement si on ne joue pas le 
            //premier coup
            if (!btn.Text.Equals("X") && dernierBouton != null) return;

            numeroClick++;
            label2.Text = (N-numeroClick).ToString(); //on mets a jour le nombre de bouton restant

            // Effacer l'image du dernier bouton cliqué
            if (dernierBouton != null) //different de null si on a deja cliqué sur un bouton
            {
                dernierBouton.Image = null;  //on efface l'image
                positionDernierBtn = (Point)dernierBouton.Tag;  //recupere des coordonées du bouton
                dernierBouton.Enabled = false; //on parcours une case une seule fois
            }

            // Assigner l'image du cavalier au nouveau bouton
            btn.Image = cavalier; //on assigne l'image au nouveau bouton
            btn.Text = numeroClick.ToString();
           
            // Mettre à jour le dernier bouton sélectionné
            dernierBouton = btn;  
        

            //cette boucle sert a afficher les X en tenant compte du nouvelle 
            //bouton appuyer en effacant ceux des boutons precedents
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {        
                    if (grille[i, j].Text.Equals("X")) //on efface les anciens indications lorsqu'on click
                                                        //sur un nouveau bouton
                       grille[i, j].Text =  " ";
                }
            }
            
            // Récupérer les coordonnées du bouton cliqué
            // Récupérer les coordonnées stockées dans `Tag`

            Point position = (Point)btn.Tag;  //recupere des coordonées du bouton
            int x = position.X;
            int y = position.Y;

            boutonCourant = grille[position.X, position.Y];

            CoordonneBtn(x, y);          

            if (!BtnBloque(x, y))
            {
                pictureBox1.Image = smiley_T;
                MessageBox.Show("Le cavalier est bloqué ! Plus de déplacements possibles.");
                

            }
                       
    }

        /*cette fonction choisit une position aleatoire pour positionné le 
         * cavalier au debut du jeu */
        private void button65_Click(object sender, EventArgs e) //random positionning
            
        {
           
            Random r = new Random();
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++)
                {
                    grille[i, j].Enabled = true;
                   
                }
            }
        
            randomPosition = r.Next(1, N+1); // compris entre 1 et 64


           //on trouve un bouton dans le form qui s'appelle buttonrandomPosition
           //et on l'affecte a btn
            Button btn = this.Controls.Find("button" + randomPosition, true).FirstOrDefault() as Button;

            if (btn != null)
            {
                numeroClick++;
                btn.Image = cavalier;
                btn.Text = numeroClick.ToString();
            }
            dernierBouton = btn; //on mets a jour le bouton qui a afficiher l'image

            button65.Enabled = false;
            button66.Enabled = false;  //on desactive l'autre option

            Point position = (Point)btn.Tag;  //recupere des coordonées du bouton
            int x = position.X;
            int y = position.Y;

            CoordonneBtn(x, y);
  
        }

        //choisir la position du cavalier
        private void button66_Click(object sender, EventArgs e)  
        {
          
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    grille[i, j].Enabled = true;
                }
            }
            //le bouton appuyer est mis a jour automatiquement puisque l'on clique sur le bouton
            //la fonction de clique sur le bouton gere la mise a jour

            // Désactive les boutons pour éviter un recliquage après la sélection
            button65.Enabled = false;
            button66.Enabled = false;  

        }

        //Gestion du click sur le bouton "Restart"
        private void button67_Click_1(object sender, EventArgs e)
        {

            DialogResult reponse = MessageBox.Show(
            "Etes-vous sur de vouloir reinitialiser la partie ?",
            "Abandon",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2,
            MessageBoxOptions.RightAlign);
            if (reponse == DialogResult.No) return;
            else
            {
                int cpt = 0;

                for (int i = 0; i < 8; i++)
                {


                    for (int j = 0; j < 8; j++)
                    {
                        cpt++;

                        Button btn = this.Controls.Find("button" + cpt, true).FirstOrDefault() as Button;
                        if (btn != null)
                        {
                            //btn.Text = cpt.ToString() ; // affiche les numeros des boutons
                            btn.Text = " ";
                            grille[i, j] = btn;
                            btn.Image = null;

                        }
                        grille[i, j].Enabled = false; //on attends qu'il fasse le choix sur la position in
                                                      //initiale du cavalier
                    }

                }
                button65.Enabled = true;
                button66.Enabled = true;  //on desactive l'autre option
                button67.Enabled = false;  //on desactive l'option de restart
                numeroClick = 0;
                dernierBouton = null; //on initialise pour que l'appuie soit pris en compte

                pictureBox1.Image = smiley;

                label2.Text = N.ToString();
                button68.Enabled = false;

            }

        }

        // Gestion du click sur le bouton back
        private void button68_Click(object sender, EventArgs e)
        {
            numeroClick--;
            pictureBox1.Image = smiley;

            // Déplacer le cavalier en arrière
            Button btn = grille[positionDernierBtn.X, positionDernierBtn.Y];
            btn.Image = cavalier;
            btn.Enabled = true;           

            boutonCourant.Image = null;
            boutonCourant.Text = " ";
              

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (grille[i, j].Text.Equals("X")) //on efface les anciens indications lorsqu'on click
                                                       //sur un nouveau bouton
                        grille[i, j].Text = " ";
                }
            }

            //on recalcule les points assessible
            CoordonneBtn(positionDernierBtn.X, positionDernierBtn.Y);
            dernierBouton = btn;

            button68.Enabled = false;
        }

        

 /*********************** GESTION DES DÉPLACEMENTS ***********************************/
        /*sert a acalculer les coordonnées du bouton et afficher où le cavalier peut se déplacer*/
        private void CoordonneBtn(int x , int y)
        {
            // Vérification des cases valides
            for (int i = 0; i < 8; i++)
            {
                int nx = x + deplacementsCavalier[i, 0];

                int ny = y + deplacementsCavalier[i, 1];
               
                if (nx >= 0 && nx < 8 && ny >= 0 && ny < 8 && grille[nx, ny].Enabled) // Vérification des limites
                {
                    //grille[nx, ny].Enabled //evite d'effacer le chiffre correspondant au numero de 
                                                //deplacement du cavalier
                        grille[nx, ny].Text = "X";
                    cavalierBlocked = false;
                }
            }
            cavalierBlocked = true;
        }

        /* sert a verifier si le cavalier est bloqué*/
        private bool BtnBloque(int x, int y)
        {
            // Vérification des cases valides
            for (int i = 0; i < 8; i++)
            {
                int nx = x + deplacementsCavalier[i, 0];

                int ny = y + deplacementsCavalier[i, 1];

                if (nx >= 0 && nx < 8 && ny >= 0 && ny < 8 && grille[nx, ny].Enabled) // Vérification des limites
                {
                    //grille[nx, ny].Enabled //evite d'effacer le chiffre correspondant au numero de 
                    //deplacement du cavalier
                    grille[nx, ny].Text = "X";
                    return true;
                }
            }
            return false;

        }


 /******************************GESTION DE l'INTERFACE**********************************************/

        //fonction qui gere la couleur de la grille
        //l'utilisateur peut changer la couleur de la grille
        private void rougeToolStripMenuItem_Click(object sender, EventArgs e)

        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item != null)
            {
                string couleur = item.Text;
                for (int i = 0; i < 8; i++)
                {
                    CouleurBtn =! CouleurBtn;

                    for (int j = 0; j < 8; j++)
                    {
                        CouleurBtn = !CouleurBtn;

                        if (CouleurBtn)
                        {
                            if(couleur.Equals("Rouge")) grille[i, j].BackColor = Color.Red;
                            if (couleur.Equals("Chocolat")) grille[i, j].BackColor = Color.Chocolate;
                            if (couleur.Equals("Violet")) grille[i, j].BackColor = Color.Violet;
                            if (couleur.Equals("Turquoise")) grille[i, j].BackColor = Color.Turquoise;
                            if (couleur.Equals("Burlywood")) grille[i, j].BackColor = Color.BurlyWood;
                            if (couleur.Equals("DarkKhaki")) grille[i, j].BackColor = Color.DarkKhaki;
                            if (couleur.Equals("Darkmagenta")) grille[i, j].BackColor = Color.DarkMagenta;
                            if (couleur.Equals("Wheat")) grille[i, j].BackColor = Color.Wheat;

                        }



                    }

                }
            }     
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult reponse = MessageBox.Show(
            "Etes-vous sur de vouloir quitter la partie ?",
            "Sortie",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2,
            MessageBoxOptions.RightAlign);
            if (reponse == DialogResult.No) e.Cancel = true;
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void couToolStripMenuItem_Click(object sender, EventArgs e)
        {
          MessageBox.Show(" Le but de cette application graphique WinForms (C#), " +
                         "est de faire parcourir à un cavalier l'ensemble d'un " +
                         "échiquier sans passer deux fois sur la même case" ,
                         "JEU DE L'ECHIQUIER" , 
          MessageBoxButtons.OK,
          MessageBoxIcon.Information,
          MessageBoxDefaultButton.Button1,
          MessageBoxOptions.RtlReading);
        }

    }
}
