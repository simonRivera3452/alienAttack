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
    displayMessage (Console.BufferWidth/2 - String.length $"Puntuación: {state.Puntuacion}") 0 ConsoleColor.Green $"Puntuación: {state.Puntuacion}"
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
    
let displayAlienLives state =
    match state.AlienLives with 
    | 3 -> displayMessage 0 0 ConsoleColor.Red"💖💖💖"
    | 2 -> displayMessage 0 0 ConsoleColor.Red"💖💖"
    | 1 -> displayMessage 0 0 ConsoleColor.Red"💖"
    | 0 -> displayMessage 0 0 ConsoleColor.Red"☠"
    | _ -> displayMessage 0 0 ConsoleColor.Red"What did you do to have more than 3 lives??"

let SpecialABilityAdvise state = 
    if state.Clock % 20 = 0 then 
        displayMessage (Console.BufferWidth/2 - 5) (Console.BufferHeight-2) ConsoleColor.Magenta "SPECIAL ABILITY READY!"
    else
        displayMessage (Console.BufferWidth/2 - 5) (Console.BufferHeight-2) ConsoleColor.Magenta "SPECIAL ABILITY CHARGING..."

let displaySpecialAbility state = 
    match SpecialAbilityCommand with 
    | Activate -> 
        state.Misiles
        |> List.iter ( fun SpecialAbility ->
        displayMessage misil.X misil.Y ConsoleColor.Yellow "=>" )