/*

---------------------------------------- General ----------------------------------------
(Tag/Layer) 
(?) All ground level elements need to have "Ground" Tag
All triggers need to have "Ignore Raycast" Layer
 
(Environment Parent Object)
Ensure it is at (0,0,0) before putting all components inside

(Checkpoint)
Ensure each level spawn point is a check point

(Ice)
Apply 0.5 factor to final drag, and increase rotation speed

(Bumper)
Make sure the red arrow of transform is the surface normal

(Moving Platform)
Make sure first waypoint is the platform's starting position. 
Each waypoint setting determines the path the moving platform takes going to the waypoint. 
The trigger hitbox above needs to be modified if the shape of platform changes to match the new shape, slightly smaller

------------------------------------ PlayerPref Names -----------------------------------
(Settings - Controls)
setting_mouse_enabled (boolean)
setting_mouse_sensitivity (float)
setting_keyboard_enabled (boolean)

(Settings - Display)
setting_resolution_index (int)
setting_width (int)
setting_height (int)

(Settings - Sound)
setting_volume (float)
setting_sfx (float)

(Level Data, X = 1-10)
levelX_time (float)

--------------------------------- Planned Level Settings --------------------------------
The Beginning - No specials
Forest - No specials
+ Buttons/Levers
Desert - Quicksand (kill when stay in too long)
+ Moving platforms
Tundra - Ice
Volcano - Lava (instant kill)
+ Bumper
Neon - Synth style
Night - Vision restriction
+ Blocks that temporarily dissapear when you step on them
Graveyard - Vision restriction, ghosts that pushes (scares) you around
Space - Low gravity
The End - Everything together

---------------------------------------- Credits ----------------------------------------

------ Fonts ------
(Chakra Petch)
https://fonts.google.com/specimen/Chakra+Petch

------ Songs & SFX ------

(Main Menu Song)
Super Friendly by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/4442-super-friendly
License: https://filmmusic.io/standard-license

(Level Select Song)
Deliberate Thought by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/3635-deliberate-thought
License: https://filmmusic.io/standard-license

(The Beginning)
Pamgaea by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/4193-pamgaea
License: https://filmmusic.io/standard-license

(The Forest)
Skye Cuillin by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/4371-skye-cuillin
License: https://filmmusic.io/standard-license

(The Desert) 
Desert of Lost Souls by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/3640-desert-of-lost-souls
License: https://filmmusic.io/standard-license

(The Tundra)
The Snow Queen by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/4511-the-snow-queen
License: https://filmmusic.io/standard-license

(The Volcano) 
Firebrand by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/3757-firebrand
License: https://filmmusic.io/standard-license

(The Neon)
Screen Saver by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/5715-screen-saver
License: https://filmmusic.io/standard-license

(The Night)
Ethernight Club by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/7612-ethernight-club
License: https://filmmusic.io/standard-license

(The Graveyard)
Graveyard Shift by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/3823-graveyard-shift
License: https://filmmusic.io/standard-license

(The Space)
Space 1990 by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/4399-space-1990
License: https://filmmusic.io/standard-license

(The End)
Truth of the Legend by Kevin MacLeod
Link: https://incompetech.filmmusic.io/song/4551-truth-of-the-legend
License: https://filmmusic.io/standard-license

Click SFX: https://freesound.org/people/moogy73/sounds/425726/
Countdown SFX: https://freesound.org/people/JustInvoke/sounds/446130/ + https://freesound.org/people/JustInvoke/sounds/446142/
Ball Breaking SFX: https://freesound.org/people/Aurelon/sounds/422633/
Ball Dizzied SFX: https://freesound.org/people/guydowsett/sounds/169307/
Time Pickup SFX: https://freesound.org/people/TreasureSounds/sounds/332629/
Victory Jingle: https://freesound.org/people/Tuudurt/sounds/258142/
Defeat Jingle: https://freesound.org/people/Absolutely_CrayCray/sounds/371205/

------ Models ------
Clock Icon (Simple) by S. Paul Michael [CC-BY] (https://creativecommons.org/licenses/by/3.0/) via Poly Pizza (https://poly.pizza/m/fYrnadMR5h9)

Tree by Marc Solà [CC-BY] (https://creativecommons.org/licenses/by/3.0/) via Poly Pizza (https://poly.pizza/m/6Yjt8nIwLsD)

Tree-2 by Marc Solà [CC-BY] (https://creativecommons.org/licenses/by/3.0/) via Poly Pizza (https://poly.pizza/m/cRipmFHCEVU)

Orange tree by Poly by Google [CC-BY] (https://creativecommons.org/licenses/by/3.0/) via Poly Pizza (https://poly.pizza/m/8zS7mfHS0i7)

Wooden fence by Frank Lynam [CC-BY] (https://creativecommons.org/licenses/by/3.0/) via Poly Pizza (https://poly.pizza/m/ecGdK1XryiQ)

Contains assets from ambientCG.com, licensed under CC0 1.0 Universal.

------ Tutorials Used ------

Hold Button
https://unity3d.college/2018/01/30/unity3d-ugui-hold-click-buttons/

Gradient Skybox
https://github.com/aadebdeb/GradientSkybox

Particle Trail
https://www.youtube.com/watch?v=agr-QEsYwD0

Smooth Camera
https://www.youtube.com/watch?v=MFQhpwc6cKE

Scene Transitions (Currently Broken)
https://www.youtube.com/watch?v=CE9VOZivb3I

Shatter Effect
https://answers.unity.com/questions/1006318/script-to-break-mesh-into-smaller-pieces.html

Dust Particles
https://www.youtube.com/watch?v=_Iq_wU_Ry1w

Ring Particle
https://www.youtube.com/watch?v=CVsZ98TSEwI

*/