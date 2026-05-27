module Game.Engine
open Game.UpdateFunctionsInGame
open Game.Utils
open Game.types
//
// Juego from scratch
//
open System
open System.Threading
open Game.States

let rec mainLoop state =
    state
    |> actualizarTick
    |> updateClock
    |> actualizarMisiles
    |> actualizarEnemigo
    |> actualizarDisparoEnemigo
    |> actualizarMisilesEnemigos
    |> detectarColisionConAlien
    |> detectarColisionConEnemigo
    |> resetAlien
    |> resetEnemigo
    |> CheckGameOver
    |> procesarTeclado
    |> redibujarPantalla
    |> fun nuevoEstado ->
        match nuevoEstado.ProgramState with
        | Finished -> nuevoEstado
        | GameOver -> nuevoEstado 
        | _ -> 
             Thread.Sleep 25
             nuevoEstado |> mainLoop

Console.Clear()
Console.CursorVisible <- false

let mostrarGame estadoEntrada =
    let game =
        Console.CursorVisible <- false
        estadoEntrada 
        |> mainLoop
    game



