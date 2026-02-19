# 🥽 Mini Projet XR : Optimisation Core & Systèmes Avancés

Ce dépôt regroupe un ensemble de travaux pratiques avancés en Réalité Virtuelle sous Unity. Le projet est divisé en deux grands piliers d'ingénierie : l'**optimisation de l'architecture logicielle** pour garantir les performances (FPS), et le développement de **systèmes visuels et interactifs complexes** (Shaders, IA, IK).

## 🧭 Navigation dans le projet (Branches & Scènes)

Pour faciliter l'évaluation et la compréhension de l'évolution du code, le dépôt est structuré de manière itérative via des branches Git.

### ⚡ Partie 1 : Optimisation & Architecture
**Scène principale :** `MainXRScene`  
*Cette partie se concentre sur la restructuration d'un prototype de tir (Shooter XR) en un système robuste pour casques autonomes (Meta Quest) et PCVR.*

* 🌿 **Branche `Part-1(cleanup architecture)` :**
  * **Centralisation** : Création d'un `GameplayManager` agissant comme *Single Source of Truth*.
  * Découplage complet des scripts de Spawner et de Cibles.
* 🌿 **Branche `Part-2(optimisation Coroutine)` :**
  * **Économie CPU** : Remplacement systématique des méthodes `Update()` par des **Coroutines** cadencées (rafraîchissement UI, boucle de spawn, nettoyage).
* 🌿 **Branche `Part-3(Pooling Addressables)` :**
  * **Gestion Mémoire** : Implémentation de l'**Object Pooling** pour les projectiles et les cibles, évitant les pics du *Garbage Collector*.
  * **Assets Dynamiques** : Utilisation d'**Unity Addressables** pour charger les effets visuels (Muzzle flash) et les modèles de manière asynchrone selon la plateforme détectée.
  * [Vidéo Démo](https://youtu.be/_37_xXRil1U) : [![Demo Video](https://img.youtube.com/vi/_37_xXRil1U/maxresdefault.jpg)](https://youtu.be/_37_xXRil1U)

---

### 🎨 Partie 2 : Modélisation, Shaders & IA
**Scène principale :** `Industrial_Map_VR`  
*Cette partie explore l'immersion visuelle et interactive du joueur.*

* 🌿 **Branche `TP3-shader` : Shaders URP Avancés**
  * **Rim Lighting (Arme) :** Shader customisé exploitant le produit scalaire $1 - \text{dot}(\vec{N}, \vec{V})$ pour illuminer les contours de l'arme et améliorer la lisibilité en VR.
  * **X-Ray Objective :** Passe de rendu personnalisée ignorant le *Depth Buffer* (`ZTest Always`) couplée à un effet Fresnel pour voir l'objectif à travers les murs.
  * [Vidéo Démo](https://youtu.be/Ca144OzV-lY) : [![Demo Video](https://img.youtube.com/vi/Ca144OzV-lY/maxresdefault.jpg)](https://youtu.be/Ca144OzV-lY)
* 🌿 **Branche `TP4-animation` : Incarnation & Avatar (IK/FK)**
  * **Hybridation** : Utilisation d'un *Avatar Mask* pour séparer la locomotion procédurale (bas du corps) des interactions physiques (haut du corps).
  * **Inverse Kinematics (IK)** : Alignement dynamique des bras, coudes et mains de l'avatar sur les contrôleurs XR. Synchronisation fluide de la tête avec le casque.
  * [Vidéo Démo](https://youtu.be/knoTEQ1KRI8) : [![Demo Video](https://img.youtube.com/vi/knoTEQ1KRI8/maxresdefault.jpg)](https://youtu.be/knoTEQ1KRI8)
* 🌿 **Branche `TP5-Pathfinding` : Drone Autonome & IA**
  * **Échantillonnage en Spirale Conique** : Génération de directions candidates en 3D pour anticiper les trajectoires.
  * **Évitement d'obstacles** : Utilisation de *SphereCasts* pour évaluer l'espace libre et choisir le meilleur vecteur de déplacement (`SmoothDamp`) vers le joueur.
  * [Vidéo Démo](https://youtu.be/XaDq9zYf6OM) : [![Demo Video](https://img.youtube.com/vi/XaDq9zYf6OM/maxresdefault.jpg)](https://youtu.be/XaDq9zYf6OM)



---

## 🛠️ Stack Technique

* **Moteur :** Unity 2022.3.x (URP)
* **XR :** XR Interaction Toolkit, OpenXR
* **Graphics :** Shader Graph, Custom Render Features, HLSL
* **Systèmes :** Unity Addressables, Animation Rigging (IK)

## 🚀 Installation & Test

1. Clonez ce dépôt sur votre machine locale.
2. Ouvrez le projet avec Unity Hub (version **6000.0.62f1** recommandée).
3. Utilisez la commande `git checkout <nom_de_la_branche>` pour observer chaque étape du développement.
4. Lancez le mode *Play* avec un casque VR connecté ou utilisez le simulateur XR intégré.

---
*Projet réalisé par Alexandre Baudin - Cursus Ingénieur EFREI Paris.*
