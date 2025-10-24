# Shooter (Skele-Strike: Skeleton Arena)

This project is built mainly as a CV game example for anyone reviewing my code.  

Sadly, game was removed from YandexGames due to low rating (no players) 😔  
Project only supports WEBGL with ability to switch between Yandex or non-Yandex builds.  
By default (with current project settings) unity will build a Yandex build.  

```main``` branch has the latest changes.  
```pages-builds``` branch has the latest non-yandex build.  
```shooter-86``` is tag with latest yandex build.  

Game is meant to be booted from Init scene.  

## Play here

https://fazamkg.github.io/shooter/

## How to build non-Yandex build:

1) Disable compression (Github Pages does not like Brotli or Gzip)
2) Select Default or Minumal presentation template
3) Remove ```YG_PLUGIN_YANDEX_GAME``` define from project settings
4) Disable YandexGame GameObject from Init scene
5) Build 

## Code

This is also not the best code I have written.  
I tried to follow atleast some MV separation here for UI code.  
Check out my own custom console feature. It's pretty handy when testing stuff in builds.  
Also I did a little bit of shaders in this project. Check them out under Assets/_Shaders
