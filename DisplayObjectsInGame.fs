module Game.DisplayObjectsInGame
open System
open Game.Utils
open Game.types
open Game.States
let dibujarAlien state =
    let sprite =
        if state.AlienState = Alive then 
            "👽"
        else
            "💀"
    displayMessage state.AlienX state.AlienY ConsoleColor.Yellow sprite

let dibujarEnemigo state =
    let sprite =
        if state.EnemigoEstado = Alive then 
            "👾"
        else
            "💥"
    displayMessage state.EnemigoX state.EnemigoY ConsoleColor.Yellow sprite

let displayPuntuación state = 
    displayMessage (Console.BufferWidth/2) 0 ConsoleColor.Green $"Puntuación: {state.Puntuacion}"
let dibujarMisiles state =
    state.Misiles
    |> List.iter ( fun misil ->
        displayMessage misil.X misil.Y ConsoleColor.Yellow "=>" )

let dibujarMisilesEnemigos state =
    state.MisilesEnemigos
    |> List.iter ( fun misil ->
        displayMessage misil.X misil.Y ConsoleColor.Red "<=" )

let displayClock state =
    displayMessageRight 0 ConsoleColor.Cyan $"{state.Clock}"
    
