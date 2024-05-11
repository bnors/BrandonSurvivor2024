EXAMEN FINAL


Brandon Ross

VampireSurvivor Sans Nom

2131047 student id


RÉSUMÉ May 10th 2024:
---------------------

Mon jeu consiste de 3 enemies et 1 boss.
Le player spawn des random Scythe et a un hammer qui tourne en loop
Le player peut level up
Level up = +DMG Hammer et +DMG Scythe et +1 Scythe
Level 10 = boss spawns
Press X pour add 100 XP
Updating Damage Indicator a gauche de l'écran
Le player a 100 HP pour le moment
Un enemy avec weak point (merman)
Un enemy qui shoot des fireball (firedragon)
Un boss qui a un state machine (idle,move,attack)
SoundPlayer avec plusieurs sons d'ambiance
Tilemap trouvée en ligne sur itch.io gratuitement
Tout les assets trouvés en ligne gratuitement (sons aussi)
Pool System
Spawn System
Plusieurs Singleton
Plusieurs Observer
Proxy (merman weak point)
State (boss)


POINTS:
-------

1. Patron Singleton


GameManager.cs: 
Utilise le patron Singleton pour gérer les états global du jeu, garantissant qu'une seule instance de ce manager existe.

SoundPlayer.cs: 
Gère tous les effets sonores et la musique de fond comme un Singleton, assurant une gestion centralisée et unique des audio source.

ObjectPool.cs: 
Implémente le Singleton pour gérer un pool d'objets, évitant la redondance et optimisant les performances.


2. Patron Object Pool


ObjectPool.cs: 
Gère les instances réutilisables d'objets comme les Scythe, les Fireball, et d'autres prefabs d'ennemis, réduisant le coût de création et destruction fréquente d'objets.


3. Patron Observer


Player.cs: 
Implémente un système d'observation avec des événements tels que OnLevelUp et OnPlayerDeath, permettant à d'autres parties du système de réagir à ces changements.

OrbitingWeapon.cs et Scythe.cs: 
Ces scripts s'abonnent à l'événement Player.OnLevelUp pour ajuster leurs dégâts en fonction du niveau du joueur, montrant l'utilisation du patron Observer.


4. Patron Factory


EnemySpawner.cs: 
Fonctionne comme une factory qui crée des instances d'ennemis. Ce script peut générer différents types d'ennemis, ce qui montre une implémentation du patron Factory.


5. Patron Proxy


MermanWeakPoint.cs: 
Sert de proxy pour interagir avec un point faible spécifique d'un ennemi (Merman), déléguant les effets de dégâts (2x DMG) au script principal de l'ennemi lorsqu'un coup est porté.


6. Patron State


BossEnemy.cs: 
Utilise un State Machine pour gérer différents états tels qu'Idle, Move, et Attack, contrôlant les transitions basées sur la proximité du joueur et d'autres logiques de jeu.


7. Progression Gameplay


Player.cs :
gère l'expérience et les niveaux du joueur, augmentant la puissance des armes et des compétences. 
Les ennemis laissent tomber des shard d'XP que le joueur peut collecter pour gagner de l'expérience.

Enemy.cs & EnemySpawner.cs : 
Les ennemis deviennent progressivement plus nombreux et plus forts au fur et à mesure que le joueur avance, augmentant ainsi le défi du jeu.


8. Qualité des éléments visuels et sonores du jeu


Plusieurs sprites et animations utilisées, tilemap/tileset aussi.

Sons gérés par SounPlayer, plusieurs sound effects, 
sons de mouvement (player), plusieurs hit sound, background music, sons d'ambiance


LA FIN