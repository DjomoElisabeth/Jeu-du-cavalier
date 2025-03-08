Synthèse des Fonctions du Programme (Jeu du Cavalier en C#)

button1_Click(object sender, EventArgs e)
 Rôle : 
- Gère le déplacement du cavalier.  
- Vérifie si le déplacement est valide (suivant les règles du cavalier).  
- Désactive les cases déjà visitées.  
- Met à jour les mouvements possibles en affichant des "X".  
- Vérifie si le cavalier est bloqué, et propose une réinitialisation si nécessaire.


button66_Click(object sender, EventArgs e)
 Rôle : 
- Active toutes les cases de la grille pour permettre à l'utilisateur de choisir manuellement la position initiale du cavalier.  
- Désactive les boutons de sélection (`button65`, `button66`) pour éviter un recliquage.


button65_Click(object sender, EventArgs e)
 Rôle :  
- Sélectionne aléatoirement une case sur l’échiquier pour placer le cavalier.  
- Désactive les boutons de sélection après le choix.  
- Initialise les coordonnées du cavalier et met à jour l'affichage.


CoordonneBtn(int x, int y)
 Rôle :   
- Calcule les cases où le cavalier peut se déplacer.  
- Vérifie si les déplacements sont possibles en restant dans les limites de l’échiquier.  
- Affiche des "X" sur les cases valides.  
- Met à jour l'état `cavalierBlocked`.


BtnBloque(int x, int y) -> bool
 Rôle :   
- Vérifie si le cavalier est bloqué (aucune case disponible).  
- Retourne `true` si un déplacement est encore possible, `false` sinon.


Init_Grille()
 Rôle :  
- Réinitialise l’échiquier (8x8).  
- Associe chaque case à un bouton et lui attribue ses coordonnées (`Tag`).  
- Applique les couleurs de l’échiquier en mode damier.  
- Désactive toutes les cases en attendant la sélection du cavalier.


button67_Click_1(object sender, EventArgs e)
 Rôle :   
- Gère le bouton "Restart".  
- Affiche une confirmation avant de réinitialiser le jeu.  
- Vide l’échiquier, réinitialise les variables et désactive les boutons inutiles (`button68`, `button67`).  


button68_Click(object sender, EventArgs e)
 Rôle : 
- Permet de revenir en arrière (annuler le dernier déplacement).  
- Replace le cavalier sur la case précédente.  
- Réaffiche les déplacements possibles.  
- Désactive le bouton `button68` après l’annulation.



