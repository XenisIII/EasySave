# Programmation-Système

## Rappel du contexte : 

Présentation de Prosoft

Votre équipe vient d'intégrer l'éditeur de logiciels ProSoft. Sous la responsabilité du DSI, vous aurez la responsabilité de gérer le projet “EasySave” qui consiste à développer un logiciel de sauvegarde.

Comme tout logiciel de la Suite ProSoft, le logiciel s'intégrera à la politique tarifaire.

    Prix unitaire : 200 €HT

    Contrat de maintenance annuel 5/7 8-17h (mises à jour incluses): 12% prix d'achat (Contrat annuel à tacite reconduction avec revalorisation basée sur l'indice SYNTEC)

Lors de ce projet, votre équipe devra assurer le développement, la gestion des versions majeures et mineures, mais aussi les documentations

    pour les utilisateurs : manuel d'utilisation (sur une page)

    pour le support client : Informations nécessaires pour le support technique (Emplacement par défaut du logiciel, Configuration minimale, Emplacement des fichiers de configuration...)

Pour garantir une reprise de votre travail par d'autres équipes, la direction vous impose de travailler dans le respect des contraintes suivantes :

    Outils et méthodes

        Visual Studio 2019 16.3 ou supérieure

        GIT Azure DevOps.

        Editeur UML : Nous préconisations l'utilisation de ArgoUML

        "« Tous vos documents et l'ensemble des codes doivent être gérés dans ces outils. »"

        "« Votre responsable (tuteur ou pilote) doit être invité sur votre GIT pour pouvoir suivre vos développements »"

    Langage, FrameWork

        Langage C#

        Bibliothèque Net.Core 5.X

    Lisibilité et maintenabilité du code :

        L'ensemble des documents, lignes de codes et commentaires doivent être exploitables par les filiales anglophones.

        Le nombre de lignes de code dans une fonction doit être raisonnable

        La redondance des lignes de code est à proscrire (une vigilance particulière sera faite sur les copier-coller)

        Respect des conventions de nommage

    Autres :

        La documentation utilisateur doit tenir en une seule page

        Release note obligatoire

Vous devez conduire ce projet de manière à réduire les coûts de développement des futures versions et surtout d'être capable de réagir rapidement à la remontée éventuelle d'un dysfonctionnement.

    Gestion des versions

    Limiter au maximum les lignes de code dupliquées

Le logiciel devant être distribué chez les clients, il est impératif de soigner les IHM.


## Livrable 1 et cahier des charges : 

Le cahier des charges de la première version du logiciel est le suivant :

    Le logiciel est une application Console utilisant .Net Core.

    Le logiciel doit permettre de créer jusqu'à 5 travaux de sauvegarde

    Un travail de sauvegarde est défini par

        Un nom de sauvegarde

        Un répertoire source

        Un répertoire cible

        Un type (complet, différentiel)

    Le logiciel doit être utilisable à minima par des utilisateurs anglophones et Francophones

    L'utilisateur peut demander l'exécution d'un des travaux de sauvegarde ou l'exécution séquentielle de l'ensemble des travaux.

    Le programme peut être lancé par une ligne de commande

        exemple 1 : 1-3 pour exécuter automatiquement les sauvegardes 1 à 3

        exemple 2 : 1 ;3 pour exécuter automatiquement les sauvegardes 1 et 3

    Les répertoires (sources et cibles) pourront être sur :

        Des disques locaux

        Des disques Externes

        Des Lecteurs réseaux

    Tous les éléments du répertoire source (fichiers et sous-répertoires ) doivent être sauvegardé.

    Fichier Log journalier :

    Le logiciel doit écrire en temps réel dans un fichier log journalier l'historique des actions des travaux de sauvegarde. Les informations minimales attendues sont :

        Horodatage

        Nom de sauvegarde

        Adresse complète du fichier Source (format UNC)

        Adresse complète du fichier de destination (format UNC)

        Taille du fichier

        Temps de transfert du fichier en ms (négatif si erreur)

        Exemple de contenu: 2020-12-17.json [json]

    Ficher Etat temps réel : Le logiciel doit enregistrer en temps réel, dans un fichier unique, l'état d'avancement des travaux de sauvegarde. Les informations à enregistrer pour chaque travail de sauvegarde sont :

        Appellation du travail de sauvegarde

        Horodatage

        Etat du travail de Sauvegarde (ex : Actif, Non Actif...)

    Si le travail est actif :

        Le nombre total de fichiers éligibles

        La taille des fichiers à transférer

        La progression

            Nombre de fichiers restants

            Taille des fichiers restants

            Adresse complète du fichier Source en cours de sauvegarde

            Adresse complète du fichier de destination

        exemple de contenu : state.json [json]

    Les emplacements des deux fichiers (log journalier et état temps réel) devront être étudiés pour fonctionner sur les serveurs des clients. De ce fait, les emplacements du type « c:\temp\ » sont à proscrire.

    Les fichiers (log journalier et état) et les éventuels fichiers de configuration seront au format JSON. Pour permettre une lecture rapide via Notepad, il est nécessaire de mettre des retours à la ligne entre les éléments JSON. Une pagination serait un plus.

Remarque importante : si le logiciel donne satisfaction, la direction vous demandera de développer une version 2.0 utilisant une interface graphique WPF (basée sur l'architecture MVVM)


## UML : 

### Diagramme des cas d'utilisation :
<img width="992" alt="Diagramme use case" src="https://github.com/XenisIII/Programmation-Systeme/assets/62725768/7a4ff413-f6ab-4a15-b6ca-b33de2a571b7">

### Diagramme de classe :
![Diagramme_class drawio](https://github.com/XenisIII/Programmation-Systeme/assets/62725768/61eed5b1-1fac-48dc-9ee2-6f1d861df7f5)

### Diagramme de séquence :
![Diagramme_Sequence drawio](https://github.com/XenisIII/Programmation-Systeme/assets/62725768/02ebc36d-e288-4a81-95d7-666dc56022b3)

### Diagramme d'activité : 
![UML Activite progsys drawio(1)](https://github.com/XenisIII/Programmation-Systeme/assets/62725768/8739a4e4-7c97-4890-94e2-ca3fb5bab522)

