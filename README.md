# Flowers For You MR

Projecte Unity de realitat mixta (MR) que adapta el videojoc **Flowers For You** a una experiència cozy on l’usuari prepara rams, atén clients i interactua amb una floristeria integrada amb l’espai real.

## Contingut
- Descripció
- Característiques principals
- Requeriments
- Instal·lació i execució
- Estructura del repositori
- Flux de treball i convencions
- Notes de desenvolupament i testing
- Equip i contactes
- Llicència

## Descripció
Flowers For You MR és una adaptació immersiva del joc original desenvolupat a Projecte III. L’usuari es troba dins la floristeria i manipula flors i eines en un taulell tridimensional ancorat a l’espai real, amb una experiència centrada en la calma, la creativitat i la interacció manual.

En aquesta versió, la realitat mixta permet combinar l’entorn físic amb elements virtuals, fent que la floristeria es desplegui parcialment sobre l’espai real de l’usuari. Això reforça la sensació de presència i converteix la preparació dels rams en una experiència més natural i immersiva.

## Característiques principals
- Experiència single-player amb estètica low-poly càlida.
- Taula de treball 3D ancorada a l’espai real per muntar rams.
- Interacció amb objectes: agafar, girar, col·locar i usar eines com tisores.
- Sistema de comandes, validació de rams i progressió mitjançant estrelles i desbloquejos.
- Diàlegs i interacció amb NPCs integrats espacialment.
- Modes assegut i dret, amb opcions de confort per afavorir l’accessibilitat.

## Requeriments
- Unity amb la versió indicada al fitxer `ProjectSettings/ProjectVersion.txt`.
- Paquets recomanats: XR Interaction Toolkit i Input System.
- Dispositiu compatible amb realitat mixta o realitat virtual.
- PC amb els requisits mínims de rendiment per executar el projecte.
- Configuració correcta del runtime XR i del dispositiu objectiu.

## Instal·lació i execució
1. Clona el repositori:
   ```bash
   git clone <url-del-repositori>
   ```
2. Obre Unity Hub i selecciona la versió de Unity indicada pel projecte.
3. Obre el projecte des de l’arrel del repositori.
4. Deixa que Unity importi tots els paquets i assets.
5. Configura el sistema XR des de `Project Settings > XR Plug-in Management`.
6. Obre l’escena principal del projecte.
7. Prem Play o genera el build segons el dispositiu objectiu.

## Estructura del repositori
```bash
.
├── Assets/
├── Packages/
├── ProjectSettings/
├── Docs/
├── Builds/
└── README.md
```

- `Assets/`: codi, escenes, prefabs, materials, sons i models.
- `Packages/`: paquets del projecte Unity.
- `ProjectSettings/`: configuració general del projecte.
- `Docs/`: documentació del projecte, conceptualització i guies.
- `Builds/`: compilacions de prova o entregues internes.

## Flux de treball i convencions
- `main`: versió estable del projecte.
- `develop`: integració de funcionalitats.
- `feature/<nom>`: branques per a noves funcionalitats.

### Gestió d’assets
- Els assets pesats s’han de controlar amb cura.
- Si cal, utilitzar Git LFS o un espai extern d’emmagatzematge compartit.
- Evitar pujar binaris innecessaris al repositori principal.

## Notes de desenvolupament i testing
- Fer proves freqüents dins del dispositiu real per validar confort i usabilitat.
- Prioritzar teleport o moviment segur com a opció principal.
- Permetre ajustar la posició, mida i distància dels elements virtuals.
- Mantenir una interfície clara, llegible i sense sobrecàrrega visual.
- Afegir feedback visual i sonor en les interaccions importants.
- Provar tant en mode assegut com dret per garantir accessibilitat.

## Equip i contactes
- **Lucía Curiel** 
- **Aura Espí** 
- **Íngrid Lara** 
- **Adriana Solís** 
- **Cristina Moles** 

## Llicència
Projecte acadèmic. Tots els drets sobre el contingut, els assets i el disseny corresponen a l’equip desenvolupador.

---

> Projecte desenvolupat com a adaptació en realitat mixta de *Flowers For You*.
